using System.Text.Json;
using Dominio;
using Dominio.Contratos;

namespace Repositorios;

public sealed class JsonEventoRepository : 
    IReadRepository<EventoAcademico, int>,
    IWriteRepository<EventoAcademico, int>
{
    private readonly string _filePath;
    private List<EventoAcademico> _cache = new();
    private int _nextId = 1;
    private readonly JsonSerializerOptions _jsonOptions = new() 
    { 
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public JsonEventoRepository(string filePath = "eventos-academicos.json")
    {
        _filePath = filePath;
        Carregar();
    }

    private void Carregar()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                var json = File.ReadAllText(_filePath);
                var eventos = JsonSerializer.Deserialize<List<EventoAcademico>>(json, _jsonOptions);
                _cache = eventos ?? new List<EventoAcademico>();
                _nextId = _cache.Count > 0 ? _cache.Max(x => x.Id) + 1 : 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar arquivo: {ex.Message}");
                _cache = new List<EventoAcademico>();
            }
        }
    }

    private void Salvar()
    {
        try
        {
            var json = JsonSerializer.Serialize(_cache, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar arquivo: {ex.Message}");
            throw;
        }
    }

    public EventoAcademico Add(EventoAcademico evento)
    {
        var eventoComId = evento with { Id = _nextId++ };
        _cache.Add(eventoComId);
        Salvar();
        return eventoComId;
    }

    public EventoAcademico? GetById(int id) => _cache.Find(x => x.Id == id);

    public IReadOnlyList<EventoAcademico> ListAll() => _cache.AsReadOnly();

    public IReadOnlyList<EventoAcademico> Find(Func<EventoAcademico, bool> predicate) 
        => _cache.Where(predicate).ToList().AsReadOnly();

    public bool Update(EventoAcademico evento)
    {
        var index = _cache.FindIndex(x => x.Id == evento.Id);
        if (index == -1)
            return false;

        _cache[index] = evento;
        Salvar();
        return true;
    }

    public bool Remove(int id)
    {
        var removido = _cache.RemoveAll(x => x.Id == id) > 0;
        if (removido)
            Salvar();
        return removido;
    }

    public int Count => _cache.Count;
}

// Versão para testes de integração
public sealed class JsonEventoRepositoryCustom : JsonEventoRepository
{
    public JsonEventoRepositoryCustom(string customPath) : base(customPath) { }
}