using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryEventosCsv.Dominio;
using RepositoryEventosCsv.Repositorio;

namespace RepositoryEventosCsv.Servicos
{
    /// <summary>
    /// Serviço de domínio que gerencia eventos acadêmicos.
    /// O CLIENTE (Program, API, etc.) fala com ESTE serviço.
    /// Este serviço fala com o Repository (indiretamente, via interface).
    /// </summary>
    public static class EventoService
    {
        public static EventoAcademico Registrar(
            IRepository<EventoAcademico, int> repo,
            EventoAcademico evento)
        {
            if (string.IsNullOrWhiteSpace(evento.Tipo))
                throw new ArgumentException("Tipo do evento é obrigatório");

            if (string.IsNullOrWhiteSpace(evento.Descricao))
                throw new ArgumentException("Descrição é obrigatória");

            if (string.IsNullOrWhiteSpace(evento.DestinatarioEmail))
                throw new ArgumentException("E-mail do destinatário é obrigatório");

            return repo.Add(evento);
        }

        public static IReadOnlyList<EventoAcademico> ListarTodos(
            IRepository<EventoAcademico, int> repo)
        {
            return repo.ListAll();
        }

        public static IReadOnlyList<EventoAcademico> ListarPorTipo(
            IRepository<EventoAcademico, int> repo, string tipo)
        {
            return repo.ListAll()
                       .Where(e => e.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                       .ToList();
        }

        public static bool MarcarComoNotificado(
            IRepository<EventoAcademico, int> repo, int id)
        {
            var evento = repo.GetById(id);
            if (evento == null)
                return false;

            var atualizado = evento with { JaNotificado = true };
            return repo.Update(atualizado);
        }

        public static bool Remover(
            IRepository<EventoAcademico, int> repo, int id)
        {
            return repo.Remove(id);
        }
    }
}
