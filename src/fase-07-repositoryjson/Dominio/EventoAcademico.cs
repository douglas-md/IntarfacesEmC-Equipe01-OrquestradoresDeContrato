namespace RepositoryEventosCsv.Dominio
{
    public class EventoAcademico
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public string DestinatarioEmail { get; set; }
        public bool JaNotificado { get; set; }
    }
}
