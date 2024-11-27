namespace SharedKernel.Integration.Tests.System.Threading.PostgreSql;


public class PostgreSqlMutexTests : CommonMutexTests<PostgreSqlApp>
{
    public PostgreSqlMutexTests(PostgreSqlApp app1Mutex, PostgreSqlApp app2Mutex) : base(app1Mutex, app2Mutex)
    {
    }
}
