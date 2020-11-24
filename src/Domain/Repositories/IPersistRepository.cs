namespace SharedKernel.Domain.Repositories
{
    public interface IPersistRepository
    {
        int Rollback();

        int SaveChanges();
    }
}
