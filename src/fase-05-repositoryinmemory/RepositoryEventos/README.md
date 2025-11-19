# README.md da Fase 5

**Arquivo:** `src/fase-05-repository-inmemory/README.md`


# Fase 5 — Repository InMemory

## Objetivo

Introduzir o padrão Repository como ponto único de acesso a dados para eventos acadêmicos, usando implementação InMemory baseada em coleção.

**Conceito-chave:** Cliente fala APENAS com o Repository, nunca com coleções diretamente.

---

## 1. Contrato (Interface)

### IRepository<T, TId>

```
public interface IRepository<T, TId>
{
    T Add(T entity);
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
    bool Update(T entity);
    bool Remove(TId id);
}
```

**Responsabilidades:**
- Define operações CRUD básicas
- Sem regras de negócio (apenas acesso)
- Retorna `IReadOnlyList` para proteger coleção interna

---

## 2. Implementação InMemory

### InMemoryRepository<T, TId>

```
public sealed class InMemoryRepository<T, TId> : IRepository<T, TId>
    where TId : notnull
{
    private readonly Dictionary<TId, T> _store = new();
    private readonly Func<T, TId> _getId;

    public InMemoryRepository(Func<T, TId> getId)
    {
        _getId = getId ?? throw new ArgumentNullException(nameof(getId));
    }

    public T Add(T entity)
    {
        var id = _getId(entity);
        _store[id] = entity;
        return entity;
    }

    public T? GetById(TId id) => 
        _store.TryGetValue(id, out var entity) ? entity : default;

    public IReadOnlyList<T> ListAll() => 
        _store.Values.ToList();

    public bool Update(T entity)
    {
        var id = _getId(entity);
        if (!_store.ContainsKey(id)) return false;
        _store[id] = entity;
        return true;
    }

    public bool Remove(TId id) => _store.Remove(id);
}
```

**Características:**
- Usa `Dictionary<TId, T>` como armazenamento
- Política de ID delegada via `Func<T, TId>` (injeção)
- Thread-unsafe (ok para InMemory simples)
- Sem I/O real (tudo volátil em memória)

---

## 3. Domínio

### EventoAcademico (Record)

```
public sealed record EventoAcademico(
    int Id,
    string Tipo,              // "AlteracaoSala", "EnvioNota", "Lembrete"
    string Descricao,
    DateTime DataHora,
    string DestinatarioEmail,
    bool JaNotificado
);
```

**Por que Record?**
- Imutabilidade por padrão
- Equality por valor (não por referência)
- Sintaxe `with` para cópias modificadas

---

## 4. Serviço (Cliente do Repository)

### EventoService

```
public static class EventoService
{
    public static EventoAcademico Registrar(
        IRepository<EventoAcademico, int> repo, EventoAcademico evento)
    {
        // Validações (regra de negócio)
        if (string.IsNullOrWhiteSpace(evento.Tipo))
            throw new ArgumentException("Tipo obrigatório");
        
        return repo.Add(evento);
    }

    public static IReadOnlyList<EventoAcademico> ListarPorTipo(
        IRepository<EventoAcademico, int> repo, string tipo)
    {
        return repo.ListAll()
                   .Where(e => e.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                   .ToList();
    }

    public static bool MarcarComoNotificado(
        IRepository<EventoAcademico, int> repo, int id)
    {
        var evento = repo.GetById(id);
        if (evento == null) return false;
        
        var atualizado = evento with { JaNotificado = true };
        return repo.Update(atualizado);
    }
}
```

**Responsabilidades:**
- Regras de negócio (validações, filtros)
- Depende apenas de `IRepository<T, TId>`
- Não conhece implementação concreta

---

## 5. Cenários de teste sem I/O

### Cenário 1: Inserir e listar

```
// Arrange
IRepository<EventoAcademico, int> repo = 
    new InMemoryRepository<EventoAcademico, int>(e => e.Id);

// Act
repo.Add(new EventoAcademico(1, "EnvioNota", "Nota disponível", 
                             DateTime.Now, "aluno@ex.com", false));

// Assert
var eventos = repo.ListAll();
Assert.Single(eventos);
Assert.Equal("EnvioNota", eventos.Tipo);
```

### Cenário 2: Buscar por ID

```
// Act
var evento = repo.GetById(1);

// Assert
Assert.NotNull(evento);
Assert.Equal("Nota disponível", evento.Descricao);
```

### Cenário 3: Atualizar evento

```
// Arrange
var original = repo.GetById(1);

// Act
var atualizado = original with { JaNotificado = true };
bool sucesso = repo.Update(atualizado);

// Assert
Assert.True(sucesso);
Assert.True(repo.GetById(1)?.JaNotificado);
```

### Cenário 4: Remover evento

```
// Act
bool removido = repo.Remove(1);

// Assert
Assert.True(removido);
Assert.Null(repo.GetById(1));
```

### Cenário 5: Filtrar por tipo (regra de negócio)

```
// Arrange
repo.Add(new EventoAcademico(1, "AlteracaoSala", "...", ...));
repo.Add(new EventoAcademico(2, "EnvioNota", "...", ...));

// Act
var alteracoes = EventoService.ListarPorTipo(repo, "AlteracaoSala");

// Assert
Assert.Single(alteracoes);
Assert.Equal("AlteracaoSala", alteracoes.Tipo);
```

---

## O que ganhou com Repository?

| Aspecto | Sem Repository | Com Repository |
|---------|----------------|----------------|
| **Acesso a dados** | Cliente manipula `List<T>` diretamente | Cliente usa `IRepository<T, TId>` |
| **Mutabilidade** | Coleção exposta permite alteração | `IReadOnlyList` protege estado interno |
| **Testabilidade** | Difícil substituir implementação | Fácil: injeta `FakeRepository` |
| **Troca de persistência** | Reescrever todo código cliente | Trocar só a implementação do repo |
| **Separação de responsabilidades** | Mistura domínio e acesso | Domínio/Serviço separado de acesso |

---

## Vantagens do padrão Repository

1. **Abstração de persistência:** Cliente não sabe se é memória, arquivo, BD
2. **Testabilidade:** Fácil criar `FakeRepository` para testes
3. **Ponto único de acesso:** Centraliza operações de dados
4. **Proteção da coleção:** `IReadOnlyList` impede mutação externa
5. **Facilita migração:** InMemory → SQL → NoSQL sem mexer no cliente

---

## Possíveis evoluções

- **Fase 6:** Repository com persistência em arquivo JSON
- **Fase 7:** Repository com Entity Framework (SQL)
- **Fase 8:** Unit of Work + Repository pattern
- **Fase 9:** CQRS (Command Query Responsibility Segregation)

---

## Princípios aplicados

- ✅ **Single Responsibility:** Repository cuida só de acesso; Serviço cuida de regras
- ✅ **Open/Closed:** Trocar implementação não modifica cliente
- ✅ **Liskov Substitution:** Qualquer `IRepository<T,TId>` pode substituir outro
- ✅ **Dependency Inversion:** Cliente depende de `IRepository`, não de `InMemoryRepository`

---
