public interface IAsyncWriter<in T>
{
    Task WriteAsync(T item, CancellationToken ct = default);
}

