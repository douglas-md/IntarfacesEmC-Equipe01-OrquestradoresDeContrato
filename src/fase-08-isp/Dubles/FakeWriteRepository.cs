using Fase8Isp.Contratos;
using Fase8Isp.Dominio;

namespace Fase8Isp.Dubles
{
    // Implementa SÓ IWriteRepository (Dublê minimalista)
    public sealed class FakeWriteRepository : IWriteRepository<EventoAcademico, int>
    {
        private readonly List<EventoAcademico> _data = new();
        private int _nextId = 100; // IDs altos para evitar conflito com FakeRead

        // Propriedade para validação nos testes
        public IReadOnlyList<EventoAcademico> Data => _data.AsReadOnly();

        public EventoAcademico Add(EventoAcademico entity)
        {
            var comId = entity with { Id = _nextId++ };
            _data.Add(comId);
            return comId;
        }

        public bool Update(EventoAcademico entity)
        {
            var idx = _data.FindIndex(e => e.Id == entity.Id);
            if (idx == -1) return false;
            _data[idx] = entity;
            return true;
        }

        public bool Remove(int id) => _data.RemoveAll(e => e.Id == id) > 0;
        
        // Não há métodos GetById, ListAll. Prova do ISP.
    }
}
