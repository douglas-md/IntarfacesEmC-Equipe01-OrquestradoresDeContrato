using System;

class NotificacaoService
{
    public void NotificarUsuario(string tipoEvento, string mensagem, string modo, bool usuarioOffline, string email, string telefone)
    {
        if (modo == "urgente")
        {
            EnviarPushNotification(email, $"URGENTE: {mensagem}");
            EnviarSMS(telefone, $"URGENTE: {mensagem}");
        }
        else if (modo == "detalhado")
        {
            EnviarEmail(email, $"[DETALHADO] {mensagem}\nInformações completas sobre o evento.");
        }
        else if (modo == "resumido")
        {
            EnviarPushNotification(email, mensagem.Substring(0, Math.Min(30, mensagem.Length)));
        }
        else // modo padrão
        {
            if (tipoEvento == "alteracaoSala" && usuarioOffline)
                EnviarSMS(telefone, mensagem);
            else
                EnviarEmail(email, mensagem);
        }
    }

    private void EnviarPushNotification(string email, string mensagem)
    {
        Console.WriteLine($"Push notification para {email}: {mensagem}");
    }

    private void EnviarEmail(string email, string mensagem)
    {
        Console.WriteLine($"E-mail enviado para {email}: {mensagem}");
    }

    private void EnviarSMS(string telefone, string mensagem)
    {
        Console.WriteLine($"SMS para {telefone}: {mensagem}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var notificacaoService = new NotificacaoService();
        // Cenário 1: Urgente
        notificacaoService.NotificarUsuario("alteracaoSala", "Troca de sala para 203", "urgente", true, "aluno@ex.com", "+5511999999999");
        // Cenário 2: Detalhado
        notificacaoService.NotificarUsuario("envioNota", "Sua nota está disponível.", "detalhado", false, "aluno@ex.com", "+5511999999999");
        // Cenário 3: Resumido
        notificacaoService.NotificarUsuario("envioNota", "Nota final: excelente desempenho acadêmico.", "resumido", false, "aluno@ex.com", "+5511999999999");
        // Cenário 4: Padrão
        notificacaoService.NotificarUsuario("alteracaoSala", "Troca de sala para 101.", "padrao", true, "aluno@ex.com", "+5511999999999");
        // Cenário 5: Modo inválido
        notificacaoService.NotificarUsuario("envioNota", "Nota A em Matemática.", "", false, "aluno@ex.com", "+5511999999999");
    }
}
