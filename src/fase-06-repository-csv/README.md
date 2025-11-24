# Fase 6 — Repository CSV (persistência em arquivo)

## Objetivo

Evoluir o Repository da Fase 5 para persistir em arquivo CSV, mantendo o mesmo contrato (`IRepository<EventoAcademico, int>`) e a mesma temática de eventos acadêmicos. O cliente continua falando indiretamente com o repositório, via serviço de domínio (`EventoService`), enquanto o `CsvEventoRepository` cuida de toda a lógica de I/O em CSV.

## Estrutura

fase-06-repository-csv/
RepositoryEventosCsv/
Dominio/
EventoAcademico.cs
Repositorio/
IRepository.cs
CsvEventoRepository.cs
Servicos/
EventoService.cs
Program.cs
README.md

text

## Contrato (reaproveitado)

public interface IRepository<T, TId>
{
T Add(T entity);
T? GetById(TId id);
IReadOnlyList<T> ListAll();
bool Update(T entity);
bool Remove(TId id);
}

text

## Implementação CSV

Pontos principais do `CsvEventoRepository`:

- Usa `Dictionary<int, EventoAcademico>` internamente (via `List<EventoAcademico>` carregado do arquivo).
- Persiste em `eventos_academicos.csv` com:
  - Cabeçalho: `Id,Tipo,Descricao,DataHora,DestinatarioEmail,JaNotificado`
  - Separador: `,`
  - Escape de aspas: `"` → `""`
  - Encoding: UTF-8 sem BOM
- Implementa `Add`, `GetById`, `ListAll`, `Update`, `Remove` lendo e escrevendo o CSV.

Trecho ilustrativo:

private void SalvarTodos(List<EventoAcademico> eventos)
{
using var writer = new StreamWriter(_caminhoArquivo, false, new UTF8Encoding(false));
writer.WriteLine(Header);
foreach (var e in eventos.OrderBy(e => e.Id))
{
var linha = string.Join(",",
e.Id,
Escape(e.Tipo),
Escape(e.Descricao),
e.DataHora.ToString("O", CultureInfo.InvariantCulture),
Escape(e.DestinatarioEmail),
e.JaNotificado
);
writer.WriteLine(linha);
}
}

text

## Serviço de domínio (cliente indireto do Repository)

public static class EventoService
{
public static EventoAcademico Registrar(
IRepository<EventoAcademico, int> repo,
EventoAcademico evento)
{
// validações de domínio...
return repo.Add(evento);
}

text
public static IReadOnlyList<EventoAcademico> ListarTodos(
    IRepository<EventoAcademico, int> repo) =>
    repo.ListAll();
}

text

**Importante:**  
- O **cliente (Program)** fala com `EventoService`.  
- `EventoService` fala com `IRepository<EventoAcademico,int>`.  
- O cliente **não acessa coleções nem arquivos diretamente**.

## Cenários exercitados

1. Registrar eventos acadêmicos com informações completas.
2. Persistir os eventos em CSV (dados sobrevivem entre execuções).
3. Listar todos os eventos a partir do arquivo.
4. Filtrar por tipo (via serviço e repository).
5. Atualizar um evento (marcar como notificado) e regravar no CSV.
6. Remover um evento e verificar a consistência no arquivo.

## O que esta fase adiciona em relação à Fase 5

- Persistência real em arquivo (dados sobrevivem ao fim do programa).
- Disciplina de I/O (encoding, cabeçalho, escape de caracteres).
- Mantém o mesmo contrato do repository, permitindo trocar InMemory/CSV sem mudar o cliente.
- Cliente continua protegido de detalhes de infraestrutura (fala só com serviço + contrato).