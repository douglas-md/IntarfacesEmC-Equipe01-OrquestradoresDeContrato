using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using RepositoryEventosCsv.Dominio;

namespace RepositoryEventosCsv.Repositorio
{
    /// <summary>
    /// Implementação de IRepository<EventoAcademico,int> com persistência em arquivo CSV.
    /// O cliente continua falando apenas com IRepository; a lógica de CSV fica encapsulada aqui.
    /// </summary>
    public sealed class CsvEventoRepository : IRepository<EventoAcademico, int>
    {
        private const string Header = "Id,Tipo,Descricao,DataHora,DestinatarioEmail,JaNotificado";
        private readonly string _caminhoArquivo;

        public CsvEventoRepository(string caminhoArquivo)
        {
            _caminhoArquivo = caminhoArquivo ?? throw new ArgumentNullException(nameof(caminhoArquivo));
            GarantirArquivoComCabecalho();
        }

        private void GarantirArquivoComCabecalho()
        {
            if (!File.Exists(_caminhoArquivo))
            {
                using var writer = new StreamWriter(_caminhoArquivo, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                writer.WriteLine(Header);
            }
        }

        public EventoAcademico Add(EventoAcademico entity)
        {
            // Política de Id: vem de fora (serviço/cliente define)
            var todos = CarregarTodos();
            if (todos.Any(e => e.Id == entity.Id))
                throw new InvalidOperationException($"Já existe evento com Id {entity.Id}.");

            todos.Add(entity);
            SalvarTodos(todos);
            return entity;
        }

        public EventoAcademico? GetById(int id)
        {
            return CarregarTodos().FirstOrDefault(e => e.Id == id);
        }

        public IReadOnlyList<EventoAcademico> ListAll()
        {
            return CarregarTodos();
        }

        public bool Update(EventoAcademico entity)
        {
            var eventos = CarregarTodos();
            var index = eventos.FindIndex(e => e.Id == entity.Id);
            if (index < 0)
                return false;

            eventos[index] = entity;
            SalvarTodos(eventos);
            return true;
        }

        public bool Remove(int id)
        {
            var eventos = CarregarTodos();
            var removido = eventos.RemoveAll(e => e.Id == id) > 0;
            if (!removido)
                return false;

            SalvarTodos(eventos);
            return true;
        }

        // ----------------- Métodos privados de CSV -----------------

        private List<EventoAcademico> CarregarTodos()
        {
            var linhas = File.ReadAllLines(_caminhoArquivo, Encoding.UTF8).ToList();
            if (linhas.Count <= 1)
                return new List<EventoAcademico>();

            // Ignora cabeçalho
            linhas.RemoveAt(0);

            var eventos = new List<EventoAcademico>();

            foreach (var linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha))
                    continue;

                var colunas = ParseCsvLine(linha);
                if (colunas.Length != 6)
                    continue; // ignora linhas inválidas

                var id = int.Parse(colunas[0], CultureInfo.InvariantCulture);
                var tipo = colunas[1];
                var descricao = colunas[2];
                var dataHora = DateTime.Parse(colunas[3], CultureInfo.InvariantCulture);
                var destinatario = colunas[4];
                var jaNotificado = bool.Parse(colunas[5]);

                eventos.Add(new EventoAcademico(
                    Id: id,
                    Tipo: tipo,
                    Descricao: descricao,
                    DataHora: dataHora,
                    DestinatarioEmail: destinatario,
                    JaNotificado: jaNotificado
                ));
            }

            return eventos;
        }

        private void SalvarTodos(List<EventoAcademico> eventos)
        {
            using var writer = new StreamWriter(_caminhoArquivo, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            writer.WriteLine(Header);
            foreach (var e in eventos.OrderBy(e => e.Id))
            {
                var linha = string.Join(",",
                    e.Id.ToString(CultureInfo.InvariantCulture),
                    Escape(e.Tipo),
                    Escape(e.Descricao),
                    e.DataHora.ToString("O", CultureInfo.InvariantCulture), // ISO 8601 round-trip
                    Escape(e.DestinatarioEmail),
                    e.JaNotificado.ToString(CultureInfo.InvariantCulture)
                );
                writer.WriteLine(linha);
            }
        }

        // ----- Utilidades de CSV: separador ',', aspas → "" e etc. -----

        private static string Escape(string valor)
        {
            if (valor.Contains('"'))
                valor = valor.Replace("\"", "\"\"");

            if (valor.Contains(',') || valor.Contains('"') || valor.Contains('\n'))
                return $"\"{valor}\"";

            return valor;
        }

        private static string[] ParseCsvLine(string linha)
        {
            var resultado = new List<string>();
            var atual = new StringBuilder();
            bool dentroDeAspas = false;

            for (int i = 0; i < linha.Length; i++)
            {
                char c = linha[i];

                if (c == '"' )
                {
                    if (dentroDeAspas && i + 1 < linha.Length && linha[i + 1] == '"')
                    {
                        // Aspas escapadas
                        atual.Append('"');
                        i++; // pula o segundo "
                    }
                    else
                    {
                        // Inverter estado
                        dentroDeAspas = !dentroDeAspas;
                    }
                }
                else if (c == ',' && !dentroDeAspas)
                {
                    resultado.Add(atual.ToString());
                    atual.Clear();
                }
                else
                {
                    atual.Append(c);
                }
            }

            resultado.Add(atual.ToString());
            return resultado.ToArray();
        }
    }
}
