namespace Fase8Isp.Contratos
{
    public interface IWriteRepository<T, TId> where T : notnull
    {
        T Add(T entity);
        bool Update(T entity);
        bool Remove(TId id);
    }
}