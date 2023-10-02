using CustomerApi.Infrastructure.EfCore;

namespace CustomerApi.Domain.Helpers;

public interface IDbInitializer
{
    void Initialize(CustomerApiContext context);
}