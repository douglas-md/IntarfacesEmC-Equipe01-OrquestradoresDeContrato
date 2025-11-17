using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryEventos.Dominio;
using RepositoryEventos.Repositorio;

namespace RepositoryEventos.Servicos
{
    /// <summary>
    /// Serviço de domínio que gerencia eventos acadêmicos.
    /// Depende APENAS do contrato IRepository, não da implementação concreta.
    /// Contém regras de negócio (validações, filtros).
    /// </summary>
    public static class EventoService
    {
        /// <summary>
        /// Registra um novo evento acadêmico com validações.
        /// </summary>
        public static EventoAcademico Registrar(IRepository<EventoAcademico, int> repo, EventoAcademico evento)
        {
            // Regra de negócio: validações
            if (string.IsNullOrWhiteSpace(evento.Tipo))
                throw new ArgumentException("Tipo do evento é obrigatório");

            if (string.IsNullOrWhiteSpace(evento.Descricao))
                throw new ArgumentException("Descrição do evento é obrigatória");

            if (string.IsNullOrWhiteSpace(evento.DestinatarioEmail))
                throw new ArgumentException("E-mail do destinatário é obrigatório");

            return repo.Add(evento);
        }

        /// <summary>
        /// Lista todos os eventos acadêmicos.
        /// </summary>
        public static IReadOnlyList<EventoAcademico> ListarTodos(IRepository<EventoAcademico, int> repo)
        {
            return repo.ListAll();
        }

        /// <summary>
        /// Lista eventos por tipo (regra de negócio - filtragem).
        /// </summary>
        public static IReadOnlyList<EventoAcademico> ListarPorTipo(
            IRepository<EventoAcademico, int> repo, string tipo)
        {
            return repo.ListAll()
                      .Where(e => e.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                      .ToList();
        }

        /// <summary>
        /// Marca evento como notificado (regra de negócio).
        /// </summary>
        public static bool MarcarComoNotificado(IRepository<EventoAcademico, int> repo, int id)
        {
            var evento = repo.GetById(id);
            if (evento == null)
                return false;

            // Record com with: cria nova instância com propriedade alterada
            var atualizado = evento with { JaNotificado = true };
            return repo.Update(atualizado);
        }

        /// <summary>
        /// Remove evento acadêmico.
        /// </summary>
        public static bool Remover(IRepository<EventoAcademico, int> repo, int id)
        {
            return repo.Remove(id);
        }
    }
}
