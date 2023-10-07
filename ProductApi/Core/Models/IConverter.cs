namespace ProductApi.Core.Models
{
    public interface IConverter<T,U>
    {
        T Convert(U model);
        U Convert(T model);
    }
}
