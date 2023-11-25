using GeekShopping.OrderApi.Model.Base;
using GeekShopping.OrderApi.Model.Context;

namespace GeekShopping.OrderApi.Model;

public class OrderDetail : BaseEntity
{
    public long OrderHeaderId { get; set; }
    public virtual OrderHeader OrderHeader { get; set; }
    public long ProductId { get; set; }
    public int Count { get; set; }
    public string ProductName { get; set; } 
    public decimal Price { get; set; }

}
