using Xunit;
using Moq;

namespace Tests.Unit;

public class GestaoEventosServiceTests
{
    private readonly Mock<IWriteRepository<EventoAcademico, int>> _mockWriteRepo;
    private readonly Mock<IReadRepository<EventoAcademico, int>> _mockReadRepo;
    private readonly GestaoEventosService _service;

    public GestaoEventosServiceTests()
    {
        _mockWriteRepo = new Mock<IWriteRepository<EventoAcademico, int>>();
        _mockReadRepo = new Mock<IReadRepository<EventoAcademico, int>>();
        _service = new GestaoEventosService(_mockWriteRepo.Object, _mockReadRepo.Object);
    }

    [Fact]
    public void RegistrarEvento_ComEventoValido_DeveChamarAddDoRepositorio()
    {
        // Arrange
        var evento = new EventoAcademico(0, "Teste", "Descrição", DateTime.Now, "test@email.com");
        var eventoSalvo = evento with { Id = 1 };
        
        _mockWriteRepo.Setup(r => r.Add(evento)).Returns(eventoSalvo);

        // Act
        var resultado = _service.RegistrarEvento(evento);

        // Assert
        Assert.Equal(1, resultado.Id);
        _mockWriteRepo.Verify(r => r.Add(evento), Times.Once);
    }

    [Fact]
    public void MarcarComoNotificado_ComEventoExistente_DeveAtualizarParaNotificado()
    {
        // Arrange
        var evento = new EventoAcademico(1, "Teste", "Descrição", DateTime.Now, "test@email.com", false);
        var eventoNotificado = evento with { JaNotificado = true };
        
        _mockReadRepo.Setup(r => r.GetById(1)).Returns(evento);
        _mockWriteRepo.Setup(r => r.Update(eventoNotificado)).Returns(true);

        // Act
        var resultado = _service.MarcarComoNotificado(1);

        // Assert
        Assert.True(resultado);
        _mockWriteRepo.Verify(r => r.Update(It.Is<EventoAcademico>(e => e.JaNotificado)), Times.Once);
    }

    [Fact]
    public void MarcarComoNotificado_ComEventoInexistente_DeveRetornarFalse()
    {
        // Arrange
        _mockReadRepo.Setup(r => r.GetById(999)).Returns((EventoAcademico?)null);

        // Act
        var resultado = _service.MarcarComoNotificado(999);

        // Assert
        Assert.False(resultado);
        _mockWriteRepo.Verify(r => r.Update(It.IsAny<EventoAcademico>()), Times.Never);
    }
}