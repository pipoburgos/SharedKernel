namespace SharedKernel.Integration.Tests.System.Threading.SqlServer;


public class SqlServerMutexTests : CommonMutexTests<SqlServerApp>
{
    public SqlServerMutexTests(SqlServerApp app1Mutex, SqlServerApp app2Mutex) : base(app1Mutex, app2Mutex)
    {
    }
}
