using Fase8Isp.Contratos;
using Fase8Isp.Dominio;

namespace Fase8Isp.Servicos
{
    // Cliente de leitura: depende apenas de IReadRepository
    public sealed class ConsultaEventosService
    {
        private readonly IReadRepository<EventoAcademico, int> _read;

        public ConsultaEventosService(IReadRepository<EventoAcademico, int> read) => _read = read;

        public IReadOnlyList<EventoAcademico> ListarPendentes() =>
            _read.ListAll().Where(e => !e.JaNotificado).ToList().AsReadOnly();
        
        public EventoAcademico? BuscarPorId(int id) => _read.GetById(id);
    }
}