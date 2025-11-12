Fase 2 — Procedural mínimo

Implementação de modos de notificação para eventos acadêmicos, abrangendo alterações de sala de aula e envio de notas em um fluxo procedural, 
com quatro modos ("urgente", "detalhado", "resumido", "padrão") e cinco cenários de teste/fronteira.

Função de Notificação com Modos

public class NotificacaoService
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
            EnviarEmail(email, $"[DETALHADO] {mensagem}\nInformações completas sobre o evento.");
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

Cinco cenários de teste/fronteira descritos

1. Modo: "urgente"
   - Notificação de alteração de sala em menos de 30 minutos para aluno offline
   - Esperado: push notification + SMS enviados

2. Modo: "detalhado"
   - Envio de nota com todas as informações completas
   - Esperado: e-mail detalhado enviado

3. Modo: "resumido"
   - Mensagem muito longa de nota
   - Esperado: push notification apenas com primeiros 30 caracteres

4. Modo: "padrão" (default)
   - Alteração de sala, aluno offline
   - Esperado: SMS enviado

5. Modo: inválido ou vazio
   - Envio de nota sem especificação do modo
   - Esperado: e-mail (comportamento padrão)

Por que essa abordagem não escala?

- Cada novo modo/canal exige nova ramificação e edições na função, tornando o código trabalhoso de manter.
- Modificações na política de seleção, de formatos ou combinação de canais requerem alteração direta e duplicação das lógicas.
- O crescimento da solução dificulta o teste, o reaproveitamento e a expansão para comportamentos mais avançados (exemplo: integração de novos canais, regras dinâmicas, customização por usuário).
- É difícil garantir consistência, pois lógica de regras e formatos pode se espalhar por vários pontos do sistema.
