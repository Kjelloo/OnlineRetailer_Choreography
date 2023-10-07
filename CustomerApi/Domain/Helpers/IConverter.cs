namespace CustomerApi.Domain.Helpers;

public interface IConverter<T, U>
{
    T Convert(U model);
    U Convert(T model);
}