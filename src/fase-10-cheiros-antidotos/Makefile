CC = gcc
CFLAGS = -Wall -Wextra -std=c99 -g
SRC = refatoracoes.c exemplo_uso.c
OBJ = $(SRC:.c=.o)
TARGET = exemplo_refatoracoes

all: $(TARGET)

$(TARGET): $(OBJ)
	$(CC) $(CFLAGS) -o $@ $^

%.o: %.c
	$(CC) $(CFLAGS) -c $<

clean:
	rm -f $(OBJ) $(TARGET)

test: $(TARGET)
	./$(TARGET)

.PHONY: all clean test
