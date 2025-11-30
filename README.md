# IntarfacesEmC-Equipe01-OrquestradoresDeContrato

Equipe:  [Douglas A. Garcia RA - 2539110], [Pedro Henrique Damasceno Gonçalves - 2509636], [Emmanuelly Madeira - 2388472]

# Sistema de Notificação de Eventos Acadêmicos

Este repositório documenta, fase a fase, o desenvolvimento incremental de um sistema de notificações acadêmicas (como alteração de sala e envio de notas), abordando desde a concepção conceitual até implementações orientadas a objetos e interfaces em C#.

O objetivo é exercitar e demonstrar a evolução de um projeto realista, incluindo análise de requisitos, design procedural, refatoração OO e práticas de documentação e testes.

***

Estrutura do Projeto

## Sumário

- [README.md](README.md) — Descrição geral do projeto (raiz)
- [src/](src/) — Código e documentação por fase
    - [Fase 0 - Aquecimento/](src/fase-00-aquecimento/) — Contratos e políticas conceituais
    - [Fase 1 - Heuristica/](src/fase-01-heuristica/) — Evolução arquitetural (procedural, OO, interface)
    - [Fase 2 - Procedural/](src/fase-02-procedural/) — Código funcional procedural mínimo (C#)
    - [Fase 3 - OO-sem-interface/](src/fase-03-OOseminterface/)
    - [Fase 4 - Interface/](src/fase-04-interface/NotificacaoInterface) — Refino OO com interfaces plugáveis
    - [Fase 5 — Repository InMemory](./src/fase-05-repositoryinmemory/RepositoryEventos/) - Implementação de repositório em memória
    - [Fase 6 — Repositório CSV](./src//fase-06-repository-csv/) - Persistência usando arquivos CSV.
    - [Fase 7 — Repositório JSON](./src/fase-07-repositoryjson/) -  Persistência usando JSON.
    - [Fase 8 — ISP na prática](./src/fase-08-isp/)
    - [Fase 9 — Dubles async](./src/fase-09-dubles-async/)
    - [Fase 10 — Cheiros e antidotos](./src/fase-10-cheiros-antidotos/)
    - [Fase 11 — mini projeto](./src/fase-11-mini-projeto/)

- [docs/arquitetura/](docs/arquitetura/) — Diagramas, mapas e decisões arquiteturais
- [tests/](tests/) — (opcional) Testes automatizados das fases futuras


***

## Descrição de cada pasta/fase

| Pasta/Fase                      | Conteúdo e Finalidade                                                                                |
|---------------------------------|------------------------------------------------------------------------------------------------------|
| fase-00-aquecimento             | Apresentação dos objetivos, definição de contratos e variações de implementação/política. Markdown.  |
| fase-01-heuristica              | Discussão da evolução arquitetural: procedural $$\rightarrow$$ OO $$\rightarrow$$ interfaces. Markdown, mapas e tabelas. |
| fase-02-procedural              | Código executável C# simples, lidando com seleção de modos e canais via fluxos condicionais.         |
| fase-03-oo-sem-interface        | Refatoração para orientação a objetos: entidades, modularização inicial e separação de responsabilidades. |
| fase-04-interface-NotificacaoInterface               | Utilização de interfaces para plugabilidade, política desacoplada, testabilidade (em andamento).     |
| fase-05-repositoryinmemory-RepositoryEventos    | Implementação de repositório em memória..                                                            |
| fase-06-repository-csv         | Persistência usando arquivos CSV.                                                                    |
| fase-07-repositoryjson        | Persistência usando JSON.                                                                            |
| fase-08-Interface-Segregation-Principle        | Separação de contratos para solidificar a arquitetura.                                |
| fase-09-Dubles-async                      |                                |
| fase-10-cheiros-e-antidotos               |                                |
| fase-11-mini-projetos                     |                                |
| docs/arquitetura                | Diagramas visuais (mapas mentais, comparativos, fluxos) e decisões de design documentadas.           |
| tests/                          | Testes automatizados de fases avançadas (opcional/in progress).                                      |

Como navegar
- Cada pasta de fase (em `src/`) contém um `README.md` próprio com detalhes, exemplos, fluxos e decisões.
- Diagramas, mapas mentais e critérios de decisão encontram-se em `docs/arquitetura/`.
- (Quando aplicável) Testes automatizados ficarão em `tests/`.

Como executar o código (a partir da Fase 2)

1. Instale o [.NET SDK](https://dotnet.microsoft.com/download)
2. Entre na pasta do projeto desejado. Exemplo para a Fase 2:

   bash
   cd src/fase-02-procedural/NotificacaoProcedural
   dotnet run
   
4. Consulte o `README.md` da fase para entender cenários testados e exemplos de saída.

Observações
- O README raiz serve para apresentar o projeto completo, orientar seu uso e direcionar para a documentação das fases.
- Para instruções detalhadas, exemplos de código, contexto didático e decisões de design, utilize sempre os READMEs próprios de cada fase (em `src/`).
- Arquivos em `docs/arquitetura/` detalham visualmente as opções e racional do projeto.
- Testes automatizados (quando houver) visam garantir regressão e evolução controlada.
