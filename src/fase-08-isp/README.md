# Fase 08 — ISP (Interface Segregation Principle)

**Diagnóstico da Interface Gorda**

Na fase anterior, o contrato `IRepository<T,TId>` com 5 métodos violava o **Princípio de Segregação de Interfaces (ISP)**. Os clientes eram forçados a depender de métodos que não usavam, aumentando o acoplamento e dificultando a escrita de dublês de teste.

**Solução Aplicada**

O contrato foi segregado em duas interfaces coesas:
- **`IReadRepository<out T, in TId>`:** Contém apenas métodos de **consulta** (`GetById`, `ListAll`).
- **`IWriteRepository<in T, in TId>`:** Contém apenas métodos de **mutação/escrita** (`Add`, `Update`, `Remove`).

**Efeitos Medidos (Os Ganhos do ISP)**

- **Acoplamento Reduzido:** Clientes como `ConsultaEventosService` dependem estritamente de `IReadRepository`, removendo o acoplamento desnecessário com métodos de escrita.
- **Testabilidade Aumentada:** Os Dublês (mocks/fakes) são minimalistas, tendo 60% menos código para serem implementados.
- **Princípio Open/Closed Preservado:** A criação de novas implementações de persistência (JSON, CSV, SQL) não exige nenhuma alteração nos serviços de Consulta ou Registro.

**Checklist Oficial — 100% Atendido**
- [x] Diagnóstico claro de quem usa o quê.
- [x] Contratos segregados, coesos e bem nomeados.
- [x] Consumidores refatorados dependem só do necessário.
- [x] Dublês mínimos e testes que comprovam a segregação (localizados na pasta `Tests`).

**Como Executar os Testes**

Para comprovar o baixo acoplamento e o funcionamento dos dublês:

```bash
cd src/fase-08-isp
dotnet test