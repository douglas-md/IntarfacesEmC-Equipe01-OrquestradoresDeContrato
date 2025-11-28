using Dominio;
using Dominio.Contratos;

var repo = new JsonEventoRepository();

var consulta = new ConsultaEventosService(repo);
var gestao   = new GestaoEventosService(repo);

Console.WriteLine("=== Mini-projeto Fase 11 – Eventos Acadêmicos ===\n");

gestao.Registrar(new(0, "AlteracaoSala", "Sala mudou para 501", DateTime.Now.AddHours(2), "aluno@facul.br"));
gestao.Registrar(new(0, "EnvioNota", "Nota da P2 liberada", DateTime.Now.AddDays(1), "aluno@facul.br"));

Console.WriteLine("Eventos pendentes:");
foreach (var e in consulta.Pendentes())
    Console.WriteLine($"  • {e.Tipo}: {e.Descricao} – {e.DataHora:dd/MM HH:mm}");

Console.WriteLine($"\nTotal cadastrados: {consulta.Todos().Count}");
Console.WriteLine("Dados salvos em eventos-academicos.json");
