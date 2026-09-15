namespace SharedKernel.Integration.Tests.System.Threading.PostgreSql;


public class PostgreSqlMutexTests : CommonMutexTests<PostgreSqlApp>
{
    public PostgreSqlMutexTests(TwoAppFixture<PostgreSqlApp> fixture) : base(fixture)
    {
    }
}
