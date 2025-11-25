using Fase8Isp.Contratos;
using Fase8Isp.Dominio;

namespace Fase8Isp.Dubles
{
    // Implementa SÓ IReadRepository (Dublê minimalista)
    public sealed class FakeReadRepository : IReadRepository<EventoAcademico, int>
    {
        // Dados de teste pré-carregados
        private readonly List<EventoAcademico> _data = new()
        {
            new(1, "AlteracaoSala", "Sala mudou para 305", DateTime.Now.AddDays(-1), "a@facul.com", false),
            new(2, "EnvioNota", "Nota de Matemática disponível: 9,75", DateTime.Now, "b@facul.com", true), // NOTIFICADO
            new(3, "RevisaoTrabalho", "Prazo final de revisão estendido", DateTime.Now.AddDays(1), "c@facul.com", false),
        };

        public EventoAcademico? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
        public IReadOnlyList<EventoAcademico> ListAll() => _data.AsReadOnly();
        
        // Não há métodos Add, Update, Remove. Teste mais rápido e focado.
    }
}