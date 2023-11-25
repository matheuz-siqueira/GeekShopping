using GeekShopping.OrderApi.Model.Base;

namespace GeekShopping.OrderApi.Model.Context;

public class OrderHeader : BaseEntity
{
    public string UserId { get; set; }
    public string CouponCode { get; set; }
    public decimal PurchaseAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime OrderTime { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string CardNumber { get; set; }
    public string CVV { get; set; } 
    public string ExpireMonthYear { get; set; }

    public int TotalItems { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
    public bool PaymentStatus { get; set; }
}
