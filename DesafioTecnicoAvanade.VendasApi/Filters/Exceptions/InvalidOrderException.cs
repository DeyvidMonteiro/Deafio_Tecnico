namespace DesafioTecnicoAvanade.VendasApi.Filters.Exceptions
{
    public class InvalidOrderException : Exception
    {
        public InvalidOrderException(string message) : base(message) { }
    }
}
