using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Contracts;
using GeekShopping.Web.Utils;
using Microsoft.VisualBasic;

namespace GeekShopping.Web.Services;

public class ProductService : IProductService
{

    private readonly HttpClient _client;
    public const string BasePath = "api/v1/product";

    public ProductService(HttpClient client)
    {
        _client = client ?? throw new ArgumentException(nameof(client)); 
    }

    public async Task<IEnumerable<ProductModel>> GetAll()
    {
        var response = await _client.GetAsync(BasePath); 
        return await response.ReadContentAs<List<ProductModel>>();
    }

    public async Task<ProductModel> GetById(long id)
    {
        var response = await _client.GetAsync($"{BasePath}/{id}");
        return await response.ReadContentAs<ProductModel>(); 
    }
    public async Task<ProductModel> Create(ProductModel model)
    {
        var response = await _client.PostAsJons(BasePath, model); 
        if(response.IsSuccessStatusCode)
        {
            return await response.ReadContentAs<ProductModel>(); 
        }
        else 
        {
            throw new Exception("Something went wrong wen calling API");
        }
    }
    public async Task<ProductModel> Update(ProductModel model)
    {
        var response = await _client.PuttAsJons(BasePath, model); 
        if(response.IsSuccessStatusCode)
        {
            return await response.ReadContentAs<ProductModel>(); 
        }
        else 
        {
            throw new Exception("Something went wrong wen calling API");
        }
    }
    public async Task<bool> Delete(long id)
    {
        var response = await _client.DeleteAsync($"{BasePath}/{id}");
        if(response.IsSuccessStatusCode)
        {
            return await response.ReadContentAs<bool>();
        } 
        else 
        {
            throw new Exception("Something went wrong wen calling API");
        }
         
    }

}
