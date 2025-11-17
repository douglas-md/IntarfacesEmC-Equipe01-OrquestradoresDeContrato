# Fase 4 — Interface plugável e testável

## Objetivo

Evoluir a solução da Fase 3 introduzindo um **contrato explícito** (interface), **ponto único de composição** e **cliente desacoplado** das implementações concretas. Demonstrar testabilidade através de dublês sem I/O real.

## Conquistas desta fase

Esta fase resolve **todas as rigidezes** identificadas na Fase 3:

1. ✅ **Cliente não conhece mais classes concretas** - Depende apenas de `INotificador`
2. ✅ **Política centralizada** - `NotificadorCatalog` é o ponto único de composição
3. ✅ **Inversão de Dependência** - Aplicação do DIP (Dependency Inversion Principle)
4. ✅ **Testabilidade plena** - Uso de `FakeNotificador` para testes rápidos e determinísticos
5. ✅ **Open/Closed Principle** - Adicionar novos canais não modifica código existente

## Estrutura de arquivos

NotificacaoInterface/
├── Interfaces/
│ └── INotificador.cs # Contrato abstrato
├── Implementacoes/
│ ├── EmailNotificador.cs # Implementação concreta
│ ├── PushNotificador.cs # Implementação concreta
│ └── SmsNotificador.cs # Implementação concreta
├── Servicos/
│ └── NotificationService.cs # Cliente (só conhece interface)
├── Fabricas/
│ └── NotificadorCatalog.cs # Ponto único de composição
├── Testes/
│ ├── FakeNotificador.cs # Dublê para testes
│ └── NotificationServiceTests.cs # Testes unitários
└── Program.cs # Demonstração de uso


## Contrato (Interface)

### INotificador.cs

public interface INotificador
{
string Notificar(string destinatario, string mensagem);
}


**Características:**
- Define **"o que"** fazer (notificar), não **"como"** fazer
- Abstração estável que não muda quando adicionamos novos canais
- Base para polimorfismo e injeção de dependência

## Implementações concretas

| Classe | Responsabilidade | Quando usar |
|--------|------------------|-------------|
| EmailNotificador | Notificação por e-mail | Comunicações detalhadas, não urgentes |
| PushNotificador | Notificação push no app | Alertas rápidos para usuários online |
| SmsNotificador | Notificação por SMS | Urgências quando usuário está offline |

Todas implementam `INotificador`, garantindo **substituibilidade** (Liskov Substitution Principle).

## Cliente desacoplado

### NotificationService.cs

public sealed class NotificationService
{
private readonly INotificador _notificador;

public NotificationService(INotificador notificador)
{
    _notificador = notificador;
}

public string ProcessarNotificacao(string tipoEvento, string destinatario, string detalhes)
{
    var mensagem = $"[{tipoEvento.ToUpper()}] {detalhes}";
    return _notificador.Notificar(destinatario, mensagem);
}


**Princípios aplicados:**
- **Injeção de Dependência:** Recebe `INotificador` via construtor
- **Inversão de Dependência (DIP):** Depende de abstração, não de concretos
- **Responsabilidade única (SRP):** Apenas orquestra, não decide qual canal usar

## Ponto único de composição

### NotificadorCatalog.cs

Centraliza a **política de seleção** de implementação:

public static INotificador Resolver(string modo)
{
return modo?.ToLowerInvariant() switch
{
"urgente" => new PushNotificador(),
"detalhado" => new EmailNotificador(),
"resumido" => new PushNotificador(),
"padrao-offline" => new SmsNotificador(),
"padrao-online" => new EmailNotificador(),
_ => new EmailNotificador()
};
}


**Vantagens:**
- Cliente não precisa conhecer as classes concretas
- Trocar política de seleção = mexer apenas aqui
- Adicionar novo canal = criar classe + adicionar case no switch

## Testabilidade com dublês

### FakeNotificador.cs

Dublê (Fake/Stub) que implementa `INotificador` sem I/O real:

public sealed class FakeNotificador : INotificador
{
public string UltimoDestinatario { get; private set; }
public string UltimaMensagem { get; private set; }
public int QuantidadeChamadas { get; private set; }

public string Notificar(string destinatario, string mensagem)
{
    UltimoDestinatario = destinatario;
    UltimaMensagem = mensagem;
    QuantidadeChamadas++;
    return $"[FAKE] Notificado: {destinatario}";
}

### NotificationServiceTests.cs

Testes unitários **rápidos e determinísticos**:

private static void Teste1_DeveNotificarUsandoNotificadorInjetado()
{
// Arrange
var fake = new FakeNotificador();
var service = new NotificationService(fake);

// Act
service.ProcessarNotificacao("Alteração de Sala", "aluno@exemplo.com", "Sala 305");

// Assert
if (fake.QuantidadeChamadas != 1)
    throw new Exception("FALHA: Deveria ter sido chamado 1 vez");

Console.WriteLine("✓ Teste passou");


**Benefícios:**
- Sem dependência de rede, banco de dados ou serviços externos
- Execução em **milissegundos**
- Previsível e repetível
- Facilita TDD (Test-Driven Development)

## Comparação: Fase 3 vs Fase 4

| Aspecto | Fase 3 (OO sem interface) | Fase 4 (Com interface) |
|---------|---------------------------|------------------------|
| **Cliente conhece concretos?** | ✗ Sim (`new EmailNotificador()`) | ✅ Não (só `INotificador`) |
| **Ponto de composição** | ✗ Espalhado | ✅ Centralizado (`NotificadorCatalog`) |
| **Testabilidade** | ✗ Difícil (precisa I/O real) | ✅ Plena (dublês) |
| **Adicionar novo canal** | ✗ Modifica cliente | ✅ Só cria classe + adiciona no catálogo |
| **Princípios SOLID** | Parcial (SRP, parcialmente OCP) | ✅ Completo (SRP, OCP, LSP, DIP) |

### Exemplo prático

**Antes (Fase 3):**

// Cliente conhece e instancia concretos
if (modo == "urgente") {
notificador = new PushNotificador(); // Acoplamento!
}

**Agora (Fase 4):**
// Cliente só conhece interface
INotificador notificador = NotificadorCatalog.Resolver(modo);
var service = new NotificationService(notificador);


**Ganho:** Cliente **totalmente desacoplado** das implementações.

## Princípios SOLID aplicados

### 1. Single Responsibility Principle (SRP)
- `EmailNotificador`: Só cuida de e-mail
- `NotificationService`: Só orquestra
- `NotificadorCatalog`: Só resolve composição

### 2. Open/Closed Principle (OCP)
- Adicionar `TelegramNotificador`:
  1. Criar classe implementando `INotificador`
  2. Adicionar case no `NotificadorCatalog`
  3. **Código existente não muda**

### 3. Liskov Substitution Principle (LSP)
- Qualquer `INotificador` pode substituir outro
- Cliente funciona com qualquer implementação

### 4. Interface Segregation Principle (ISP)
- Interface mínima: apenas `Notificar()`
- Implementações não são forçadas a implementar métodos desnecessários

### 5. Dependency Inversion Principle (DIP)
- `NotificationService` depende de `INotificador` (abstração)
- Não depende de `EmailNotificador`, `PushNotificador`, etc. (concretos)