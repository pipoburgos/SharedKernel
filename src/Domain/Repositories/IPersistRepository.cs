namespace SharedKernel.Domain.Repositories
{
    internal interface IPersistRepository
    {
        int Rollback();

        int SaveChanges();
    }
}
