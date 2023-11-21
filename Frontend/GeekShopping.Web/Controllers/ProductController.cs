using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _service;
    public ProductController(IProductService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<IActionResult> ProductIndex()
    {
        var products = await _service.GetAll(); 
        return View(products);
    }

    public IActionResult ProductCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductModel model)
    {
        if(ModelState.IsValid)
        {
            var response = await _service.Create(model); 
            if(response != null)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(model);
    }

    public async Task<IActionResult> ProductUpdate(long id)
    {
        var model = await _service.GetById(id); 
        if(model is not null)
        {
            return View(model);
        }
        return NotFound(); 
    }

    [HttpPost]
    public async Task<IActionResult> ProductUpdate(ProductModel model)
    {
        if(ModelState.IsValid)
        {
            var response = await _service.Update(model);
            if(response is not null)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(model);
    }

    public async Task<IActionResult> ProductDelete(long id)
    {
        var model = await _service.GetById(id);
        if(model is not null)
        {
            return View(model); 
        }
        return NotFound(); 
    }

    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductModel model)
    {   
        var response = await _service.Delete(model.Id); 
        if(response)
        {
            return RedirectToAction(nameof(ProductIndex));
        }
        return View(model);
    }
}
