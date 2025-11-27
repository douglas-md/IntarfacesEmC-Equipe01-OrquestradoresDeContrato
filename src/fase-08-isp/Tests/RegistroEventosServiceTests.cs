using Xunit;
using Fase8Isp.Servicos;
using Fase8Isp.Dubles;
using Fase8Isp.Dominio;
using System.Linq;

namespace Fase8Isp.Tests
{
    public class RegistroEventosServiceTests
    {
        [Fact]
        public void RegistrarNovo_DeveAdicionarEventoAoRepositorioEForncerId()
        {
            // Arrange
            var fakeRepo = new FakeWriteRepository(); // Usa o Dublê SÓ de Escrita
            var service = new RegistroEventosService(fakeRepo);
            var novoEvento = new EventoAcademico(0, "Novo", "Descrição", DateTime.Now, "teste@teste.com", false);

            // Act
            var eventoRegistrado = service.RegistrarNovo(novoEvento);

            // Assert
            Assert.NotEqual(0, eventoRegistrado.Id); // ID foi gerado
            Assert.Single(fakeRepo.Data); // Evento está na base do Fake
            Assert.Equal("Novo", fakeRepo.Data.First().Tipo);
        }

        [Fact]
        public void Remover_DeveRemoverEventoPeloId()
        {
            // Arrange
            var fakeRepo = new FakeWriteRepository();
            var service = new RegistroEventosService(fakeRepo);
            
            // Simula adição para garantir que a remoção funciona
            var eventoParaRemover = new EventoAcademico(101, "ParaRemover", "Desc", DateTime.Now, "r@r.com", false);
            // Ao adicionar, o FakeWriteRepository gera um novo Id; devemos usar o Id retornado pelo Add
            var eventoAdicionado = fakeRepo.Add(eventoParaRemover);
            Assert.Single(fakeRepo.Data);

            // Act
            var sucesso = service.Remover(eventoAdicionado.Id);

            // Assert
            Assert.True(sucesso);
            Assert.Empty(fakeRepo.Data);
        }
    }
}