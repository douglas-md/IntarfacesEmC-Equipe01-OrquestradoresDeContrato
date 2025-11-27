# Diagnóstico — Fase 9: Dublês avançados e assíncronos



**Costuras extraídas:**

- IClock → controle total de tempo (backoff sem Thread.Sleep)

- IAsyncReader<T> → stream assíncrono testável

- IAsyncWriter<T> → escrita com falhas controladas



**Cenários testados (todos verdes):**

1. Sucesso simples

2. Retentativa com backoff simulado (3 tentativas)

3. Cancelamento parcial (OperationCanceledException)

4. Stream vazio

5. Erro no meio do stream



**Ganhos mensuráveis:**

- Testes 100% determinísticos

- Zero Thread.Sleep

- Backoff testado sem esperar segundos

- Cancelamento respeitado em todo o pipeline

- Dublês mínimos e focados