using Microsoft.EntityFrameworkCore;
using UserApi.Core.Models;

namespace UserApi.Infrastructure.EfCore;

public class UserApiContext : DbContext
{
    public UserApiContext(DbContextOptions<UserApiContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
}