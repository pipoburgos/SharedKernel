namespace SharedKernel.Integration.Tests.System.Threading.SqlServer;


public class SqlServerMutexTests : CommonMutexTests<SqlServerApp>
{
    public SqlServerMutexTests(TwoAppFixture<SqlServerApp> fixture) : base(fixture)
    {
    }
}
