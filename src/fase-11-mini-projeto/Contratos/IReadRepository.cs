public interface IReadRepository<out T, in TId>
{
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
}
