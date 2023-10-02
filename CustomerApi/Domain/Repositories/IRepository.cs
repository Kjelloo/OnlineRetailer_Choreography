namespace CustomerApi.Domain.Repositories;

public interface IRepository<T>
{
    T Add(T entity);
    T Get(int Id);
    IEnumerable<T> GetAll();
    void Update(T entity);
    void Delete(T entity);
}