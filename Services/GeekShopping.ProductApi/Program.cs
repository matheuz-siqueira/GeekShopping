using GeekShopping.ProductApi.Model.Context;
using GeekShopping.ProductApi.Repositories;
using GeekShopping.ProductApi.Repositories.contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GeekShopping.ProductApi",
        Version = "v1", 
        Description = "Microsserviço produtos", 
        Contact = new OpenApiContact
        {
            Name = "Matheus Siqueira", 
            Email = "matheussiqueira.work@gmail.com", 
            Url = new Uri("https://github.com/matheuz-siqueira")
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
    )
);
    
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
