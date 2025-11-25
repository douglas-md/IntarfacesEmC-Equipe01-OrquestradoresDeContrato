using System.Collections.Generic;

namespace RepositoryEventosCsv.Repositorio
{
    public interface IRepository<T, TId>
    {
        void Add(T entity);
        T GetById(TId id);
        List<T> ListAll();
        void Update(T entity);
        void Remove(TId id);
    }
}