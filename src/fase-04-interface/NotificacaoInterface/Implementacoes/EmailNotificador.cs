using NotificacaoInterface.Interfaces;

namespace NotificacaoInterface.Implementacoes
{
    /// <summary>
    /// Implementação concreta: notificação por e-mail.
    /// Ideal para comunicações detalhadas e não urgentes.
    /// </summary>
    public sealed class EmailNotificador : INotificador
    {
        public string Notificar(string destinatario, string mensagem)
        {
            // Simula envio de e-mail
            var resultado = $"[E-MAIL] Enviado para {destinatario}: {mensagem}";
            System.Console.WriteLine(resultado);
            return resultado;
        }
    }
}
