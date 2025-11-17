using System;
using RepositoryEventos.Dominio;
using RepositoryEventos.Repositorio;
using RepositoryEventos.Servicos;

namespace RepositoryEventos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 5 - REPOSITORY INMEMORY ===\n");
            Console.WriteLine("Sistema de Eventos Acadêmicos\n");

            // Composição: criar repositório com política de ID
            IRepository<EventoAcademico, int> repo =
                new InMemoryRepository<EventoAcademico, int>(e => e.Id);

            // Registrar eventos acadêmicos
            Console.WriteLine("--- Registrando eventos acadêmicos ---\n");

            var evento1 = EventoService.Registrar(repo, new EventoAcademico(
                Id: 1,
                Tipo: "AlteracaoSala",
                Descricao: "Sala mudou de 201 para 305",
                DataHora: DateTime.Now.AddHours(1),
                DestinatarioEmail: "aluno1@exemplo.com",
                JaNotificado: false
            ));
            Console.WriteLine($"✓ Registrado: {evento1.Descricao}");

            var evento2 = EventoService.Registrar(repo, new EventoAcademico(
                Id: 2,
                Tipo: "EnvioNota",
                Descricao: "Nota de Matemática disponível",
                DataHora: DateTime.Now,
                DestinatarioEmail: "aluno2@exemplo.com",
                JaNotificado: false
            ));
            Console.WriteLine($"✓ Registrado: {evento2.Descricao}");

            var evento3 = EventoService.Registrar(repo, new EventoAcademico(
                Id: 3,
                Tipo: "Lembrete",
                Descricao: "Prova de POO amanhã às 14h",
                DataHora: DateTime.Now.AddDays(1),
                DestinatarioEmail: "aluno3@exemplo.com",
                JaNotificado: false
            ));
            Console.WriteLine($"✓ Registrado: {evento3.Descricao}");

            // Listar todos
            Console.WriteLine("\n--- Listando todos os eventos ---\n");
            var todos = EventoService.ListarTodos(repo);
            foreach (var ev in todos)
            {
                Console.WriteLine($"#{ev.Id} [{ev.Tipo}] {ev.Descricao} → {ev.DestinatarioEmail}");
            }

            // Filtrar por tipo
            Console.WriteLine("\n--- Eventos de Alteração de Sala ---\n");
            var alteracoes = EventoService.ListarPorTipo(repo, "AlteracaoSala");
            foreach (var ev in alteracoes)
            {
                Console.WriteLine($"#{ev.Id} {ev.Descricao}");
            }

            // Marcar como notificado
            Console.WriteLine("\n--- Marcando evento #1 como notificado ---\n");
            EventoService.MarcarComoNotificado(repo, 1);
            var eventoAtualizado = repo.GetById(1);
            Console.WriteLine($"Evento #1 - JaNotificado: {eventoAtualizado?.JaNotificado}");

            // Remover evento
            Console.WriteLine("\n--- Removendo evento #3 ---\n");
            bool removido = EventoService.Remover(repo, 3);
            Console.WriteLine($"Evento #3 removido: {removido}");

            Console.WriteLine($"\nTotal de eventos restantes: {repo.ListAll().Count}");

            Console.WriteLine("\n=== FIM DA EXECUÇÃO ===");
        }
    }
}
