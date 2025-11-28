public sealed class ConsultaEventosService
{
    private readonly IReadRepository<EventoAcademico, int> _readRepo;

    public ConsultaEventosService(IReadRepository<EventoAcademico, int> readRepo)
    {
        _readRepo = readRepo;
    }

    public IReadOnlyList<EventoAcademico> ObterTodos() => _readRepo.ListAll();

    public IReadOnlyList<EventoAcademico> ObterPendentes() 
        => _readRepo.Find(e => e.EstaPendente);

    public IReadOnlyList<EventoAcademico> ObterPorTipo(string tipo) 
        => _readRepo.Find(e => e.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<EventoAcademico> ObterFuturos()
        => _readRepo.Find(e => e.DataHora > DateTime.Now);

    public EventoAcademico? ObterPorId(int id) => _readRepo.GetById(id);

    public int ContarPendentes() => ObterPendentes().Count;
}