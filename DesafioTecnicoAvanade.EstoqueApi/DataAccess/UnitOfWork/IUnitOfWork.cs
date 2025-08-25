namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    public Task Commit();
    public Task Rollback();
}
