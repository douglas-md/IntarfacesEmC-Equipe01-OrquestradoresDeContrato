using System.Text.Json;
using System.Text.Json.Serialization;
using RepositoryEventosCsv.Dominio;

namespace RepositoryEventosCsv.Repositorio
{
    public class JsonEventoRepository : IRepository<EventoAcademico, int>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonEventoRepository(string filePath)
        {
            _filePath = filePath;
            _options = new JsonSerializerOptions
            {
                // Serialização em camelCase, ignorando campos nulos, formato indentado.
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public void Add(EventoAcademico evento)
        {
            var eventos = ListAll();
            // Implementação simples: o ID é passado pelo cliente, mas idealmente seria gerado aqui.
            eventos.Add(evento);
            SaveAll(eventos);
        }

        public EventoAcademico GetById(int id)
        {
            var evento = ListAll().FirstOrDefault(e => e.Id == id);
            if (evento == null)
                throw new KeyNotFoundException($"EventoAcademico with Id {id} not found.");
            return evento;
        }

        public List<EventoAcademico> ListAll()
        {
            if (!File.Exists(_filePath) || new FileInfo(_filePath).Length == 0)
                return new List<EventoAcademico>();

            var json = File.ReadAllText(_filePath);
            // Deserializa a lista de EventoAcademico do JSON
            return JsonSerializer.Deserialize<List<EventoAcademico>>(json, _options) ?? new List<EventoAcademico>();
        }

        public void Update(EventoAcademico evento)
        {
            var eventos = ListAll();
            var idx = eventos.FindIndex(e => e.Id == evento.Id);
            if (idx == -1) return;
            eventos[idx] = evento;
            SaveAll(eventos);
        }

        public void Remove(int id)
        {
            // Filtra e mantém todos os eventos exceto o que possui o ID fornecido
            var eventos = ListAll().Where(e => e.Id != id).ToList();
            SaveAll(eventos);
        }

        private void SaveAll(List<EventoAcademico> eventos)
        {
            // Serializa e reescreve todo o arquivo
            var json = JsonSerializer.Serialize(eventos, _options);
            File.WriteAllText(_filePath, json);
        }
    }
}