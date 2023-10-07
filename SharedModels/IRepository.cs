using System.Collections.Generic;

namespace SharedModels
{
    public interface IRepository<T>
    {
        T Add(T entity);
        IEnumerable<T> GetAll();
        T Get(int id);
        T Edit(T entity);
        T Remove(T entity);
    }
}