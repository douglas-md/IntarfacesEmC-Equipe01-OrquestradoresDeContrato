public interface IWriteRepository<in T, in TId>
{
    T Add(T entity);
    bool Update(T entity);
    bool Remove(TId id);
}
