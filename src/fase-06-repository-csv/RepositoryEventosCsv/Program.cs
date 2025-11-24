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
            Console.WriteLine("=== FASE 6 - REPOSITORY CSV ===\n");
            Console.WriteLine("Sistema de Eventos Acadêmicos com persistência em arquivo CSV\n");

            // Define caminho do CSV (pasta do projeto)
            var pastaAtual = AppContext.BaseDirectory;
            var caminhoCsv = Path.Combine(pastaAtual, "eventos_academicos.csv");

            // Composição: cria repository concreto de CSV
            IRepository<EventoAcademico, int> repo =
                new CsvEventoRepository(caminhoCsv);

            // CLIENTE fala apenas com o SERVIÇO
            Console.WriteLine("--- Registrando eventos acadêmicos ---\n");

            EventoService.Registrar(repo, new EventoAcademico(
                Id: 1,
                Tipo: "AlteracaoSala",
                Descricao: "Sala mudou de 201 para 305, prédio central",
                DataHora: DateTime.Now.AddHours(1),
                DestinatarioEmail: "aluno1@exemplo.com",
                JaNotificado: false
            ));

            EventoService.Registrar(repo, new EventoAcademico(
                Id: 2,
                Tipo: "EnvioNota",
                Descricao: "Nota de Matemática disponível: 9,75",
                DataHora: DateTime.Now,
                DestinatarioEmail: "aluno2@exemplo.com",
                JaNotificado: false
            ));

            EventoService.Registrar(repo, new EventoAcademico(
                Id: 3,
                Tipo: "Lembrete",
                Descricao: "Prova de POO amanhã às 14h, levar documento.",
                DataHora: DateTime.Now.AddDays(1),
                DestinatarioEmail: "aluno3@exemplo.com",
                JaNotificado: false
            ));

            Console.WriteLine("\n--- Listando todos os eventos (a partir do CSV) ---\n");
            foreach (var ev in EventoService.ListarTodos(repo))
            {
                Console.WriteLine($"#{ev.Id} [{ev.Tipo}] {ev.Descricao} → {ev.DestinatarioEmail} (Notificado: {ev.JaNotificado})");
            }

            Console.WriteLine("\n--- Marcando evento #1 como notificado ---\n");
            EventoService.MarcarComoNotificado(repo, 1);

            var evento1 = repo.GetById(1);
            Console.WriteLine($"Evento #1 - JaNotificado: {evento1?.JaNotificado}");

            Console.WriteLine("\n--- Removendo evento #3 ---\n");
            EventoService.Remover(repo, 3);

            Console.WriteLine("\n--- Eventos restantes (recarregados do CSV) ---\n");
            foreach (var ev in EventoService.ListarTodos(repo))
            {
                Console.WriteLine($"#{ev.Id} [{ev.Tipo}] {ev.Descricao}");
            }

            Console.WriteLine($"\nArquivo CSV usado: {caminhoCsv}");
            Console.WriteLine("\n=== FIM DA EXECUÇÃO ===");
        }
    }
}
