using System;
using System.IO;
using RepositoryEventosCsv.Dominio;
using RepositoryEventosCsv.Repositorio;
using RepositoryEventosCsv.Servicos;

namespace RepositoryEventosCsv
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 7 - REPOSITORY JSON ===\n");
            Console.WriteLine("Sistema de Eventos Acadêmicos com persistência em JSON\n");

            var pastaAtual = AppContext.BaseDirectory;
            var caminhoJson = Path.Combine(pastaAtual, "eventos_academicos.json");
            IRepository<EventoAcademico, int> repo = new JsonEventoRepository(caminhoJson);

            Console.WriteLine("--- Registrando eventos acadêmicos ---\n");
            EventoService.Registrar(repo, new EventoAcademico
            {
                Id = 1,
                Tipo = "AlteracaoSala",
                Descricao = "Sala mudou de 201 para 305, prédio central",
                DataHora = DateTime.Now.AddHours(1),
                DestinatarioEmail = "aluno1@exemplo.com",
                JaNotificado = false
            });

            EventoService.Registrar(repo, new EventoAcademico
            {
                Id = 2,
                Tipo = "EnvioNota",
                Descricao = "Nota de Matemática disponível: 9,75",
                DataHora = DateTime.Now,
                DestinatarioEmail = "aluno2@exemplo.com",
                JaNotificado = false
            });

            Console.WriteLine("\n--- Listando todos os eventos (a partir do JSON) ---\n");
            foreach (var ev in EventoService.ListarTodos(repo))
            {
                Console.WriteLine($"#{ev.Id} [{ev.Tipo}] {ev.Descricao} → {ev.DestinatarioEmail} (Notificado: {ev.JaNotificado})");
            }

            Console.WriteLine("\n--- Marcando evento #1 como notificado ---\n");
            EventoService.MarcarComoNotificado(repo, 1);

            var evento1 = repo.GetById(1);
            Console.WriteLine($"Evento #1 - JaNotificado: {evento1?.JaNotificado}");

            Console.WriteLine("\n--- Removendo evento #2 ---\n");
            EventoService.Remover(repo, 2);

            Console.WriteLine("\n--- Eventos restantes (recarregados do JSON) ---\n");
            foreach (var ev in EventoService.ListarTodos(repo))
            {
                Console.WriteLine($"#{ev.Id} [{ev.Tipo}] {ev.Descricao}");
            }

            Console.WriteLine($"\nArquivo JSON usado: {caminhoJson}");
            Console.WriteLine("\n=== FIM DA EXECUÇÃO ===");
        }
    }
}
