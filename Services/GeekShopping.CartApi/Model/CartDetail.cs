using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CartApi.Model.Base;

namespace GeekShopping.CartApi.Model;

public class CartDetail : BaseEntity
{
    public long CartHeaderId {get; set;}
    public long ProductId { get; set; }
    public virtual CartHeader CartHeader { get; set; }
    public virtual Product Product { get; set; }
    public int Count { get; set; }

}
