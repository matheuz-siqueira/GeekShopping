using GeekShopping.Web.Services;
using GeekShopping.Web.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Razor.TagHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IProductService, ProductService>(c => 
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductApi"]);
});

builder.Services.AddHttpClient<ICartService, CartService>(c => 
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartApi"]);
});

builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(20))
.AddOpenIdConnect("oidc", options =>  
{
    options.Authority = builder.Configuration["ServiceUrls:IdentityServer"]; 
    options.GetClaimsFromUserInfoEndpoint = true; 
    options.RequireHttpsMetadata = false; 
    options.ClientId = "geek_shopping";
    options.ClientSecret = "Zjk5MDE5YmEtMjc3MC00ZjdhLThmY2QtNThjNWNlYTA5YjJh";
    options.ResponseType = "code"; 
    options.ClaimActions.MapJsonKey("role", "role", "role");
    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
    options.TokenValidationParameters.NameClaimType = "name"; 
    options.TokenValidationParameters.RoleClaimType = "role"; 
    options.Scope.Add("geek_shopping");
    options.SaveTokens = true; 
});

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
