using Dominio;
using System.Runtime.CompilerServices;

public sealed class FakeAsyncReader : IAsyncReader<EventoAcademico>
{
    private readonly IEnumerable<EventoAcademico> _itens;
    private int _leitoCount = 0;
    public bool DeveFalharNoSegundo { get; set; }

    public FakeAsyncReader(IEnumerable<EventoAcademico> itens) => _itens = itens;

    public async IAsyncEnumerable<EventoAcademico> ReadAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        foreach (var item in _itens)
        {
            ct.ThrowIfCancellationRequested();
            
            if (DeveFalharNoSegundo && _leitoCount == 1)
            {
                throw new InvalidOperationException("Simulated failure on second read");
            }
            
            _leitoCount++;
            await Task.Delay(10, ct); // simula latência
            yield return item;
        }
    }
}
