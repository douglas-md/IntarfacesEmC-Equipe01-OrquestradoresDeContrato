using NotificacaoInterface.Interfaces;

namespace NotificacaoInterface.Implementacoes
{
    /// <summary>
    /// Implementação concreta: notificação por SMS.
    /// Ideal para urgências quando o aluno está offline.
    /// </summary>
    public sealed class SmsNotificador : INotificador
    {
        public string Notificar(string destinatario, string mensagem)
        {
            // Simula envio de SMS
            var resultado = $"[SMS] Enviado para {destinatario}: {mensagem}";
            System.Console.WriteLine(resultado);
            return resultado;
        }
    }
}
