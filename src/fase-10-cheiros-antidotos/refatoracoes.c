#include "refatoracoes.c"

// Antídoto 5: Validação centralizada
bool evento_valido(Evento* evento) {
    return evento != NULL && 
           evento->id > 0 && 
           strlen(evento->tipo) > 0 &&
           strlen(evento->descricao) > 0;
}

// Antídoto 1: Parsing separado
Evento* parsear_evento(char* linha) {
    if (linha == NULL || strlen(linha) == 0) {
        return NULL;
    }
    
    static Evento evento;
    char* token = strtok(linha, ",");
    
    if (token == NULL) return NULL;
    evento.id = atoi(token);
    
    token = strtok(NULL, ",");
    if (token == NULL) return NULL;
    strncpy(evento.tipo, token, sizeof(evento.tipo) - 1);
    
    token = strtok(NULL, "\n");
    if (token != NULL) {
        strncpy(evento.descricao, token, sizeof(evento.descricao) - 1);
    }
    
    return &evento;
}

// Antídoto 1: Validação separada
bool validar_evento(Evento* evento) {
    return evento_valido(evento);
}

// Antídoto 6: Cálculo de delay exponencial
int calcular_delay_exponencial(int tentativa) {
    return DELAY_BASE_MS * (1 << tentativa); // Exponencial: 100, 200, 400ms
}

// Antídoto 6: Execução com retentativa
bool executar_com_retentativa(RetryPolicy* policy, bool (*acao)(void*), void* contexto) {
    for (int tentativa = 0; tentativa < policy->max_tentativas; tentativa++) {
        if (acao(contexto)) {
            return true;
        }
        
        if (tentativa < policy->max_tentativas - 1) {
            int delay_ms = policy->calcular_delay(tentativa);
            usleep(delay_ms * 1000); // Converter para microseconds
        }
    }
    return false;
}