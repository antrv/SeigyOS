using System.Diagnostics.Contracts;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.CompilerServices
{
    public struct YieldAwaitable
    {
        // TODO !!!

        /// <summary>Gets an awaiter for this <see cref="YieldAwaitable"/>.</summary>
        /// <returns>An awaiter for this awaitable.</returns>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        public YieldAwaiter GetAwaiter()
        {
            return new YieldAwaiter();
        }

        /// <summary>Provides an awaiter that switches into a target environment.</summary>
        /// <remarks>This type is intended for compiler use only.</remarks>
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        public struct YieldAwaiter: ICriticalNotifyCompletion
        {
            /// <summary>Gets whether a yield is not required.</summary>
            /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
            public bool IsCompleted
            {
                get
                {
                    return false;
                }
            } // yielding is always required for YieldAwaiter, hence false

            /// <summary>Posts the <paramref name="continuation"/> back to the current context.</summary>
            /// <param name="continuation">The action to invoke asynchronously.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            [SecuritySafeCritical]
            public void OnCompleted(Action continuation)
            {
                QueueContinuation(continuation, flowContext: true);
            }

            /// <summary>Posts the <paramref name="continuation"/> back to the current context.</summary>
            /// <param name="continuation">The action to invoke asynchronously.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            [SecurityCritical]
            public void UnsafeOnCompleted(Action continuation)
            {
                QueueContinuation(continuation, flowContext: false);
            }

            /// <summary>Posts the <paramref name="continuation"/> back to the current context.</summary>
            /// <param name="continuation">The action to invoke asynchronously.</param>
            /// <param name="flowContext">true to flow ExecutionContext; false if flowing is not required.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            [SecurityCritical]
            private static void QueueContinuation(Action continuation, bool flowContext)
            {
                // Validate arguments
                if (continuation == null)
                    throw new ArgumentNullException("continuation");
                Contract.EndContractBlock();

                if (TplEtwProvider.Log.IsEnabled())
                {
                    continuation = OutputCorrelationEtwEvent(continuation);
                }
                // Get the current SynchronizationContext, and if there is one,
                // post the continuation to it.  However, treat the base type
                // as if there wasn't a SynchronizationContext, since that's what it
                // logically represents.
                var syncCtx = SynchronizationContext.CurrentNoFlow;
                if (syncCtx != null && syncCtx.GetType() != typeof(SynchronizationContext))
                {
                    syncCtx.Post(s_sendOrPostCallbackRunAction, continuation);
                }
                else
                {
                    // If we're targeting the default scheduler, queue to the thread pool, so that we go into the global
                    // queue.  As we're going into the global queue, we might as well use QUWI, which for the global queue is
                    // just a tad faster than task, due to a smaller object getting allocated and less work on the execution path.
                    TaskScheduler scheduler = TaskScheduler.Current;
                    if (scheduler == TaskScheduler.Default)
                    {
                        if (flowContext)
                        {
                            ThreadPool.QueueUserWorkItem(s_waitCallbackRunAction, continuation);
                        }
                        else
                        {
                            ThreadPool.UnsafeQueueUserWorkItem(s_waitCallbackRunAction, continuation);
                        }
                    }
                    // We're targeting a custom scheduler, so queue a task.
                    else
                    {
                        Task.Factory.StartNew(continuation, default(CancellationToken), TaskCreationOptions.PreferFairness, scheduler);
                    }
                }
            }

            private static Action OutputCorrelationEtwEvent(Action continuation)
            {
                int continuationId = Task.NewId();
                Task currentTask = Task.InternalCurrent;
                // fire the correlation ETW event
                TplEtwProvider.Log.AwaitTaskContinuationScheduled(TaskScheduler.Current.Id, (currentTask != null) ? currentTask.Id : 0, continuationId);

                return AsyncMethodBuilderCore.CreateContinuationWrapper(continuation, () =>
                {
                    var etwLog = TplEtwProvider.Log;
                    etwLog.TaskWaitContinuationStarted(continuationId);

                    // ETW event for Task Wait End.
                    Guid prevActivityId = new Guid();
                    // Ensure the continuation runs under the correlated activity ID generated above
                    if (etwLog.TasksSetActivityIds)
                        EventSource.SetCurrentThreadActivityId(TplEtwProvider.CreateGuidForTaskID(continuationId), out prevActivityId);

                    // Invoke the original continuation provided to OnCompleted.
                    continuation();
                    // Restore activity ID

                    if (etwLog.TasksSetActivityIds)
                        EventSource.SetCurrentThreadActivityId(prevActivityId);

                    etwLog.TaskWaitContinuationComplete(continuationId);
                });

            }

            /// <summary>WaitCallback that invokes the Action supplied as object state.</summary>
            private static readonly WaitCallback s_waitCallbackRunAction = RunAction;

            /// <summary>SendOrPostCallback that invokes the Action supplied as object state.</summary>
            private static readonly SendOrPostCallback s_sendOrPostCallbackRunAction = RunAction;

            /// <summary>Runs an Action delegate provided as state.</summary>
            /// <param name="state">The Action delegate to invoke.</param>
            private static void RunAction(object state)
            {
                ((Action)state)();
            }

            /// <summary>Ends the await operation.</summary>
            public void GetResult()
            {
            } // Nop. It exists purely because the compiler pattern demands it.
        }
    }
}