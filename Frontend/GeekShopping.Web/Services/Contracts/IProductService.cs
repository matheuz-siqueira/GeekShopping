using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductModel>> GetAll(); 
    Task<ProductModel> GetById(long id); 
    Task<ProductModel> Create(ProductModel model); 
    Task<ProductModel> Update(ProductModel model); 
    Task<bool> Delete(long id);  
}
