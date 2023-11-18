using UserApi.Infrastructure.EfCore;

namespace UserApi.Domain.Helpers;

public interface IDbInitializer
{
    void Initialize(UserApiContext context);
}