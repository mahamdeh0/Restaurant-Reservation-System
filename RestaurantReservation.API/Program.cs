using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Interfaces;
using RestaurantReservation.Db.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using RestaurantReservation.API.Services;
using FluentValidation.AspNetCore;
using System.Reflection;
using Microsoft.OpenApi.Models;
using RestaurantReservation.API.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RestaurantReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantReservationDb")));


builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddFluentValidation(config =>
    {
        config.ImplicitlyValidateChildProperties = true;
        config.ImplicitlyValidateRootCollectionElements = true;
        config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<IEmployeeRepository, EmployeeRepository>()
    .AddScoped<IRestaurantRepository, RestaurantRepository>()
    .AddScoped<IMenuItemRepository, MenuItemRepository>()
    .AddScoped<IOrderItemRepository, OrderItemRepository>()
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddScoped<IReservationRepository, ReservationRepository>()
    .AddScoped<ITableRepository, TableRepository>()
    .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero


    };

});

builder.Services.AddSwaggerGen(setupAction =>
{

    setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});


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
