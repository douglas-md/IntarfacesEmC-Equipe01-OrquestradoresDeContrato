using NotificacaoInterface.Interfaces;

namespace NotificacaoInterface.Testes
{
    /// <summary>
    /// Dublê (Fake) para testes unitários.
    /// Implementa INotificador sem I/O real.
    /// Permite testar o cliente de forma rápida e determinística.
    /// </summary>
    public sealed class FakeNotificador : INotificador
    {
        public string UltimoDestinatario { get; private set; } = string.Empty;
        public string UltimaMensagem { get; private set; } = string.Empty;
        public int QuantidadeChamadas { get; private set; } = 0;

        public string Notificar(string destinatario, string mensagem)
        {
            UltimoDestinatario = destinatario;
            UltimaMensagem = mensagem;
            QuantidadeChamadas++;

            // Retorna algo previsível para teste
            return $"[FAKE] Notificado: {destinatario}";
        }
    }
}
