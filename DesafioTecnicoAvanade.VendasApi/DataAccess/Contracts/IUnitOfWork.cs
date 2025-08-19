namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        public Task Commit();
    }
}
