using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSingleton<IConverter<OrderLine, OrderLineDto>, OrderLineConverter>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

// Register MessagePublisher (a messaging gateway) for dependency injection
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
