### Fase 07 — Repository JSON (Diagnóstico ISP)

O objetivo desta fase era demonstrar a alternância de implementações de persistência (de CSV ou In-Memory para JSON) sem alterar o código cliente. O objetivo foi atingido, pois o `EventoService` depende apenas da interface `IRepository<T, TId>`.

[cite\_start]No entanto, a interface **`IRepository<T, TId>`** atual apresenta um "cheiro de design" (ISP Violation), que deve ser endereçado nas fases subsequentes (Fase 7: ISP na prática, de acordo com a estrutura sugerida no PDF [cite: 1]).

| Característica | Detalhe da `IRepository<T, TId>` |
| :--- | :--- |
| **Tipo de Interface** | Interface Gorda ("Fat Interface") ou Onipotente. |
| **Métodos Agregados** | Leitura: `GetById`, `ListAll` |
| | Escrita/Mutação: `Add`, `Update`, `Remove` |

### 1\. Diagnóstico: Violação do ISP

O Princípio de Segregação de Interfaces (ISP) afirma que nenhum cliente deve ser forçado a depender de métodos que não usa.

A interface **`IRepository<T, TId>`** atual agrupa todas as operações de persistência (leitura e escrita).

  * **Problema:** Se criarmos um novo cliente, como um `ServicoDeRelatorio` que só precisa **ler** a lista de eventos, esse cliente será forçado a depender também dos métodos de **escrita** (`Add`, `Update`, `Remove`), que são irrelevantes para sua função.
  * **Consequência:** Isso aumenta o acoplamento do sistema e torna a interface mais frágil a mudanças. Se a assinatura de um método de escrita mudar, o `ServicoDeRelatorio` pode ser obrigado a recompilar, mesmo não utilizando aquele método.

### 2\. Antídoto Proposto (Refatoração para ISP)

Para aderir ao ISP, a interface única deve ser segregada em contratos coesos por capacidade:

| Interface Segregada | Métodos | Cliente Típico |
| :--- | :--- | :--- |
| **`IReadRepository<T, TId>`** | `GetById(TId id)`, `ListAll()` | Serviços de Relatório, Serviços de Consulta |
| **`IWriteRepository<T>`** | `Add(T entity)`, `Update(T entity)`, `Remove(TId id)` | Serviços de Negócio (ex: `EventoService` para registro) |

#### Implementação de Transição

A interface original `IRepository<T, TId>` (ou uma nova como `IEventoRepository`) pode ser mantida como uma **interface de composição**, herdando ambas as interfaces segregadas, garantindo compatibilidade retroativa (se o cliente precisar de tudo):

```csharp
// Contrato de composição
public interface IEventoRepository : IReadRepository<EventoAcademico, int>, IWriteRepository<EventoAcademico> { }
```

### 3\. Conclusão

Embora a implementação JSON tenha sido concluída com sucesso, a estrutura do contrato de interface será o foco da próxima fase de refatoração para aplicar o **ISP** e melhorar a **coesão** e a **testabilidade** do sistema.