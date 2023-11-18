using UserApi.Infrastructure.EfCore;

namespace UserApi.Domain.Helpers;

public class DbInitializer : IDbInitializer
{
    public void Initialize(UserApiContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}