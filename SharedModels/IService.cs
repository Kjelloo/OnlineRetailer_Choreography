using System.Collections.Generic;

namespace SharedModels
{
    public interface IService<T>
    {
        T Add(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        T Edit(T entity);
        T Remove(T entity);
    }
}