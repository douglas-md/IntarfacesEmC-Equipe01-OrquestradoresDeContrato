using System.Collections.Generic;

namespace RepositoryEventos.Repositorio
{
    /// <summary>
    /// Contrato genérico para acesso a dados.
    /// Define "o que" fazer, não "como" fazer.
    /// Expõe apenas operações de acesso, sem regra de negócio.
    /// </summary>
    public interface IRepository<T, TId>
    {
        /// <summary>
        /// Adiciona uma entidade ao repositório.
        /// </summary>
        T Add(T entity);

        /// <summary>
        /// Busca uma entidade por ID.
        /// </summary>
        /// <returns>Entidade se encontrada, null caso contrário.</returns>
        T? GetById(TId id);

        /// <summary>
        /// Lista todas as entidades.
        /// Retorna coleção somente leitura (não mutável).
        /// </summary>
        IReadOnlyList<T> ListAll();

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <returns>true se atualizou, false se não encontrou.</returns>
        bool Update(T entity);

        /// <summary>
        /// Remove uma entidade por ID.
        /// </summary>
        /// <returns>true se removeu, false se não encontrou.</returns>
        bool Remove(TId id);
    }
}
