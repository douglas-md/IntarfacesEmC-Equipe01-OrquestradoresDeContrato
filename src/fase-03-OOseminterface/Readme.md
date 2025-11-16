# Fase 3 — OO sem interface

## Objetivo

Refatorar a solução procedural da Fase 2 em uma arquitetura orientada a objetos, utilizando herança e polimorfismo para substituir decisões condicionais. Esta fase introduz uma hierarquia de classes com variações concretas e base comum, conforme requerido pelo enunciado.

## O que mudou da Fase 2

Na Fase 2, tínhamos uma única função com múltiplos `if`/`else` para decidir qual canal de notificação usar. Agora:

- **Criamos uma classe base abstrata** `NotificadorBase` que define o comportamento comum
- **Cada canal virou uma classe concreta** especializada (EmailNotificador, PushNotificador, SmsNotificador)
- **Utilizamos polimorfismo** para tratar diferentes tipos de notificadores de forma uniforme
- **Reduzimos (mas não eliminamos)** as decisões condicionais no serviço

## Estrutura de classes

| Classe | Tipo | Responsabilidade |
|--------|------|------------------|
| NotificadorBase | Abstrata | Define o contrato comum `Notificar()` |
| EmailNotificador | Concreta | Implementa notificação por e-mail |
| PushNotificador | Concreta | Implementa notificação push |
| SmsNotificador | Concreta | Implementa notificação por SMS |

**Relacionamento:** Todas as classes concretas herdam de `NotificadorBase` e sobrescrevem o método `Notificar()` para implementar a lógica específica de cada canal.

## Código principal

### Classe base abstrata

public abstract class NotificadorBase
{
  public abstract void Notificar(string destinatario, string mensagem);
}

### Implementações concretas

Cada classe especializada implementa `Notificar()` de acordo com as características do seu canal:

- **EmailNotificador:** Simula envio de e-mail
- **PushNotificador:** Simula notificação push
- **SmsNotificador:** Simula envio de SMS

### Serviço orquestrador

O `NotificationService` ainda contém lógica de decisão (com `if`/`else`), mas agora instancia as classes concretas e usa polimorfismo para chamar `Notificar()`.

## Comparação: Antes e Agora

| Aspecto | Fase 2 (Procedural) | Fase 3 (OO sem interface) |
|---------|---------------------|---------------------------|
| **Coesão** | Baixa (tudo em uma função) | Alta (cada classe tem responsabilidade única) |
| **Extensibilidade** | Adicionar canal = editar função gigante | Adicionar canal = criar nova classe |
| **Organização** | Código misturado | Separação clara por responsabilidade |
| **Manutenibilidade** | Difícil (muitos if aninhados) | Mais fácil (alterações localizadas) |
| **Testabilidade** | Baixa (testar tudo junto) | Média (testar cada classe separada) |

### Exemplo prático

**Antes (Procedural):**

if (modo == "urgente") {
  EnviarPushNotification(...);
  EnviarSMS(...);
} 
else if (modo == "detalhado") {
  EnviarEmail(...);
}

**Agora (OO sem interface):**

if (modo == "urgente") {
  notificador = new PushNotificador();
  notificador.Notificar(...);
}

**Ganho:** Modularidade através de classes especializadas, mas ainda há dependência de tipos concretos.

## O que ainda fica rígido

Apesar das melhorias, alguns problemas permanecem:

1. **Cliente conhece classes concretas**
      - O `NotificationService` ainda faz `new EmailNotificador()`, `new PushNotificador()`
   - Troca de implementação exige modificar o código do serviço (viola Open/Closed Principle)

3. **Decisões centralizadas no serviço**
   - O `if`/`else` para escolher qual classe instanciar permanece
   - A política de seleção está embutida no orquestrador

4. **Acoplamento a tipos concretos**
   - Não há abstração/contrato entre cliente e implementações
   - Dificulta teste com dublês (mocks/stubs)
   - Impede injeção de dependência

5. **Extensão exige modificação**
   - Adicionar WhatsApp ou Telegram ainda exige mexer no `NotificationService`

### Exemplo da rigidez

// Cliente precisa conhecer e instanciar concretos

if (modo == "urgente") {
  notificador = new PushNotificador(); // Acoplamento!
}


