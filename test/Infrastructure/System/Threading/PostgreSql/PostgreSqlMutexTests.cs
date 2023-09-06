using Xunit;

namespace SharedKernel.Integration.Tests.System.Threading.PostgreSql;

[Collection("DockerHook")]
public class PostgreSqlMutexTests : CommonMutexTests<PostgreSqlApp>
{
    public PostgreSqlMutexTests(PostgreSqlApp app1Mutex, PostgreSqlApp app2Mutex) : base(app1Mutex, app2Mutex)
    {
    }
}
