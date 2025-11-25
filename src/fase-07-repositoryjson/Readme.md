# Fase 07 — Repository JSON

## Persistência de Eventos Acadêmicos usando arquivo JSON

Esta fase implementa o padrão Repository utilizando um arquivo **JSON** como mecanismo de persistência, mantendo o contrato de interface (`IRepository`) estabelecido nas fases anteriores.

- **Arquivo gerado:** `eventos_academicos.json`
- **Serialização:** Utiliza `System.Text.Json` para serializar e desserializar a lista de eventos.
- **Opções de Serialização:** Serialização em **camelCase**, **ignorando campos nulos**, e no **formato indentado** para facilitar a leitura.
- **Contrato:** A classe `JsonEventoRepository` implementa o contrato `IRepository<EventoAcademico, int>`, expondo os métodos: `Add`, `GetById`, `ListAll`, `Update`, `Remove`.
- **Fluxo de Dados:** Todas as operações de mutação (Add, Update, Remove) primeiro carregam o arquivo (`ListAll`), modificam a lista em memória e, em seguida, **reescrevem todo o arquivo JSON** (`SaveAll`).

### Como Executar e Testar
1. **Compilar e Rodar:** Execute o projeto a partir do `Program.cs`.
2. **Verificar a Saída:** Observe a saída no console para confirmar o registro, listagem, atualização (`JaNotificado`) e remoção de eventos.
3. **Checar o Arquivo:** Após a execução, confira que o arquivo **`eventos_academicos.json`** será criado na pasta de execução (`bin/Debug/net8.0/`) com os dados registrados, refletindo o evento #2 removido.

### Decisões de Design
* **Ponto de Composição:** A troca da implementação do repositório (ex: de CSV para JSON) ocorre em um único ponto no `Program.cs` (`IRepository<...> repo = new JsonEventoRepository(...)`), provando que o cliente (`EventoService`) não precisa ser alterado.
* **Ida e Volta:** O repositório garante a conversão de `List<EventoAcademico>` para JSON (`SaveAll`) e de JSON para `List<EventoAcademico>` (`ListAll`) de forma transparente para o cliente.
* **Troca sem Alterar Cliente:** O `EventoService.cs` continua funcionando perfeitamente, pois depende da abstração (`IRepository`), não dos detalhes da persistência JSON.

---