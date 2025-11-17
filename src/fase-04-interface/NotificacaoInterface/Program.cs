using System;
using NotificacaoInterface.Interfaces;
using NotificacaoInterface.Servicos;
using NotificacaoInterface.Fabricas;
using NotificacaoInterface.Testes;

namespace NotificacaoInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 4 - INTERFACE PLUGÁVEL E TESTÁVEL ===\n");
            Console.WriteLine("Sistema de Notificação de Eventos Acadêmicos\n");

            // Primeiro, executar os testes
            NotificationServiceTests.ExecutarTodos();

            // Agora, demonstrar cenários de uso
            Console.WriteLine("=== DEMONSTRAÇÃO DE CENÁRIOS ===\n");

            DemonstrarCenario("urgente", "Alteração de Sala", "+5511999999999", 
                             "Sala mudou de 201 para 305 em 10 minutos");

            DemonstrarCenario("detalhado", "Envio de Nota", "aluno@exemplo.com", 
                             "Sua nota foi publicada: 9.5. Consulte o portal para detalhes.");

            DemonstrarCenario("resumido", "Lembrete", "aluno@exemplo.com", 
                             "Prova amanhã às 14h");

            DemonstrarCenario("padrao-offline", "Alteração de Sala", "+5511888888888", 
                             "Troca de sala para 101");

            DemonstrarCenario("padrao-online", "Comunicado", "aluno@exemplo.com", 
                             "Reunião de coordenação às 16h");

            Console.WriteLine("\n=== FIM DA EXECUÇÃO ===");
        }

        private static void DemonstrarCenario(string modo, string tipoEvento, 
                                             string destinatario, string detalhes)
        {
            Console.WriteLine($"--- Cenário: Modo '{modo}' ---");

            // Composição em ponto único (Catálogo resolve qual implementação usar)
            INotificador notificador = NotificadorCatalog.Resolver(modo);

            // Cliente depende apenas da interface
            var service = new NotificationService(notificador);

            // Execução
            service.ProcessarNotificacao(tipoEvento, destinatario, detalhes);

            Console.WriteLine();
        }
    }
}
