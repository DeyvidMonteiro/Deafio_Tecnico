namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    Task Commit();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task BeginTransactionAsync();
    void Dispose();

}
