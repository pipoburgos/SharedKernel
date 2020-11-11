using System.Threading.Tasks;

namespace SharedKernel.Application.System
{
    public static class TaskHelper
    {
#if NET40 || NET45
        public static Task CompletedTask => new Task(() => { });
#else
        public static Task CompletedTask => Task.CompletedTask;
#endif
    }
}
