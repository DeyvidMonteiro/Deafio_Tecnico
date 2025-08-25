namespace DesafioTecnicoAvanade.EstoqueApi.Filters.Exceptions
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string message) : base(message)
        {
        }
    }
}
