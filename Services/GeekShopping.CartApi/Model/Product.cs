using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeekShopping.CartApi.Model.Base;

namespace GeekShopping.CartApi.Model;

public class Product
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }

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
