namespace RepositoryEventosCsv.Dominio
{
    public class EventoAcademico
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public string DestinatarioEmail { get; set; } = string.Empty;
        public bool JaNotificado { get; set; }
    }
}