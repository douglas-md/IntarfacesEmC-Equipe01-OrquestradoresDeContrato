using System.Collections.Generic;
using RepositoryEventosCsv.Dominio;
using RepositoryEventosCsv.Repositorio;

namespace RepositoryEventosCsv.Servicos
{
    public static class EventoService
    {
        public static void Registrar(IRepository<EventoAcademico, int> repo, EventoAcademico evento)
        {
            repo.Add(evento);
        }

        public static List<EventoAcademico> ListarTodos(IRepository<EventoAcademico, int> repo)
        {
            return repo.ListAll();
        }

        public static void MarcarComoNotificado(IRepository<EventoAcademico, int> repo, int id)
        {
            var evento = repo.GetById(id);
            if (evento != null)
            {
                evento.JaNotificado = true;
                repo.Update(evento);
            }
        }

        public static void Remover(IRepository<EventoAcademico, int> repo, int id)
        {
            repo.Remove(id);
        }
    }
}
