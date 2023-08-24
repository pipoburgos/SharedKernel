namespace SharedKernel.Testing.Acceptance.Exceptions
{
    public class SaveChangesExceptionHandler
    {
        public T SaveChanges<T>(Func<T> guardar)
        {
            try
            {
                return guardar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                throw;
            }
        }
    }
}
