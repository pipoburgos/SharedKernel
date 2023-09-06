using Xunit;

namespace SharedKernel.Integration.Tests.System.Threading.Mongo;

[Collection("DockerHook")]
public class MongoMutexTests : CommonMutexTests<MongoApp>
{
    public MongoMutexTests(MongoApp app1Mutex, MongoApp app2Mutex) : base(app1Mutex, app2Mutex)
    {
    }
}
