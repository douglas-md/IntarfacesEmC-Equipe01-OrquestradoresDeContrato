public sealed class FakeAsyncReader : IAsyncReader<EventoAcademico>
{
    private readonly IEnumerable<EventoAcademico> _itens;

    public FakeAsyncReader(IEnumerable<EventoAcademico> itens) => _itens = itens;

    public async IAsyncEnumerable<EventoAcademico> ReadAsync(CancellationToken ct = default)
    {
        foreach (var item in _itens)
        {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(10, ct); // simula latência
            yield return item;
        }
    }
}
