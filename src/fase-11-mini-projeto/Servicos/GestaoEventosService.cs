using Dominio;
using Dominio.Contratos;

public sealed class GestaoEventosService
{
    private readonly IWriteRepository<EventoAcademico, int> _write;

    public GestaoEventosService(IWriteRepository<EventoAcademico, int> write) => _write = write;

    public EventoAcademico Registrar(EventoAcademico e) => _write.Add(e);
    public bool MarcarComoNotificado(int id)
    {
        var evento = new InMemoryEventoRepository().GetById(id); // só exemplo
        if (evento == null) return false;
        return _write.Update(evento with { JaNotificado = true });
    }
}
