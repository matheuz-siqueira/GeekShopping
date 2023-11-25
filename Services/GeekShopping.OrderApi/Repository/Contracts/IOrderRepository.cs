using GeekShopping.OrderApi.Model.Context;

namespace GeekShopping.OrderApi.Repository.Contracts;

public interface IOrderRepository
{
    Task<bool> AddOrder(OrderHeader orderHeader);
    Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid);  
}
