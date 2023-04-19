namespace Ecommerce.Common.Core;

public interface IRepository<T> where T : EntityRoot
{
    Task<T> GetById(Guid id);
    Task<PagedList<T>> FindAll(PaginationParameter paginationParameter);
    Task<T> Create(T entity);
    Task Update(T entity);
    Task Delete(Guid id);
}