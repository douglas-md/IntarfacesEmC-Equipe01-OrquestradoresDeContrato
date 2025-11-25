using Fase8Isp.Contratos;
using Fase8Isp.Dominio;

namespace Fase8Isp.Implementacoes
{
    // A implementação atende a AMBOS os contratos segregados
    public sealed class InMemoryEventoRepository :
        IReadRepository<EventoAcademico, int>,
        IWriteRepository<EventoAcademico, int>
    {
        private readonly Dictionary<int, EventoAcademico> _db = new();
        private int _nextId = 1;

        // IWriteRepository
        public EventoAcademico Add(EventoAcademico entity)
        {
            var comId = entity with { Id = _nextId++ }; 
            _db[comId.Id] = comId;
            return comId;
        }
        public bool Update(EventoAcademico entity)
        {
            if (!_db.ContainsKey(entity.Id)) return false;
            _db[entity.Id] = entity;
            return true;
        }
        public bool Remove(int id) => _db.Remove(id);
        
        // IReadRepository
        public EventoAcademico? GetById(int id) => _db.GetValueOrDefault(id);
        public IReadOnlyList<EventoAcademico> ListAll() => _db.Values.ToList().AsReadOnly();
    }
}