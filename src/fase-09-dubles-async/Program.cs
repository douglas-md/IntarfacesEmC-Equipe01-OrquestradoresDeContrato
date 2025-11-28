
using System.Threading.Channels;
using Dominio;

Console.WriteLine("=== Fase 9 – Demonstração ao vivo de Pump com retentativa ===\n");

// Dados de exemplo (3 eventos pendentes)
var eventos = new[]
{
    new EventoAcademico(0, "AlteracaoSala", "Sala mudou para 501", DateTime.Now.AddHours(1), "aluno1@facul.br", false),
    new EventoAcademico(0, "EnvioNota",    "Nota P2 liberada",     DateTime.Now.AddDays(1),  "aluno1@facul.br", false),
    new EventoAcademico(0, "Lembrete",     "Inscrição encerra amanhã", DateTime.Now.AddHours(5), "aluno1@facul.br", false)
};

// Reader fake em memória (simula leitura de banco/arquivo)
var reader = new FakeAsyncReader(eventos.Where(e => !e.JaNotificado));

// Writer que falha 2 vezes e depois funciona (simula serviço externo instável)
var writer = new FakeAsyncWriter(falhasIniciais: 2);

// Clock fake para controle total do tempo
var clock = new FakeClock { Now = DateTimeOffset.Now };

var pump = new PumpEventosService(reader, writer, clock);

Console.WriteLine("Iniciando processamento com retentativa (máx 3 tentativas)...\n");

var processados = await pump.ProcessarPendentesAsync();

Console.WriteLine($"\nProcessamento concluído!");
Console.WriteLine($"Eventos processados com sucesso: {processados}");
Console.WriteLine($"Tentativas que falharam e foram retentadas: {writer.FalhasAcumuladas}");
Console.WriteLine($"Eventos realmente enviados: {writer.Escritos.Count}");

// Exibe o que foi efetivamente enviado
foreach (var e in writer.Escritos)
    Console.WriteLine($"   Enviado: {e.Tipo} – {e.Descricao}");