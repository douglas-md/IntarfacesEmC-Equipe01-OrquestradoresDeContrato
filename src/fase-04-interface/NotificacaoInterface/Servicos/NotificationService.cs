using System;
using NotificacaoInterface.Interfaces;

namespace NotificacaoInterface.Servicos
{
    /// <summary>
    /// Cliente que depende APENAS da interface INotificador.
    /// Não conhece as implementações concretas.
    /// Princípio da Inversão de Dependência (DIP) aplicado.
    /// </summary>
    public sealed class NotificationService
    {
        private readonly INotificador _notificador;

        /// <summary>
        /// Injeção de dependência via construtor.
        /// </summary>
        public NotificationService(INotificador notificador)
        {
            _notificador = notificador ?? throw new ArgumentNullException(nameof(notificador));
        }

        /// <summary>
        /// Processa notificação de evento acadêmico.
        /// </summary>
        public string ProcessarNotificacao(string tipoEvento, string destinatario, string detalhes)
        {
            var mensagem = $"[{tipoEvento.ToUpper()}] {detalhes}";
            return _notificador.Notificar(destinatario, mensagem);
        }
    }
}
