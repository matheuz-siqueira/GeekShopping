using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductModel>> GetAll(string token); 
    Task<ProductModel> GetById(long id, string token); 
    Task<ProductModel> Create(ProductModel model, string token); 
    Task<ProductModel> Update(ProductModel model, string token); 
    Task<bool> Delete(long id, string token);  
}
