Fase 1 — Heurística antes do código

Sistema de Notificação de Eventos Acadêmicos: evolução para tratar tanto alterações de sala de aula quanto envio de notas, dentro da mesma arquitetura de notificação.

| Abordagem              | Descrição                                                                                                       |
|------------------------|-----------------------------------------------------------------------------------------------------------------|
| Procedural (if/switch) | No modelo procedural, toda a lógica fica agrupada em uma função, utilizando `if`/`else` para decidir o canal (push, email, SMS). 
                         | Cada novo tipo exige alterar essa função, e cada canal novo aumenta ramificações nas decisões.

void NotificarEvento(string tipo, DateTime hora, bool usuarioOffline) {
    if (tipo == "alteracaoSala" && (hora - DateTime.Now).TotalMinutes < 30) {
        EnviarPushNotification();
    } else if (tipo == "alteracaoSala") {
        EnviarEmail();
    } else if (tipo == "envioNota" && usuarioOffline) {
        EnviarSMS();
    } else if (tipo == "envioNota") {
        EnviarEmail();
    }
}

- Toda regra (novo tipo/canal) exige edição. - Testar cada cenário é difícil. |

| OO sem interface | Agora, a lógica de cada canal é dividida em classes separadas; ainda assim, 
                   | o serviço de notificação precisa saber (e instanciar) cada classe concreta e fazer a escolha via `if`.

class NotificationService {
    public void NotificarEvento(Evento evento) {
        if (evento.Tipo == "alteracaoSala" && evento.MinutosAteEvento < 30) {
            var push = new PushNotificationSender();
            push.Enviar(evento);
        } else if (evento.Tipo == "alteracaoSala") {
            var email = new EmailSender();
            email.Enviar(evento);
        } else if (evento.Tipo == "envioNota" && evento.UsuarioOffline) {
            var sms = new SmsSender();
            sms.Enviar(evento);
        } else if (evento.Tipo == "envioNota") {
            var email = new EmailSender();
            email.Enviar(evento);
        }
    }
}

- A decisão de canal e o conhecimento dos concretos ainda estão centralizados. - Mudança de política exige mexer nesse serviço.

| Com interface | Introduz-se um contrato (interface), que abstrai o conceito de notificador. 
                | O serviço depende do contrato, e a seleção do canal é externalizada (por um selector/factory político, DI ou catálogo).

public interface INotificador {
    void Notificar(Evento evento);
}

class NotificationSelector {
    public INotificador SelecionarCanal(Evento evento) {
        if (evento.Tipo == "alteracaoSala" && evento.MinutosAteEvento < 30)
            return new PushNotificationSender();
        if (evento.Tipo == "envioNota" && evento.UsuarioOffline)
            return new SmsSender();
        return new EmailSender();
    }
}

class NotificationService {
    private readonly INotificador _notificador;
    public NotificationService(INotificador notificador) {
        _notificador = notificador;
    }
    public void NotificarEvento(Evento evento) {
        _notificador.Notificar(evento);
    }
}

- Serviço não conhece detalhes concretos. - Política pode mudar ou expandir facilmente sem afetar o cliente.

Três sinais de alerta previstos

1. Acoplamento do cliente aos canais concretos: Se o serviço de notificação precisa conhecer (ou instanciar) todas as implementações, qualquer adição/mudança exige reescrever esse serviço.
2. Ramificações (if/switch) espalhadas: Se a lógica de seleção do canal fica duplicada em vários pontos do código, mudanças de política tornam-se caras e sujeitas a erro.
3. Dificuldade de testar cenários: Sem depender de interface, não é possível injetar mocks/dublês de canais facilmente, dificultando o teste isolado de comportamentos e erros.
