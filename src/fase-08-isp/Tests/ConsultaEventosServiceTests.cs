using Xunit;
using Fase8Isp.Servicos;
using Fase8Isp.Dubles;
using System.Linq;

namespace Fase8Isp.Tests
{
    public class ConsultaEventosServiceTests
    {
        [Fact]
        public void ListarPendentes_DeveRetornarApenasEventosNaoNotificados()
        {
            // Arrange
            var fakeRepo = new FakeReadRepository(); // Usa o Dublê SÓ de Leitura
            var service = new ConsultaEventosService(fakeRepo);

            // Act
            var pendentes = service.ListarPendentes();

            // Assert
            // O FakeRead tem 3 eventos, 2 não notificados (Id 1 e 3)
            Assert.Equal(2, pendentes.Count); 
            Assert.Contains(pendentes, e => e.Id == 1);
            Assert.Contains(pendentes, e => e.Id == 3);
            Assert.True(pendentes.All(e => !e.JaNotificado));
        }

        [Fact]
        public void BuscarPorId_DeveRetornarEventoCorreto()
        {
            // Arrange
            var fakeRepo = new FakeReadRepository();
            var service = new ConsultaEventosService(fakeRepo);

            // Act
            var evento = service.BuscarPorId(1);

            // Assert
            Assert.NotNull(evento);
            Assert.Equal("AlteracaoSala", evento!.Tipo);
        }
    }
}