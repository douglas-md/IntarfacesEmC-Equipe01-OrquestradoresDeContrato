using Xunit;

namespace Tests.Integration;

public class JsonRepositoryIntegrationTests
{
    [Fact]
    public void Dados_Sobrevivem_Entre_Instancias()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        
        try
        {
            // Act - Primeira instância
            var repo1 = new JsonEventoRepository(tempFile);
            var evento = repo1.Add(new EventoAcademico(0, "TesteIntegracao", "Evento de integração", 
                DateTime.Now, "integracao@test.com"));

            // Assert - Segunda instância deve ver os mesmos dados
            var repo2 = new JsonEventoRepository(tempFile);
            var eventos = repo2.ListAll();
            var eventoRecuperado = repo2.GetById(evento.Id);

            Assert.Single(eventos);
            Assert.NotNull(eventoRecuperado);
            Assert.Equal("TesteIntegracao", eventoRecuperado.Tipo);
            Assert.Equal(evento.Id, eventoRecuperado.Id);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void Atualizacao_DevePersistir_EntreInstancias()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        
        try
        {
            var repo1 = new JsonEventoRepository(tempFile);
            var evento = repo1.Add(new EventoAcademico(0, "Teste", "Original", DateTime.Now, "test@test.com"));

            // Act
            var atualizado = evento with { Descricao = "Atualizada" };
            repo1.Update(atualizado);

            // Assert
            var repo2 = new JsonEventoRepository(tempFile);
            var recuperado = repo2.GetById(evento.Id);

            Assert.NotNull(recuperado);
            Assert.Equal("Atualizada", recuperado.Descricao);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }
}