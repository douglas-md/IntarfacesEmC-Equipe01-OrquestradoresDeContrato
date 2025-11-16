using System;

// Classe base abstrata - define o contrato comum
public abstract class NotificadorBase
{
    public abstract void Notificar(string destinatario, string mensagem);
}

// Implementação concreta - Email
public class EmailNotificador : NotificadorBase
{
    public override void Notificar(string destinatario, string mensagem)
    {
        Console.WriteLine($"E-mail enviado para {destinatario}: {mensagem}");
    }
}

// Implementação concreta - Push Notification
public class PushNotificador : NotificadorBase
{
    public override void Notificar(string destinatario, string mensagem)
    {
        Console.WriteLine($"Push notification para {destinatario}: {mensagem}");
    }
}

// Implementação concreta - SMS
public class SmsNotificador : NotificadorBase
{
    public override void Notificar(string destinatario, string mensagem)
    {
        Console.WriteLine($"SMS para {destinatario}: {mensagem}");
    }
}

// Serviço que ainda decide qual classe usar (rigidez)
public class NotificationService
{
    public void ProcessarNotificacao(string tipoEvento, string modo, bool usuarioOffline, 
                                      string destinatario, string mensagem)
    {
        NotificadorBase notificador;

        // Decisão de qual notificador usar (ainda há if/else, mas reduzido)
        if (modo == "urgente")
        {
            notificador = new PushNotificador();
            notificador.Notificar(destinatario, $"URGENTE: {mensagem}");
            
            // No modo urgente, envia também SMS
            var smsNotificador = new SmsNotificador();
            smsNotificador.Notificar(destinatario, $"URGENTE: {mensagem}");
        }
        else if (modo == "detalhado")
        {
            notificador = new EmailNotificador();
            notificador.Notificar(destinatario, $"[DETALHADO] {mensagem}\nInformações completas sobre o evento.");
        }
        else if (modo == "resumido")
        {
            notificador = new PushNotificador();
            string mensagemCurta = mensagem.Length > 30 ? mensagem.Substring(0, 30) : mensagem;
            notificador.Notificar(destinatario, mensagemCurta);
        }
        else // modo padrão
        {
            if (tipoEvento == "alteracaoSala" && usuarioOffline)
            {
                notificador = new SmsNotificador();
            }
            else
            {
                notificador = new EmailNotificador();
            }
            notificador.Notificar(destinatario, mensagem);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var service = new NotificationService();

        Console.WriteLine("=== FASE 3 - OO SEM INTERFACE ===\n");
        Console.WriteLine("Sistema de Notificação de Eventos Acadêmicos\n");

        // Cenário 1: Urgente
        Console.WriteLine("--- Cenário 1: Modo Urgente ---");
        service.ProcessarNotificacao("alteracaoSala", "urgente", true, 
                                      "aluno@exemplo.com", "Troca de sala para 203");
        Console.WriteLine();

        // Cenário 2: Detalhado
        Console.WriteLine("--- Cenário 2: Modo Detalhado ---");
        service.ProcessarNotificacao("envioNota", "detalhado", false, 
                                      "aluno@exemplo.com", "Sua nota está disponível");
        Console.WriteLine();

        // Cenário 3: Resumido
        Console.WriteLine("--- Cenário 3: Modo Resumido ---");
        service.ProcessarNotificacao("envioNota", "resumido", false, 
                                      "aluno@exemplo.com", "Nota final: excelente desempenho acadêmico");
        Console.WriteLine();

        // Cenário 4: Padrão (offline)
        Console.WriteLine("--- Cenário 4: Modo Padrão (offline) ---");
        service.ProcessarNotificacao("alteracaoSala", "padrao", true, 
                                      "+5511999999999", "Troca de sala para 101");
        Console.WriteLine();

        // Cenário 5: Padrão (online)
        Console.WriteLine("--- Cenário 5: Modo Padrão (online) ---");
        service.ProcessarNotificacao("envioNota", "padrao", false, 
                                      "aluno@exemplo.com", "Nota A em Matemática");
        Console.WriteLine();

        Console.WriteLine("=== FIM DA EXECUÇÃO ===");
    }
}
