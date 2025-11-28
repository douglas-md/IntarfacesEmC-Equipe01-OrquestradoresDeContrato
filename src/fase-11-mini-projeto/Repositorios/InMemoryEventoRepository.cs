public sealed class InMemoryEventoRepository : 
    IReadRepository<EventoAcademico, int>,
    IWriteRepository<EventoAcademico, int>
{
    private readonly Dictionary<int, EventoAcademico> _db = new();
    private int _nextId = 1;

    public EventoAcademico Add(EventoAcademico evento)
    {
        var eventoComId = evento with { Id = _nextId++ };
        _db[eventoComId.Id] = eventoComId;
        return eventoComId;
    }

    public EventoAcademico? GetById(int id) => _db.GetValueOrDefault(id);

    public IReadOnlyList<EventoAcademico> ListAll() => _db.Values.ToList().AsReadOnly();

    public IReadOnlyList<EventoAcademico> Find(Func<EventoAcademico, bool> predicate) 
        => _db.Values.Where(predicate).ToList().AsReadOnly();

    public bool Update(EventoAcademico evento)
    {
        if (!_db.ContainsKey(evento.Id))
            return false;
            
        _db[evento.Id] = evento;
        return true;
    }

    public bool Remove(int id) => _db.Remove(id);

    public int Count => _db.Count;
}