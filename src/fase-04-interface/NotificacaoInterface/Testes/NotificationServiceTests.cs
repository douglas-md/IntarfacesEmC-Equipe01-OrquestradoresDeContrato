using System;
using NotificacaoInterface.Servicos;
using NotificacaoInterface.Testes;

namespace NotificacaoInterface.Testes
{
    /// <summary>
    /// Testes unitários para NotificationService usando dublê.
    /// Sem I/O real, rápido e determinístico.
    /// </summary>
    public static class NotificationServiceTests
    {
        public static void ExecutarTodos()
        {
            Console.WriteLine("=== EXECUTANDO TESTES UNITÁRIOS ===\n");

            Teste1_DeveNotificarUsandoNotificadorInjetado();
            Teste2_DeveLancarExcecaoQuandoNotificadorNulo();
            Teste3_DeveFormatarMensagemCorretamente();

            Console.WriteLine("\n=== TODOS OS TESTES PASSARAM ===\n");
        }

        private static void Teste1_DeveNotificarUsandoNotificadorInjetado()
        {
            // Arrange
            var fake = new FakeNotificador();
            var service = new NotificationService(fake);

            // Act
            var resultado = service.ProcessarNotificacao(
                "Alteração de Sala", 
                "aluno@exemplo.com", 
                "Sala mudou de 201 para 305"
            );

            // Assert
            if (fake.UltimoDestinatario != "aluno@exemplo.com")
                throw new Exception("FALHA: Destinatário incorreto");

            if (!fake.UltimaMensagem.Contains("ALTERAÇÃO DE SALA"))
                throw new Exception("FALHA: Mensagem não formatada");

            if (fake.QuantidadeChamadas != 1)
                throw new Exception("FALHA: Deveria ter sido chamado 1 vez");

            Console.WriteLine("✓ Teste 1 passou: Notificador injetado usado corretamente");
        }

        private static void Teste2_DeveLancarExcecaoQuandoNotificadorNulo()
        {
            // Arrange & Act & Assert
            try
            {
                var service = new NotificationService(null!);
                throw new Exception("FALHA: Deveria lançar ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("✓ Teste 2 passou: Exceção lançada quando notificador é nulo");
            }
        }

        private static void Teste3_DeveFormatarMensagemCorretamente()
        {
            // Arrange
            var fake = new FakeNotificador();
            var service = new NotificationService(fake);

            // Act
            service.ProcessarNotificacao("Envio de Nota", "aluno@teste.com", "Nota: 9.5");

            // Assert
            if (!fake.UltimaMensagem.StartsWith("[ENVIO DE NOTA]"))
                throw new Exception("FALHA: Formato de mensagem incorreto");

            Console.WriteLine("✓ Teste 3 passou: Mensagem formatada corretamente");
        }
    }
}
