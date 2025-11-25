namespace Fase8Isp.Contratos
{
    // Interface de Composição para quem precisa de Leitura E Escrita
    public interface IRepository<T, TId> : 
        IReadRepository<T, TId>, 
        IWriteRepository<T, TId> 
        where T : notnull
    { }
}