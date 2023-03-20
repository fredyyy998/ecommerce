namespace Ecommerce.Common.Core;

public interface IRepository<T> where T : EntityRoot
{
    T GetById(Guid id);
    void Create(T entity);
    void Update(T entity);
    void Delete(Guid id);
}