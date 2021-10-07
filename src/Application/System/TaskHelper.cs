#if !NET40 && !NET45
using System;
using System.Threading;
#endif
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

        /// <summary> Create a task that will be completed when the time comes. </summary>
        /// <returns></returns>
        public static Task DelayToUtcHour(int hour, int minute, int second, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var next = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
            var result = next - now;
            return Task.Delay(result, cancellationToken);
        }

        /// <summary> Create a task that will be completed when the time comes. </summary>
        /// <returns></returns>
        public static Task DelayToHour(int hour, int minute, int second, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var next = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
            var result = next - now;
            return Task.Delay(result, cancellationToken);
        }

        private static readonly TaskFactory MyTaskFactory = new TaskFactory(CancellationToken.None,
            TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Ejecutar una función asíncrona de forma síncrona
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Ejecutar una función asíncrona de forma síncrona
        /// </summary>
        /// <param name="func"></param>
        public static void RunSync(Func<Task> func)
        {
            MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
#endif

    }
}
