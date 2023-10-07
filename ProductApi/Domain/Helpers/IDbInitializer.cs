using ProductApi.Infrastructure.EfCore;

namespace ProductApi.Domain.Helpers
{
    public interface IDbInitializer
    {
        void Initialize(ProductApiContext context);
    }
}
