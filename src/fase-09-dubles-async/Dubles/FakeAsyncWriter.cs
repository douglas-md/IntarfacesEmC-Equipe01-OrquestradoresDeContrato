using Dominio;

public sealed class FakeAsyncWriter : IAsyncWriter<EventoAcademico>
{
    private int _falhasRestantes = 0;
    public List<EventoAcademico> Escritos { get; } = new();
    public int FalhasAcumuladas { get; private set; }

    public FakeAsyncWriter() { }

    public FakeAsyncWriter(int falhasIniciais)
    {
        _falhasRestantes = falhasIniciais;
    }

    public void ConfigurarFalhas(int quantas) => _falhasRestantes = quantas;

    public Task WriteAsync(EventoAcademico item, CancellationToken ct = default)
    {
        if (_falhasRestantes > 0)
        {
            _falhasRestantes--;
            FalhasAcumuladas++;
            throw new InvalidOperationException("Falha temporária simulada");
        }

        Escritos.Add(item);
        return Task.CompletedTask;
    }
}
