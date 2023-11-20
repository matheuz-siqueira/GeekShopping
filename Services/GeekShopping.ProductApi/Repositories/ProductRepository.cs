using AutoMapper;
using GeekShopping.ProductApi.Data.ValueObjects;
using GeekShopping.ProductApi.Model;
using GeekShopping.ProductApi.Model.Context;
using GeekShopping.ProductApi.Repositories.contracts;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(AppDbContext context, IMapper mapper)
    {
        _context = context; 
        _mapper = mapper; 
    }
    public async Task<IEnumerable<ProductVO>> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        return _mapper.Map<List<ProductVO>>(products); 
    }

    public async Task<ProductVO> GetById(long id)
    {
        var product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync(); 
        return _mapper.Map<ProductVO>(product); 
    }

    public async Task<ProductVO> Create(ProductVO productVO)
    {
        Product product = _mapper.Map<Product>(productVO);
        await _context.Products.AddAsync(product); 
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductVO>(product);     
    }

    public async Task<ProductVO> Update(ProductVO productVO)
    {
        Product product = _mapper.Map<Product>(productVO);
        _context.Products.Update(product); 
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductVO>(product);        
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            var product = await _context.Products
                .Where(p => p.Id == id).FirstOrDefaultAsync(); 

            if(product is null)
            {
                return false; 
            }

            _context.Products.Remove(product); 
            await _context.SaveChangesAsync(); 
            return true; 
        }
        catch(Exception)
        {
            return false; 
        }
    }
}
