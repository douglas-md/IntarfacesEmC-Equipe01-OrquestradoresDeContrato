namespace Dominio
{
    public record EventoAcademico(int Id, string Tipo, string Descricao, DateTime DataHora, string DestinatarioEmail, bool JaNotificado);
}
