using System.Collections.Generic;
using SharedModels.Order.Dtos;

namespace SharedModels
{
    public interface IConverter<T, U>
    {
        T Convert(U model);
        U Convert(T model);
    }
}