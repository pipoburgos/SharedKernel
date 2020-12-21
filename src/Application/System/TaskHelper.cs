using System.Threading.Tasks;

namespace SharedKernel.Application.System
{
    /// <summary>Gets a task that has already completed successfully.</summary>
    /// <returns>The successfully completed task.</returns>
    public static class TaskHelper
    {
#if NET40 || NET45
        /// <summary>Gets a task that has already completed successfully.</summary>
    /// <returns>The successfully completed task.</returns>
        public static Task CompletedTask => new Task(() => { });
#else

        /// <summary>Gets a task that has already completed successfully.</summary>
        /// <returns>The successfully completed task.</returns>
        public static Task CompletedTask => Task.CompletedTask;
#endif
    }
}
