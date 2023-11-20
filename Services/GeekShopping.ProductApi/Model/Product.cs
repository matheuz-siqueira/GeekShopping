using System.ComponentModel.DataAnnotations;
using GeekShopping.ProductApi.Model.Base;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace GeekShopping.ProductApi.Model;

public class Product : BaseEntity
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; }

    [Required]
    [Range(1, 10000)]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [StringLength(50)]
    public string CategoryName { get; set; }

    [StringLength(255)]
    public string ImageUrl { get; set; }
    

}
