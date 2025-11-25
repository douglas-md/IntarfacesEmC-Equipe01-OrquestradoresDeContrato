using System.Collections.Generic;

namespace Fase8Isp.Contratos
{
    public interface IReadRepository<out T, in TId> where T : notnull
    {
        T? GetById(TId id);
        IReadOnlyList<T> ListAll();
    }
}