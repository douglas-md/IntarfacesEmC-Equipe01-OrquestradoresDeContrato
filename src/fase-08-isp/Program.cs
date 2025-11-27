using Fase8Isp.Dominio;
using Fase8Isp.Implementacoes;
using Fase8Isp.Servicos;
using System.Linq;
using System;

namespace Fase8Isp
{
    internal class NewBaseType
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 08 - ISP (Interface Segregation Principle) ===\n");

            // 1. Ponto de Composição (Injeção de Dependência)
            // A implementação 'InMemory' atende aos dois contratos segregados (IRead e IWrite).
            var repository = new InMemoryEventoRepository();

            // 2. Clientes (Serviços) dependem apenas do mínimo necessário
            var consultaService = new ConsultaEventosService(repository); // SÓ IRead
            var registroService = new RegistroEventosService(repository);   // SÓ IWrite

            // 3. Uso do Sistema - Demonstração da segregação de responsabilidades
            Console.WriteLine("--- 1. Registrando novos eventos (RegistroService SÓ ESCREVE) ---");
            var evento1 = registroService.RegistrarNovo(
                new EventoAcademico(0, "TCC Apresentação", "Defesa final de TCC", DateTime.Now.AddDays(10), "aluno@facul.com", false)
            );
            var evento2 = registroService.RegistrarNovo(
                new EventoAcademico(0, "Reunião de Equipe", "Alinhamento de projeto", DateTime.Now.AddDays(5), "equipe@facul.com", false)
            );
            Console.WriteLine($"Evento 1 Registrado (ID: {evento1.Id})");
            Console.WriteLine($"Eventos no repositório: {repository.ListAll().Count}");

            Console.WriteLine("\n--- 2. Consultando Eventos Pendentes (ConsultaService SÓ LÊ) ---");
            Console.WriteLine($"Pendentes iniciais: {consultaService.ListarPendentes().Count}");

            Console.WriteLine("\n--- 3. Atualizando Evento (Mutação) ---");
            // Cliente LÊ via ConsultaService.
            var eventoParaAtualizar = consultaService.BuscarPorId(evento1.Id);

            if (eventoParaAtualizar is not null)
            {
                // Cliente ALTERA a entidade (nova instância do record).
                var eventoNotificado = eventoParaAtualizar with { JaNotificado = true };

                // Cliente PASSA para o serviço de ESCRETA (RegistroService SÓ ESCREVE).
                registroService.Atualizar(eventoNotificado);
                Console.WriteLine($"Evento {evento1.Id} marcado como Notificado (via RegistroService.Atualizar).");
            }

            Console.WriteLine("\n--- 4. Consultando Novamente (ISP em ação) ---");
            Console.WriteLine($"Pendentes restantes: {consultaService.ListarPendentes().Count}"); // Deve ser 1

            Console.WriteLine("\n--- 5. Removendo Evento ---");
            registroService.Remover(evento2.Id);
            Console.WriteLine($"Evento {evento2.Id} removido (via RegistroService.Remover).");

            Console.WriteLine($"\nItens totais no repositório (após remoção): {repository.ListAll().Count}");

            Console.WriteLine("\n=== FIM DA DEMONSTRAÇÃO ===");
        }
    } 
    }


