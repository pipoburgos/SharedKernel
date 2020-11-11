namespace SharedKernel.Application.UnitOfWorks
{
    public interface IUnitOfWork
    {
        int Rollback();

        int SaveChanges();
    }
}