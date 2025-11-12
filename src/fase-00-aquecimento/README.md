Fase 0 - Aquecimento Conceitual

Contexto

Esta fase treina o olhar de design para identificar:
- Contratos (o que fazer) vs Implementações (como fazer)
- Peças alternáveis que resolvem o mesmo problema por caminhos diferentes
- Políticas simples para escolha entre implementações

Tema Escolhido pela Equipe

Sistema de Notificação de Eventos Acadêmicos

Escolhemos este tema por sua proximidade com nossa realidade como estudantes e por 
permitir identificar trade-offs concretos (urgência, custo, disponibilidade tecnológica).

Artefatos Entregues

- [ContratosDeCapacidade.md] - Descrição completa dos 2 casos (formato de entrega)

Checklist de Qualidade Aplicado

| Critério | Status | Observação |
|----------|:------:|------------|
| Cada caso tem objetivo, contrato, 2 implementações e política | Completo |
| Contrato não revela "como" | Validado pela equipe |
| Implementações são alternáveis | Trade-offs reais identificados |
| Política concreta e aplicável | Baseada em tempo/urgência |
| Pelo menos um risco/observação por caso | Custo, latência, dependências |

Reflexões da Equipe

1. Aprendizado principal: Percebemos que é comum confundir "contrato" com "implementação" 
   no início. Ex: querer escrever "enviar e-mail" como contrato.

2. Desafio enfrentado: Encontrar implementações genuinamente alternáveis (não apenas 
   variações cosméticas da mesma técnica).

3. Conexão com próximas fases: Os contratos identificados aqui servirão de base para 
   as interfaces da Fase 4.
