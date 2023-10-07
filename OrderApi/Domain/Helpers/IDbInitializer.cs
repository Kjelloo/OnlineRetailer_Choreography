using OrderApi.Infrastructure.EfCore;

namespace OrderApi.Domain.Helpers;

public interface IDbInitializer
{
    void Initialize(OrderApiContext context);
}