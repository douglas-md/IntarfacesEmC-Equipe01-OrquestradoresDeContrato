# IntarfacesEmC-Equipe01-OrquestradoresDeContrato

Equipe:
Douglas A. Garcia RA 2539110
Pedro H. Damasceno RA

Sumário das fases

0 - Aquecimento conceitual: contratos de capacidade (sem código)

Enunciado: Liste 2 situações reais com mesmo objetivo e peças alternáveis. Nomeie o contrato (o
que) e duas possíveis implementações (como).
Descrição: Refere-se ao aquecimento do guia. Em 4–6 linhas por caso: objetivo, contrato, duas peças e
uma política simples (ex.: “à noite usar A; em urgência, B”).

1 — Heurística antes do código (mapa mental)

Enunciado: Desenhe um mapa de evolução para um problema trivial escolhido pela equipe.
Descrição: Uma página com: (1) versão procedural (onde surgem if/switch ), (2) OO sem interface
(quem muda o quê), (3) com interface (qual contrato permite alternar). Liste 3 sinais de alerta
previstos.

2 — Procedural mínimo (ex.: formatar texto)
Enunciado: Implemente a ideia de modos (mínimo 3 + padrão) para um objetivo simples.
Descrição: Entregue função/fluxo e 5 cenários de teste/fronteira descritos em texto. Explique em
poucas linhas por que essa abordagem não escala.

3 — OO sem interface
Enunciado: Transforme a solução anterior em uma hierarquia com variações concretas e base comum.
Descrição: Substitua decisões por polimorfismo. Mantenha classes concretas restritas a sua
responsabilidade e descreva o que melhorou/ficou rígido.

4 — Interface plugável e testável

Enunciado: Defina um contrato claro e refatore o cliente para depender dele.
Descrição: Explique como alternar implementações sem mudar o cliente e como dobrar a
dependência em testes (injeção simples).

5 — Essenciais de interfaces em C
Enunciado: Proponha duas interfaces do seu domínio e uma classe que implementa duas.
Descrição: Explique quando usar implementação explícita, quando genéricos com constraints
ajudam e quando default members devem ser evitados.

...
