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
            return Task.Delay(DelayCommon(DateTime.UtcNow, hour, minute, second), cancellationToken);
        }

        /// <summary> Create a task that will be completed when the time comes. </summary>
        /// <returns></returns>
        public static Task DelayToHour(int hour, int minute, int second, CancellationToken cancellationToken)
        {
            return Task.Delay(DelayCommon(DateTime.Now, hour, minute, second), cancellationToken);
        }

        private static TimeSpan DelayCommon(DateTime dateTime, int hour, int minute = 0, int second = 0)
        {
            var next = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second);

            if (next < dateTime)
                next = next.AddDays(1);

            return next - dateTime;
        }

        private static readonly TaskFactory MyTaskFactory = new TaskFactory(CancellationToken.None,
            TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Execute an asynchronous function synchronously
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Execute an asynchronous function synchronously
        /// </summary>
        public static void RunSync(Func<Task> func)
        {
            MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Wait for a task to finish
        /// </summary>
        public static TResult RunSync<TResult>(Task<TResult> task)
        {
            return MyTaskFactory.StartNew(async () => await task).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Wait for a task to finish
        /// </summary>
        public static void RunSync(Task task)
        {
            MyTaskFactory.StartNew(async () => await task).Unwrap().GetAwaiter().GetResult();
        }
#endif

    }
}
