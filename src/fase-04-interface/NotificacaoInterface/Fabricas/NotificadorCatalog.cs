using System;
using NotificacaoInterface.Interfaces;
using NotificacaoInterface.Implementacoes;

namespace NotificacaoInterface.Fabricas
{
    /// <summary>
    /// Catálogo/Factory: ponto único de composição.
    /// Converte "modo" (política) em implementação concreta.
    /// Centraliza as decisões de instanciação.
    /// </summary>
    public static class NotificadorCatalog
    {
        /// <summary>
        /// Resolve qual implementação usar baseado no modo.
        /// </summary>
        /// <param name="modo">urgente, detalhado, resumido, padrao-offline, padrao-online</param>
        /// <returns>Implementação concreta de INotificador</returns>
        public static INotificador Resolver(string modo)
        {
            return modo?.ToLowerInvariant() switch
            {
                "urgente" => new PushNotificador(),      // Push para urgências
                "detalhado" => new EmailNotificador(),   // E-mail para detalhes
                "resumido" => new PushNotificador(),     // Push para resumos
                "padrao-offline" => new SmsNotificador(), // SMS para offline
                "padrao-online" => new EmailNotificador(), // E-mail para online
                _ => new EmailNotificador() // Padrão seguro
            };
        }
    }
}
