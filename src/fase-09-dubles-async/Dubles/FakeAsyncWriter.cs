public sealed class FakeAsyncWriter : IAsyncWriter<EventoAcademico>
{
    private int _falhasRestantes = 0;
    public List<EventoAcademico> Escritos { get; } = new();

    public void ConfigurarFalhas(int quantas) => _falhasRestantes = quantas;

    public Task WriteAsync(EventoAcademico item, CancellationToken ct = default)
    {
        if (_falhasRestantes > 0)
        {
            _falhasRestantes--;
            throw new InvalidOperationException("Falha temporária simulada");
        }

        Escritos.Add(item);
        return Task.CompletedTask;
    }
}
