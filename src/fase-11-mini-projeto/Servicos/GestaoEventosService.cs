public sealed class GestaoEventosService
{
    private readonly IWriteRepository<EventoAcademico, int> _writeRepo;
    private readonly IReadRepository<EventoAcademico, int> _readRepo;

    public GestaoEventosService(
        IWriteRepository<EventoAcademico, int> writeRepo,
        IReadRepository<EventoAcademico, int> readRepo)
    {
        _writeRepo = writeRepo;
        _readRepo = readRepo;
    }

    public EventoAcademico RegistrarEvento(EventoAcademico evento)
    {
        if (string.IsNullOrWhiteSpace(evento.Tipo))
            throw new ArgumentException("Tipo do evento é obrigatório");
            
        return _writeRepo.Add(evento);
    }

    public bool MarcarComoNotificado(int id)
    {
        var evento = _readRepo.GetById(id);
        if (evento == null) return false;

        return _writeRepo.Update(evento with { JaNotificado = true });
    }

    public bool CancelarEvento(int id) => _writeRepo.Remove(id);
}