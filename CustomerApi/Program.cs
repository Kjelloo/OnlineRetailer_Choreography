using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using CustomerApi.Domain.Converters;
using CustomerApi.Domain.Helpers;
using CustomerApi.Domain.Services;
using CustomerApi.Infrastructure.EfCore;
using CustomerApi.Infrastructure.EfCore.Repositories;
using CustomerApi.Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;
using SharedModels;
using SharedModels.Customer;
using SharedModels.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CustomerApiContext>(opt => opt.UseInMemoryDatabase("CustomerDB"));

builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddSingleton<IConverter<Customer, CustomerDto>, CustomerConverter>();

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<CustomerApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

Task.Factory.StartNew(() =>
    new MessageListener(app.Services, MessageConnectionHelper.ConnectionString).Start());

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();