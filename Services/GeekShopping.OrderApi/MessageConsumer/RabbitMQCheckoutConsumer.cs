
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using GeekShopping.OrderApi.Messages;
using GeekShopping.OrderApi.Model;
using GeekShopping.OrderApi.Model.Context;
using GeekShopping.OrderApi.RabbitMQSender;
using GeekShopping.OrderApi.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.OrderApi.MessageConsumer;

public class RabbitMQCheckoutConsumer : BackgroundService
{

    private readonly OrderRepository _repository;
    private IConnection _connection;
    private IModel _channel; 
    private IRabbitMQMessageSender _rabbitMQMessageSender;

    public RabbitMQCheckoutConsumer(OrderRepository repository, 
        IRabbitMQMessageSender rabbitMQMessageSender)
    {
        _repository = repository;
        _rabbitMQMessageSender = rabbitMQMessageSender;
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest", 
            Password = "guest"
        };
        _connection = factory.CreateConnection(); 
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "checkoutqueue",false, false, false, arguments: null);

    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested(); 
        var consumer = new EventingBasicConsumer(_channel); 
        consumer.Received += (channel, evt) => 
        {
            var content = Encoding.UTF8.GetString(evt.Body.ToArray());
            CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
            ProccessOrder(vo).GetAwaiter().GetResult();

            _channel.BasicAck(evt.DeliveryTag, false); 
        
        };

        _channel.BasicConsume("checkoutqueue", false, consumer);
        return Task.CompletedTask;
    }

    private async Task ProccessOrder(CheckoutHeaderVO vo)
    {
        OrderHeader order = new() 
        {
            UserId = vo.UserId, 
            FirstName = vo.FirstName,
            LastName = vo.LastName, 
            OrderDetails = new List<OrderDetail>(), 
            CardNumber = vo.CardNumber, 
            CouponCode = vo.CouponCode, 
            CVV = vo.CVV, 
            DiscountAmount = vo.DiscountAmount, 
            Email = vo.Email, 
            ExpireMonthYear = vo.ExpireMonthYear, 
            OrderTime = DateTime.Now, 
            PaymentStatus = false, 
            Phone = vo.Phone,
            PurchaseDate = vo.DateTime
        }; 

        foreach(var item in vo.CartDetails)
        {
            OrderDetail details = new() 
            {
                ProductId = item.ProductId, 
                ProductName = item.Product.Name, 
                Price = item.Product.Price, 
                Count = item.Count,
            };
            order.TotalItems += details.Count; 
            order.OrderDetails.Add(details);  
        }

        await _repository.AddOrder(order); 
        PaymentVO payment = new()
        {
            Name = order.FirstName + " " + order.LastName, 
            CardNumber = order.CardNumber, 
            CVV = order.CVV, 
            ExpireMonthYear = order.ExpireMonthYear, 
            OrderId = order.Id, 
            PurchaseAmount = order.PurchaseAmount, 
            Email = order.Email 
        };

        try 
        {
            _rabbitMQMessageSender.SendMessage(payment, "orderpaymentprocessqueue");
        }
        catch(Exception)
        {
            throw;
        }
    }
}
