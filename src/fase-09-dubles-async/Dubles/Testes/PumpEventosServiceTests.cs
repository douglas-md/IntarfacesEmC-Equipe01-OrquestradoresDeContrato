using Dominio;

using Xunit;



public class PumpEventosServiceTests

{

    private readonly EventoAcademico _evento = new(1, "AlteracaoSala", "Sala 305", DateTime.Now, "a@facul.com", false);



    [Fact]

    public async Task Sucesso_Simples()

    {

        var reader = new FakeAsyncReader(new[] { _evento });

        var writer = new FakeAsyncWriter();

        var clock = new FakeClock();

        var pump = new PumpEventosService(reader, writer, clock);



        var resultado = await pump.ProcessarPendentesAsync();



        Assert.Equal(1, resultado);

        Assert.Single(writer.Escritos);

    }



    [Fact]

    public async Task Retentativa_Com_Sucesso()

    {

        var reader = new FakeAsyncReader(new[] { _evento });

        var writer = new FakeAsyncWriter();

        writer.ConfigurarFalhas(2); // falha 2x, depois escreve

        var clock = new FakeClock();

        var pump = new PumpEventosService(reader, writer, clock);



        var resultado = await pump.ProcessarPendentesAsync();



        Assert.Equal(1, resultado);

        Assert.Single(writer.Escritos);

    }



    [Fact]

    public async Task Cancelamento_Parcial()

    {

        using var cts = new CancellationTokenSource();

        var reader = new FakeAsyncReader(Enumerable.Repeat(_evento, 10));

        var writer = new FakeAsyncWriter();

        var clock = new FakeClock();

        var pump = new PumpEventosService(reader, writer, clock);



        var task = pump.ProcessarPendentesAsync(cts.Token);

        await Task.Delay(50);

        cts.Cancel();



        await Assert. PartialSuccess(task, typeof(OperationCanceledException));

        Assert.True(writer.Escritos.Count < 5);

    }



    [Fact]

    public async Task Stream_Vazio()

    {

        var reader = new FakeAsyncReader(Array.Empty<EventoAcademico>());

        var writer = new FakeAsyncWriter();

        var clock = new FakeClock();

        var pump = new PumpEventosService(reader, writer, clock);



        var resultado = await pump.ProcessarPendentesAsync();

        Assert.Equal(0, resultado);

    }



    [Fact]

    public async Task Erro_No_Meio_Do_Stream()

    {

        var reader = new FakeAsyncReader(new[] { _evento }) { DeveFalharNoSegundo = true };

        var writer = new FakeAsyncWriter();

        var clock = new FakeClock();

        var pump = new PumpEventosService(reader, writer, clock);



        await Assert.ThrowsAsync<InvalidOperationException>(

            () => pump.ProcessarPendentesAsync());

    }

}



