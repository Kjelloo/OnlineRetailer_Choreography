using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using CustomerApi.Domain.Converters;
using CustomerApi.Domain.Helpers;
using CustomerApi.Domain.Services;
using CustomerApi.Infrastructure.EfCore;
using CustomerApi.Infrastructure.EfCore.Repositories;
using CustomerApi.Infrastructure.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestSharp.Authenticators;
using SharedModels;
using SharedModels.Customer;
using SharedModels.Helpers;

var builder = WebApplication.CreateBuilder(args);

var secret = Convert.FromBase64String("jp9VneOn8T0807HfupYKEyj7VgiNaa2K+BAo/m/8jVmwdXW+uKpXBQ==");

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

builder.Services.AddDbContext<CustomerApiContext>(opt => opt.UseInMemoryDatabase("CustomerDB"));

builder.Services.AddTransient<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddSingleton<IConverter<Customer, CustomerDto>, CustomerConverter>();

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