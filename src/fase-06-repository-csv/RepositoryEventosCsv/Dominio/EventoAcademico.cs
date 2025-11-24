namespace RepositoryEventosCsv.Dominio
{
    public sealed record EventoAcademico(
        int Id,
        string Tipo,              // "AlteracaoSala", "EnvioNota", "Lembrete"
        string Descricao,
        DateTime DataHora,
        string DestinatarioEmail,
        bool JaNotificado
    );
}
