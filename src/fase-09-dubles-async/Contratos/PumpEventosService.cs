using Dominio;



public sealed class PumpEventosService

{

    private readonly IAsyncReader<EventoAcademico> _reader;

    private readonly IAsyncWriter<EventoAcademico> _writer;

    private readonly IClock _clock;



    public PumpEventosService(

        IAsyncReader<EventoAcademico> reader,

        IAsyncWriter<EventoAcademico> writer,

        IClock clock)

        => (_reader, _writer, _clock) = (reader, writer, clock);



    public async Task<int> ProcessarPendentesAsync(CancellationToken ct = default)

    {

        var processados = 0;



        await foreach (var evento in _reader.ReadAsync(ct).WithCancellation(ct))

        {

            var tentativa = 0;

            const int maxTentativas = 3;



            while (true)

            {

                ct.ThrowIfCancellationRequested();



                try

                {

                    await _writer.WriteAsync(evento with { JaNotificado = true }, ct);

                    processados++;

                    break;

                }

                catch (OperationCanceledException) { throw; }

                catch when (++tentativa <= maxTentativas)

                {

                    // Backoff exponencial simulado via clock fake nos testes

                    var delay = TimeSpan.FromMilliseconds(100 * Math.Pow(2, tentativa - 1));

                    var deadline = _clock.Now.Add(delay);

                    while (_clock.Now < deadline && !ct.IsCancellationRequested) { await Task.Yield(); }

                }

            }

        }



        return processados;

    }

}
