using GeekShopping.Email.MessageConsumer;
using GeekShopping.Email.Model.Context;
using GeekShopping.Email.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        Title = "GeekShopping.Email",
        Version = "v1", 
        Description = "Microsserviço email", 
        Contact = new OpenApiContact
        {
            Name = "Matheus Siqueira", 
            Email = "matheussiqueira.work@gmail.com", 
            Url = new Uri("https://github.com/matheuz-siqueira")
        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token", 
        Name = "Authorization", 
        In = ParameterLocation.Header, 
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer" 
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
         new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer", 
                In = ParameterLocation.Header
            }, 
            new List<string>() 
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


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer" ,options => 
    {
       options.Authority = "http://localhost:7036";
       options.RequireHttpsMetadata = false; 
       options.TokenValidationParameters = new TokenValidationParameters
       {
            ValidateAudience = false
       }; 
    });

builder.Services.AddAuthorization(options => 
{ 
    options.AddPolicy("ApiScope", policy => 
    {
        policy.RequireAuthenticatedUser(); 
        policy.RequireClaim("scope", "geek_shopping");
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
    )
);

var build = new DbContextOptionsBuilder<AppDbContext>(); 
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

build.UseMySql(connection, 
    ServerVersion.AutoDetect(connection));
builder.Services.AddSingleton(new EmailRepository(build.Options));

builder.Services.AddScoped<IEmailRepository, EmailRepository>();

builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
