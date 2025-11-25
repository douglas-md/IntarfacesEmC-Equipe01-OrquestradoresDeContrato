namespace Fase8Isp.Dominio
{
    // Usando 'record' para imutabilidade e facilidade de manipulação
    public record EventoAcademico(
        int Id,
        string Tipo,
        string Descricao,
        DateTime DataHora,
        string DestinatarioEmail,
        bool JaNotificado
    );
}