using Fase8Isp.Contratos;
using Fase8Isp.Dominio;

namespace Fase8Isp.Servicos
{
    // Cliente de escrita: depende apenas de IWriteRepository
    public sealed class RegistroEventosService
    {
        private readonly IWriteRepository<EventoAcademico, int> _write;

        public RegistroEventosService(IWriteRepository<EventoAcademico, int> write) => _write = write;

        public EventoAcademico RegistrarNovo(EventoAcademico evento) => _write.Add(evento);

        public bool Atualizar(EventoAcademico evento) => _write.Update(evento);
        
        public bool Remover(int id) => _write.Remove(id);
    }
}