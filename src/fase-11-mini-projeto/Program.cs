namespace fase11
{   
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Mini-projeto Fase 11 – Sistema de Eventos Acadêmicos ===\n");

            // Ponto de composição centralizado
            var repo = new JsonEventoRepository();
            
            var consultaService = new ConsultaEventosService(repo);
            var gestaoService = new GestaoEventosService(repo, repo);

            // Demo: Criar eventos de exemplo
            CriarEventosExemplo(gestaoService);

            // Demo: Consultas
            ExibirRelatorios(consultaService);

            Console.WriteLine("\n=== Fim da demonstração ===");
            Console.WriteLine("Dados salvos em: eventos-academicos.json");
        }

        static void CriarEventosExemplo(GestaoEventosService gestao)
        {
            Console.WriteLine("Registrando eventos de exemplo...");

            gestao.RegistrarEvento(new EventoAcademico(
                Id: 0,
                Tipo: "AlteracaoSala",
                Descricao: "Sala mudou para laboratório 501",
                DataHora: DateTime.Now.AddHours(2),
                DestinatarioEmail: "aluno@faculdade.edu.br"
            ));

            gestao.RegistrarEvento(new EventoAcademico(
                Id: 0,
                Tipo: "EnvioNota",
                Descricao: "Nota da Prova B1 liberada",
                DataHora: DateTime.Now.AddDays(1),
                DestinatarioEmail: "aluno@faculdade.edu.br"
            ));

            Console.WriteLine("✓ Eventos registrados!\n");
        }

        static void ExibirRelatorios(ConsultaEventosService consulta)
        {
            Console.WriteLine("📊 RELATÓRIOS:\n");

            var pendentes = consulta.ObterPendentes();
            Console.WriteLine($"Eventos Pendentes: {pendentes.Count}");
            foreach (var evento in pendentes)
            {
                Console.WriteLine($"  • {evento.Tipo}: {evento.Descricao}");
            }

            var todos = consulta.ObterTodos();
            Console.WriteLine($"\nTotal de Eventos: {todos.Count}");
        }
    }
}