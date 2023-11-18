using UserApi.Core.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserApi.Domain.Helpers;
using UserApi.Domain.Repositories;
using UserApi.Domain.Service;
using UserApi.Infrastructure.EfCore;
using UserApi.Infrastructure.EfCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

var secret = Convert.FromBase64String("jp9VneOn8T0807HfupYKEyj7VgiNaa2K+BAo/m/8jVmwdXW+uKpXBQ==");

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

// Add services to the container.
builder.Services.AddDbContext<UserApiContext>(opt => opt.UseInMemoryDatabase("UserDB"));
builder.Services.AddSingleton<IAuthService>(new AuthService(secret));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddTransient<IDbInitializer, DbInitializer>();

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
    var dbContext = services.GetService<UserApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();