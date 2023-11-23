using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShopping.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GeekShopping.Web.Services.Contracts;

namespace GeekShopping.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _service; 
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService service, 
        ICartService cartService)
    {
        _logger = logger;
        _service = service;
        _cartService = cartService; 
    }

    public async Task<IActionResult> Index()
    { 
        var products = await _service.GetAll(string.Empty); 
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(long id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var model = await _service.GetById(id, token);
        return View(model);  
    }

    
    [HttpPost]
    [Authorize]
    [ActionName("Details")]
    public async Task<IActionResult> DetailsPost(ProductModel model)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        CartViewModel cart = new() 
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value 
            }
        };

        CartDetailViewModel cartDetail = new()
        {
            Count = model.Count, 
            ProductId = model.Id, 
            Product = await _service.GetById(model.Id, token) 
        };

        List<CartDetailViewModel> cartDetails = new List<CartDetailViewModel>
        {
            cartDetail
        };
        cart.CartDetails = cartDetails;
        
        var response = await _cartService.AddItemToCart(cart, token); 
        if(response is not null)
            return RedirectToAction(nameof(Index));   

        return View(model); 
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}
