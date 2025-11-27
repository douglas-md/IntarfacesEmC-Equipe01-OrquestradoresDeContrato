
public interface IAsyncReader<out T>
{
    IAsyncEnumerable<T> ReadAsync(CancellationToken ct = default);
}
