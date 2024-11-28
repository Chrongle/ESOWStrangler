namespace Catalog.Core.Interfaces;
public interface IGetCatalogService<T> where T : IAggregateRoot
{
    Task<T> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}
