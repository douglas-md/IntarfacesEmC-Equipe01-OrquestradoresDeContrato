# Fase 9 — Dublês avançados e testes assíncronos



**Objetivo cumprido 100%:**

- Contratos mínimos para tempo e I/O assíncrono

- Serviço de bombeamento com retentativa e backoff

- Testes cobrindo sucesso, erro, cancelamento, stream vazio e erro no meio

- Zero I/O real, tudo com dublês previsíveis



**Como executar os testes**

bash

cd src/fase-09-dubles-async

dotnet test





**Checklist oficial — tudo verde**

- [x] IClock, IAsyncReader<T>, IAsyncWriter<T>

- [x] Serviço com IAsyncEnumerable + retentativa

- [x] 5 cenários de teste (sucesso, retentativa, cancelamento, vazio, erro)

- [x] Backoff simulado via FakeClock (sem Thread.Sleep)

- [x] Diagnóstico completo