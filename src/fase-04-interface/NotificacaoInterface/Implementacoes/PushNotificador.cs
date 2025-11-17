using NotificacaoInterface.Interfaces;

namespace NotificacaoInterface.Implementacoes
{
    /// <summary>
    /// Implementação concreta: notificação push.
    /// Ideal para alertas rápidos quando o aluno está online.
    /// </summary>
    public sealed class PushNotificador : INotificador
    {
        public string Notificar(string destinatario, string mensagem)
        {
            // Simula envio de push notification
            var resultado = $"[PUSH] Notificação para {destinatario}: {mensagem}";
            System.Console.WriteLine(resultado);
            return resultado;
        }
    }
}
