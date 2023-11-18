using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderApi.Core.Converters;
using OrderApi.Core.Models;
using OrderApi.Core.Services;
using OrderApi.Domain.Converters;
using OrderApi.Domain.Helpers;
using OrderApi.Domain.Repositories;
using OrderApi.Domain.Services;
using OrderApi.Infrastructure.EfCore;
using OrderApi.Infrastructure.EfCore.Repositories;
using OrderApi.Infrastructure.Messages;
using SharedModels;
using SharedModels.Helpers;
using SharedModels.Order.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add db context and in-memory database for testing
builder.Services.AddDbContext<OrderApiContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register services for dependency injection
builder.Services.AddScoped<IOrderService, OrderService>();

// Register converters for dependency injection
builder.Services.AddSingleton<IConverter<Order, OrderDto>, OrderConverter>();
builder.Services.AddSingleton<IConverter<OrderStatus, OrderStatusDto>, OrderStatusConverter>();
builder.Services.AddSingleton<IOrderLineConverter, OrderLineConverter>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

// Register MessagePublisher (a messaging gateway) for dependency injection
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
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

// Initialize the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<OrderApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

// Create a message listener in a separate thread.

Task.Factory.StartNew(() =>
    new MessageListener(app.Services, MessageConnectionHelper.ConnectionString).Start());

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();