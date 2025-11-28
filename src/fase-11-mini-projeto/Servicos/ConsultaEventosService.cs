using Dominio;
using Dominio.Contratos;

public sealed class ConsultaEventosService
{
    private readonly IReadRepository<EventoAcademico, int> _read;

    public ConsultaEventosService(IReadRepository<EventoAcademico, int> read) => _read = read;

    public IReadOnlyList<EventoAcademico> Todos() => _read.ListAll();
    public IReadOnlyList<EventoAcademico> Pendentes() => _read.ListAll().Where(e => !e.JaNotificado).ToList();
}
