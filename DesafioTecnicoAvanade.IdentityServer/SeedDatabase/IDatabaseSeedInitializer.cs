namespace DesafioTecnicoAvanade.IdentityServer.SeedDatabase
{
    public interface IDatabaseSeedInitializer
    {
        void InitializerSeedRoles();
        void InitializerSeedUser();
    }
}
