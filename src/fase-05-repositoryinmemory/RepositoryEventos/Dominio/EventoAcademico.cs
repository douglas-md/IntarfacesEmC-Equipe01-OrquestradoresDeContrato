namespace RepositoryEventos.Dominio
{
    /// <summary>
    /// Representa um evento acadêmico no sistema de notificações.
    /// Mantém a persistência temática das fases anteriores.
    /// </summary>
    public sealed record EventoAcademico(
        int Id,
        string Tipo,              // "AlteracaoSala", "EnvioNota", "Lembrete"
        string Descricao,
        DateTime DataHora,
        string DestinatarioEmail,
        bool JaNotificado
    );
}
