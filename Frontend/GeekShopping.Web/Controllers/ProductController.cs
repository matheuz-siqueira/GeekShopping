using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Contracts;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        var token = await HttpContext.GetTokenAsync("access_token");
        var products = await _service.GetAll(token); 
        return View(products);
    }


    public IActionResult ProductCreate()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProductCreate(ProductModel model)
    {
        if(ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _service.Create(model, token); 
            if(response != null)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(model);
    }

    public async Task<IActionResult> ProductUpdate(long id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var model = await _service.GetById(id, token); 
        if(model is not null)
        {
            return View(model);
        }
        return NotFound(); 
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ProductUpdate(ProductModel model)
    {
        if(ModelState.IsValid)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _service.Update(model, token);
            if(response is not null)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
        }
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> ProductDelete(long id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var model = await _service.GetById(id, token);
        if(model is not null)
        {
            return View(model); 
        }
        return NotFound(); 
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> ProductDelete(ProductModel model)
    {   
        var token = await HttpContext.GetTokenAsync("access_token");
        var response = await _service.Delete(model.Id, token); 
        if(response)
        {
            return RedirectToAction(nameof(ProductIndex));
        }
        return View(model);
    }
}
