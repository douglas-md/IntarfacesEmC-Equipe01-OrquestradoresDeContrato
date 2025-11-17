namespace NotificacaoInterface.Interfaces
{
    /// <summary>
    /// Contrato para o passo variável: notificação de eventos acadêmicos.
    /// Define "o que" fazer (notificar), não "como" fazer.
    /// </summary>
    public interface INotificador
    {
        /// <summary>
        /// Notifica um destinatário sobre um evento acadêmico.
        /// </summary>
        /// <param name="destinatario">E-mail, telefone ou identificador do aluno</param>
        /// <param name="mensagem">Conteúdo da notificação</param>
        /// <returns>Confirmação do envio (simulado)</returns>
        string Notificar(string destinatario, string mensagem);
    }
}
