#include "refatoracoes.h"
#include <stdio.h>

// Implementação fake para testes
void fake_escrever(const char* linha) {
    printf("[FAKE IO] %s\n", linha);
}

int fake_ler(char* buffer, size_t tamanho) {
    const char* dados_fake = "1,AlteracaoSala,Sala 101 alterada";
    strncpy(buffer, dados_fake, tamanho);
    return strlen(dados_fake);
}

// Ação de exemplo para retentativa
bool acao_exemplo(void* contexto) {
    static int contador = 0;
    contador++;
    printf("Tentativa %d\n", contador);
    return (contador >= 2); // Sucesso na segunda tentativa
}

int main() {
    printf("=== Demonstração dos Antídotos ===\n\n");
    
    // Antídoto 2: IO injetado
    IOHandler io_fake = {
        .escrever = fake_escrever,
        .ler = fake_ler
    };
    
    char buffer[BUFFER_TAMANHO];
    io_fake.ler(buffer, sizeof(buffer));
    
    // Antídoto 1: Processamento separado
    Evento* evento = parsear_evento(buffer);
    
    if (validar_evento(evento)) {
        printf("Evento válido: ID=%d, Tipo=%s\n", evento->id, evento->tipo);
        io_fake.escrever("Evento processado com sucesso");
    }
    
    // Antídoto 6: Retentativa controlável
    RetryPolicy policy = {
        .max_tentativas = 3,
        .delay_base_ms = 100,
        .calcular_delay = calcular_delay_exponencial
    };
    
    printf("\nTestando retentativa...\n");
    bool sucesso = executar_com_retentativa(&policy, acao_exemplo, NULL);
    printf("Resultado: %s\n", sucesso ? "SUCESSO" : "FALHA");
    
    return 0;
}