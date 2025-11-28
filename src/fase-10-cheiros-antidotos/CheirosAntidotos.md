
# Fase 10 — Cheiros e antídotos (6 refatorações reais do nosso projeto)

## 1. Função monolítica com múltiplas responsabilidades → Separação de concerns
**Cheiro:** Funções grandes que fazem parsing, validação e processamento juntos

**Antes (exemplo do processador de eventos):**
```c
void processar_evento_completo(char* linha) {
    // parsing
    char* token = strtok(linha, ",");
    // validação
    if (strlen(token) == 0) return;
    // processamento
    // formatação de saída
    // etc...
}
Depois:

c
Evento* parsear_evento(char* linha);
bool validar_evento(Evento* evento);
void processar_evento(Evento* evento);
void formatar_saida(Evento* evento);
Antídoto: Single Responsibility Principle
Prova: Cada função pode ser testada isoladamente.

2. Acoplamento direto com I/O → Injeção de dependências
Cheiro: Funções que chamam diretamente printf, fopen, etc.

Antes:

c
void salvar_evento(Evento* evento) {
    FILE* file = fopen("eventos.csv", "a");
    fprintf(file, "%d,%s,%s\n", evento->id, evento->tipo, evento->descricao);
    fclose(file);
}
Depois:

c
typedef struct {
    void (*escrever)(const char* linha);
    void (*ler)(char* buffer, size_t tamanho);
} IOHandler;

void salvar_evento(Evento* evento, IOHandler* io) {
    char linha[256];
    snprintf(linha, sizeof(linha), "%d,%s,%s", evento->id, evento->tipo, evento->descricao);
    io->escrever(linha);
}
Antídoto: Dependency Inversion Principle
Prova: Testes podem usar IOHandler fake que não toca no sistema de arquivos real.

3. Magic numbers e strings hardcoded → Constantes nomeadas
Cheiro: Números e strings mágicas espalhadas pelo código

Antes:

c
if (evento->tipo == 1) { // O que é 1?
    processar_alteracao_sala(evento);
} else if (evento->tipo == 2) { // O que é 2?
    processar_envio_nota(evento);
}

char buffer[256]; // Por que 256?
Depois:

c
typedef enum {
    TIPO_ALTERACAO_SALA = 1,
    TIPO_ENVIO_NOTA = 2,
    TIPO_MAXIMO_EVENTOS = 100
} TipoEvento;

#define BUFFER_TAMANHO 256
#define MAX_EVENTOS 100

if (evento->tipo == TIPO_ALTERACAO_SALA) {
    processar_alteracao_sala(evento);
}
Antídoto: Constantes nomeadas + enums
Prova: Código mais legível e fácil de manter.

4. Estruturas com campos soltos → Value Objects
Cheiro: Estruturas com muitos campos primitivos sem semântica

Antes:

c
typedef struct {
    int id;
    char tipo[50];
    char descricao[200];
    int sala; // às vezes usado, às vezes não
    float nota; // idem
    // ... muitos campos opcionais
} Evento;
Depois:

c
typedef struct {
    int numero;
    char bloco[10];
} Sala;

typedef struct {
    float valor;
    char disciplina[50];
} Nota;

typedef struct {
    int id;
    char tipo[50];
    char descricao[200];
    union {
        Sala sala;
        Nota nota;
    } dados;
} Evento;
Antídoto: Value Objects + Union para dados variantes
Prova: Melhor type safety e semântica clara.

5. Código duplicado em validações → Funções utilitárias
Cheiro: Mesmas validações repetidas em múltiplos lugares

Antes:

c
// Em processar_evento.c
if (evento == NULL || evento->id <= 0) {
    return;
}

// Em salvar_evento.c  
if (evento == NULL || evento->id <= 0) {
    return;
}

// Em notificar_evento.c
if (evento == NULL || evento->id <= 0) {
    return;
}
Depois:

c
bool evento_valido(Evento* evento) {
    return evento != NULL && evento->id > 0 && 
           strlen(evento->tipo) > 0 &&
           strlen(evento->descricao) > 0;
}

// Uso consistente:
if (!evento_valido(evento)) {
    return;
}

Prova: Validação consistente em toda aplicação.

6. Retentativa com busy-waiting → Estratégia controlável
Cheiro: Loops de retentativa com delay fixo

Antes:

c
for (int i = 0; i < 3; i++) {
    if (processar_evento(evento)) {
        break;
    }
    sleep(1); // Delay fixo, bloqueante
}
Depois:

c
typedef struct {
    int max_tentativas;
    int delay_base_ms;
    int (*calcular_delay)(int tentativa);
} RetryPolicy;

bool executar_com_retentativa(RetryPolicy* policy, bool (*acao)(void*), void* contexto) {
    for (int i = 0; i < policy->max_tentativas; i++) {
        if (acao(contexto)) return true;
        int delay = policy->calcular_delay(i);
        usleep(delay * 1000); // ms para microseconds
    }
    return false;
}
Antídoto: Strategy Pattern para retentativas
Prova: Política de retentativa configurável e testável.

Conclusão da equipe
Aplicamos 6 refatorações específicas para C que tornam o código mais limpo, testável e mantenível. Cada mudança foi guiada por princípios SOLID adaptados para programação procedural em C.

Resultado: código C profissional, com responsabilidades claras e facilidade de teste.

text

## 🔧 **Implementação dos Antídotos**

Vou criar os arquivos de implementação:

### `src/fase-10-cheiros-antidotos/refatoracoes.h`
```c
#ifndef REFATORACOES_H
#define REFATORACOES_H

#include <stdio.h>
#include <stdbool.h>
#include <string.h>
#include <unistd.h>

// Antídoto 3: Constantes nomeadas
typedef enum {
    TIPO_ALTERACAO_SALA = 1,
    TIPO_ENVIO_NOTA = 2
} TipoEvento;

#define BUFFER_TAMANHO 256
#define MAX_EVENTOS 100
#define MAX_TENTATIVAS 3
#define DELAY_BASE_MS 100

// Antídoto 4: Value Objects
typedef struct {
    int numero;
    char bloco[10];
} Sala;

typedef struct {
    float valor;
    char disciplina[50];
} Nota;

typedef struct {
    int id;
    char tipo[50];
    char descricao[200];
    union {
        Sala sala;
        Nota nota;
    } dados;
} Evento;

// Antídoto 2: Injeção de dependências
typedef struct {
    void (*escrever)(const char* linha);
    int (*ler)(char* buffer, size_t tamanho);
} IOHandler;

// Antídoto 6: Estratégia de retentativa
typedef struct {
    int max_tentativas;
    int delay_base_ms;
    int (*calcular_delay)(int tentativa);
} RetryPolicy;

// Antídoto 5: Funções utilitárias
bool evento_valido(Evento* evento);
Evento* parsear_evento(char* linha);
bool validar_evento(Evento* evento);

// Antídoto 6: Implementação de retentativa
int calcular_delay_exponencial(int tentativa);
bool executar_com_retentativa(RetryPolicy* policy, bool (*acao)(void*), void* contexto);

#endif
 