using Dominio;
using Dominio.Contratos;
using Servicos;
using Moq;
using Xunit;

namespace Tests.Unit;

public class ConsultaEventosServiceTests
{
    private readonly Mock<IReadRepository<EventoAcademico, int>> _mockRepo;
    private readonly ConsultaEventosService _service;

    public ConsultaEventosServiceTests()
    {
        _mockRepo = new Mock<IReadRepository<EventoAcademico, int>>();
        _service = new ConsultaEventosService(_mockRepo.Object);
    }

    [Fact]
    public void ObterPendentes_DeveRetornarApenasEventosNaoNotificados()
    {
        // Arrange
        var eventos = new List<EventoAcademico>
        {
            new(1, "Tipo1", "Desc1", DateTime.Now, "email1@test.com", false),
            new(2, "Tipo2", "Desc2", DateTime.Now, "email2@test.com", true),
            new(3, "Tipo3", "Desc3", DateTime.Now, "email3@test.com", false)
        };

        _mockRepo.Setup(r => r.Find(It.IsAny<Func<EventoAcademico, bool>>()))
                .Returns((Func<EventoAcademico, bool> predicate) => 
                    eventos.Where(predicate).ToList().AsReadOnly());

        // Act
        var pendentes = _service.ObterPendentes();

        // Assert
        Assert.Equal(2, pendentes.Count);
        Assert.All(pendentes, e => Assert.False(e.JaNotificado));
    }

    [Fact]
    public void ObterPorTipo_DeveRetornarEventosDoTipoEspecificado()
    {
        // Arrange
        var eventos = new List<EventoAcademico>
        {
            new(1, "AlteracaoSala", "Desc1", DateTime.Now, "email1@test.com"),
            new(2, "EnvioNota", "Desc2", DateTime.Now, "email2@test.com"),
            new(3, "AlteracaoSala", "Desc3", DateTime.Now, "email3@test.com")
        };

        _mockRepo.Setup(r => r.Find(It.IsAny<Func<EventoAcademico, bool>>()))
                .Returns((Func<EventoAcademico, bool> predicate) => 
                    eventos.Where(predicate).ToList().AsReadOnly());

        // Act
        var resultado = _service.ObterPorTipo("AlteracaoSala");

        // Assert
        Assert.Equal(2, resultado.Count);
        Assert.All(resultado, e => Assert.Equal("AlteracaoSala", e.Tipo));
    }
}