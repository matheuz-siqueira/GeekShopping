using System.Security.Claims;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer.Contracts;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Initializer;

public class DbInitializer : IDbInitializer
{
    private readonly AppDbContext _context; 
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(AppDbContext context, UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _context = context; 
        _userManager = userManager; 
        _roleManager = roleManager;
    }
    public void Initialize()
    {
        if(_roleManager.FindByNameAsync(IdentityConfiguration.Admin).Result is not null)
            return; 

        _roleManager
            .CreateAsync(new IdentityRole(IdentityConfiguration.Admin))
                .GetAwaiter().GetResult();

        _roleManager
            .CreateAsync(new IdentityRole(IdentityConfiguration.Client))
                .GetAwaiter().GetResult();

        ApplicationUser admin = new()
        {
            UserName = "user-admin", 
            Email = "admin@mail.com", 
            EmailConfirmed = true, 
            PhoneNumber = "+55 34 1234-9876",
            FistName = "User",
            LastName = "Admin"  
        };

        _userManager.CreateAsync(admin, "P4ssword@.").GetAwaiter().GetResult(); 
        _userManager.AddToRoleAsync(
                admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

        var adminClaims = _userManager.AddClaimsAsync(admin, new Claim[]
        {
            new (JwtClaimTypes.Name, $"{admin.FistName} {admin.LastName}"),
            new (JwtClaimTypes.GivenName, admin.FistName),
            new (JwtClaimTypes.FamilyName, admin.LastName),
            new (JwtClaimTypes.Role, IdentityConfiguration.Admin)
        }).Result;

        ApplicationUser client = new()
        {
            UserName = "user-client", 
            Email = "client@mail.com", 
            EmailConfirmed = true, 
            PhoneNumber = "+55 34 1234-9876",
            FistName = "User",
            LastName = "Client"  
        };

        _userManager.CreateAsync(client, "P4ssword@.").GetAwaiter().GetResult(); 
        _userManager.AddToRoleAsync(
                client, IdentityConfiguration.Client).GetAwaiter().GetResult();
        var clientClaims = _userManager.AddClaimsAsync(client, new Claim[]
        {
            new (JwtClaimTypes.Name, $"{client.FistName} {client.LastName}"),
            new (JwtClaimTypes.GivenName, client.FistName),
            new (JwtClaimTypes.FamilyName, client.LastName),
            new (JwtClaimTypes.Role, IdentityConfiguration.Client)
        }).Result;
    }
}
