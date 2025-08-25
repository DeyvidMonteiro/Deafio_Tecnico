namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
        Task BeginTransactionAsync();
        Task RollbackTransactionAsync();
        Task CommitTransactionAsync();
        void Dispose();
    }
}
