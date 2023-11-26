using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using GeekShopping.PaymentApi.Messages;
using RabbitMQ.Client;

namespace GeekShopping.PaymentApi.RabbitMQSender;

public class RabbitMQMessageSender : IRabbitMQMessageSender
{
    private readonly string _hostName; 
    private readonly string _password; 
    private readonly string _userName; 
    private IConnection _connection;
    private const string ExchangeName = "DirectPaymentUpdateExchange";
    private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";
    private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

    public RabbitMQMessageSender()
    {
        _hostName = "localhost"; 
        _password = "guest"; 
        _userName = "guest"; 
    }
    public void SendMessage(BaseMessage message)
    {
        if(ConnectionExists()){
            using var chanel = _connection.CreateModel(); 
            chanel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);
            chanel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null); 
            chanel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null); 

            chanel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "PaymentEmail");
            chanel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PaymentOrder");


            byte[] body = GetMessageAsByteArray(message);

            chanel.BasicPublish(
                exchange: ExchangeName, routingKey: "PaymentEmail", basicProperties: null, body: body);
        
            chanel.BasicPublish(
                exchange: ExchangeName, routingKey: "PaymentOrder", basicProperties: null, body: body);
        }
    }

    private byte[] GetMessageAsByteArray(BaseMessage message)
    {
        var options = new JsonSerializerOptions
        { 
            WriteIndented = true 
        }; 

        var json = JsonSerializer
            .Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options); 

        var body = Encoding.UTF8.GetBytes(json);
        return body; 

    }

    private bool ConnectionExists()
    {
        if(_connection is not null) 
            return true; 

        CreateConnection();
        return _connection is not null; 
    }

    private void CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
            HostName = _hostName, 
            UserName = _userName, 
            Password = _password
            };
            _connection = factory.CreateConnection();

        }
        catch(Exception)
        {
            throw; 
        }
    }
}
