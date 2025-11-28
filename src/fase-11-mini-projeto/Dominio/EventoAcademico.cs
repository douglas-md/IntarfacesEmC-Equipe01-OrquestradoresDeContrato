namespace Fase11MiniProjeto.Dominio;

public sealed record EventoAcademico(
    int Id,
    string Tipo,
    string Descricao,
    DateTime DataHora,
    string DestinatarioEmail,
    bool JaNotificado = false
)
{
    public bool EstaPendente => !JaNotificado;
    public bool Ocorreu => DataHora <= DateTime.Now;
}