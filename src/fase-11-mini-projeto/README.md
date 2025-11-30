# Fase 11 — Mini-projeto de Consolidação

## 🎯 Tecnologias Consolidadas

- **ISP (Interface Segregation Principle)** → Contratos segregados (`IReadRepository` / `IWriteRepository`)
- **Repository Pattern** com duas implementações: `InMemory` (testes) + `JSON` (persistência real)
- **Composição Centralizada** no `Program.cs` (ponto único de configuração)
- **Testabilidade** com dublês (Moq) + testes de integração com arquivos temporários
- **Imutabilidade** com `record` e `with-expressions`
- **CLI Funcional** com casos de uso reais

## 🏗️ Arquitetura

Camadas:

Domínio: EventoAcademico (modelo imutável)

Contratos: Interfaces ISP (leitura/escrita separadas)

Repositórios: Implementações concretas (InMemory, JSON)

Serviços: Lógica de aplicação (Consulta, Gestão)

Apresentação: CLI demo (Program.cs)

text

## 🚀 Como Executar

```bash
# Navegar para a pasta do projeto
cd src/fase-11-mini-projeto

# Restaurar dependências
dotnet restore

# Executar demonstração completa
dotnet run

# Executar todos os testes
dotnet test

# Executar apenas testes unitários
dotnet test --filter Category=Unit

# Executar apenas testes de integração
dotnet test --filter Category=Integration


📋 Funcionalidades Implementadas
Serviços de Domínio
GestaoEventosService: Registrar, marcar como notificado, cancelar eventos

ConsultaEventosService: Consultar pendentes, por tipo, futuros, estatísticas

Repositórios
InMemoryEventoRepository: Para testes e scenarios temporários

JsonEventoRepository: Persistência real em JSON com recovery automático

Casos de Uso da Demo
✅ Registrar eventos acadêmicos de diferentes tipos

✅ Consultar eventos pendentes de notificação

✅ Estatísticas por tipo e status

✅ Persistência automática em JSON

✅ Dados sobrevivem entre execuções

🧪 Estratégia de Testes
Testes Unitários (12+)
Serviços com dublês de repositório

Zero I/O → execução em < 100ms

Cobertura dos caminhos felizes e exceções

Testes de Integração (2+)
Persistência JSON com arquivos temporários

Recuperação de dados entre instâncias

Cleanup automático de recursos

📊 Métricas de Qualidade
✅ ISP Aplicado: Clientes de leitura não dependem de escrita

✅ Baixo Acoplamento: Serviços dependem apenas de interfaces

✅ Testabilidade: 100% dos serviços testáveis com dublês

✅ Imutabilidade: Zero side effects com records

✅ Persistência: Dados sobrevivem com JSON

✅ CLI Funcional: Demo completa com casos reais

🔄 Como Estender
Adicionar Novo Tipo de Repositório
csharp
public class SqlEventoRepository : IReadRepository<EventoAcademico, int>, 
                                  IWriteRepository<EventoAcademico, int>
{
    // Implementar contratos sem mudar clientes existentes
}
Adicionar Novo Serviço
csharp
public class NotificacaoService
{
    public NotificacaoService(IReadRepository<EventoAcademico, int> readRepo)
    {
        // Usar apenas leitura → ISP em ação!
    }
}

📝 Checklist de Qualidade Aplicado
Contratos coesos e segregados (ISP)

Alternância InMemory↔JSON sem mudar cliente

Testes unitários sem I/O (dublês)

Zero downcasts / switch desnecessários

Imutabilidade com records

Composição centralizada

Documentação arquitetural

Código auto-documentado

