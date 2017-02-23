using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [Flags]
    [ComVisible(true)]
    public enum CompilationRelaxations
    {
        NoStringInterning = 0x0008,
    }

    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    [ComVisible(true)]
    public sealed class DecimalConstantAttribute: Attribute
    {
        [CLSCompliant(false)]
        public DecimalConstantAttribute(
            byte scale,
            byte sign,
            uint hi,
            uint mid,
            uint low
        )
        {
            dec = new decimal((int)low, (int)mid, (int)hi, (sign != 0), scale);
        }

        public DecimalConstantAttribute(
            byte scale,
            byte sign,
            int hi,
            int mid,
            int low
        )
        {
            dec = new decimal(low, mid, hi, (sign != 0), scale);
        }


        public decimal Value
        {
            get
            {
                return dec;
            }
        }

        internal static decimal GetRawDecimalConstant(CustomAttributeData attr)
        {
            Contract.Requires(attr.Constructor.DeclaringType == typeof(DecimalConstantAttribute));

            foreach (CustomAttributeNamedArgument namedArgument in attr.NamedArguments)
            {
                if (namedArgument.MemberInfo.Name.Equals("Value"))
                {
                    // This is not possible because Decimal cannot be represented directly in the metadata.
                    Contract.Assert(false, "Decimal cannot be represented directly in the metadata.");
                    return (decimal)namedArgument.TypedValue.Value;
                }
            }

            ParameterInfo[] parameters = attr.Constructor.GetParameters();
            Contract.Assert(parameters.Length == 5);

            Collections.Generic.IList<CustomAttributeTypedArgument> args = attr.ConstructorArguments;
            Contract.Assert(args.Count == 5);

            if (parameters[2].ParameterType == typeof(uint))
            {
                // DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
                int low = (int)(uint)args[4].Value;
                int mid = (int)(uint)args[3].Value;
                int hi = (int)(uint)args[2].Value;
                byte sign = (byte)args[1].Value;
                byte scale = (byte)args[0].Value;

                return new decimal(low, mid, hi, (sign != 0), scale);
            }
            else
            {
                // DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
                int low = (int)args[4].Value;
                int mid = (int)args[3].Value;
                int hi = (int)args[2].Value;
                byte sign = (byte)args[1].Value;
                byte scale = (byte)args[0].Value;

                return new decimal(low, mid, hi, (sign != 0), scale);
            }
        }

        private decimal dec;
    }

    public static class IsBoxed
    {
    }

    public static class IsByValue
    {
    }

    public static class IsConst
    {
    }

    [ComVisible(true)]
    public static class IsCopyConstructed
    {
    }

    public static class IsExplicitlyDereferenced
    {
    }

    public static class IsImplicitlyDereferenced
    {
    }

    public static class IsJitIntrinsic
    {
    }

    public static class IsLong
    {
    }

    public static class IsPinned
    {
    }

    public static class IsSignUnspecifiedByte
    {
    }

    public static class IsUdtReturn
    {
    }

    [ComVisible(true)]
    public static class IsVolatile
    {
    }

    [Serializable]
    public sealed class RuntimeWrappedException: Exception
    {
        private RuntimeWrappedException(object thrownObject)
            : base(Environment.GetResourceString("RuntimeWrappedException"))
        {
            SetErrorCode(__HResults.COR_E_RUNTIMEWRAPPED);
            m_wrappedException = thrownObject;
        }

        public object WrappedException
        {
            get
            {
                return m_wrappedException;
            }
        }

        private object m_wrappedException;

        [SecurityCritical] // auto-generated_required
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            Contract.EndContractBlock();
            base.GetObjectData(info, context);
            info.AddValue("WrappedException", m_wrappedException, typeof(object));
        }

        internal RuntimeWrappedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_wrappedException = info.GetValue("WrappedException", typeof(object));
        }
    }

    [Serializable]
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class ScopelessEnumAttribute: Attribute
    {
        public ScopelessEnumAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method |
                    AttributeTargets.Property |
                    AttributeTargets.Field |
                    AttributeTargets.Event |
                    AttributeTargets.Struct)]


    public sealed class SpecialNameAttribute: Attribute
    {
        public SpecialNameAttribute()
        {
        }
    }

    [Serializable, AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class StateMachineAttribute: Attribute
    {
        public Type StateMachineType { get; private set; }

        public StateMachineAttribute(Type stateMachineType)
        {
            StateMachineType = stateMachineType;
        }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module)]
    public sealed class SuppressIldasmAttribute: Attribute
    {
        public SuppressIldasmAttribute()
        {
        }
    }

    [HostProtection(Synchronization = true, ExternalThreading = true)]
    public struct TaskAwaiter: ICriticalNotifyCompletion
    {
        /// <summary>The task being awaited.</summary>
        private readonly Task m_task;

        /// <summary>Initializes the <see cref="TaskAwaiter"/>.</summary>
        /// <param name="task">The <see cref="System.Threading.Tasks.Task"/> to be awaited.</param>
        internal TaskAwaiter(Task task)
        {
            Contract.Requires(task != null, "Constructing an awaiter requires a task to await.");
            m_task = task;
        }

        /// <summary>Gets whether the task being awaited is completed.</summary>
        /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        public bool IsCompleted
        {
            get
            {
                return m_task.IsCompleted;
            }
        }

        /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="System.InvalidOperationException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        [SecuritySafeCritical]
        public void OnCompleted(Action continuation)
        {
            OnCompletedInternal(m_task, continuation, continueOnCapturedContext: true, flowExecutionContext: true);
        }

        /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="System.InvalidOperationException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        [SecurityCritical]
        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompletedInternal(m_task, continuation, continueOnCapturedContext: true, flowExecutionContext: false);
        }

        /// <summary>Ends the await on the completed <see cref="System.Threading.Tasks.Task"/>.</summary>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
        /// <exception cref="System.Exception">The task completed in a Faulted state.</exception>
        public void GetResult()
        {
            ValidateEnd(m_task);
        }

        /// <summary>
        /// Fast checks for the end of an await operation to determine whether more needs to be done
        /// prior to completing the await.
        /// </summary>
        /// <param name="task">The awaited task.</param>
        internal static void ValidateEnd(Task task)
        {
            // Fast checks that can be inlined.
            if (task.IsWaitNotificationEnabledOrNotRanToCompletion)
            {
                // If either the end await bit is set or we're not completed successfully,
                // fall back to the slower path.
                HandleNonSuccessAndDebuggerNotification(task);
            }
        }

        /// <summary>
        /// Ensures the task is completed, triggers any necessary debugger breakpoints for completing 
        /// the await on the task, and throws an exception if the task did not complete successfully.
        /// </summary>
        /// <param name="task">The awaited task.</param>
        private static void HandleNonSuccessAndDebuggerNotification(Task task)
        {
            // NOTE: The JIT refuses to inline ValidateEnd when it contains the contents
            // of HandleNonSuccessAndDebuggerNotification, hence the separation.

            // Synchronously wait for the task to complete.  When used by the compiler,
            // the task will already be complete.  This code exists only for direct GetResult use,
            // for cases where the same exception propagation semantics used by "await" are desired,
            // but where for one reason or another synchronous rather than asynchronous waiting is needed.
            if (!task.IsCompleted)
            {
                bool taskCompleted = task.InternalWait(Timeout.Infinite, default(CancellationToken));
                Contract.Assert(taskCompleted, "With an infinite timeout, the task should have always completed.");
            }

            // Now that we're done, alert the debugger if so requested
            task.NotifyDebuggerOfWaitCompletionIfNecessary();

            // And throw an exception if the task is faulted or canceled.
            if (!task.IsRanToCompletion)
                ThrowForNonSuccess(task);
        }

        /// <summary>Throws an exception to handle a task that completed in a state other than RanToCompletion.</summary>
        private static void ThrowForNonSuccess(Task task)
        {
            Contract.Requires(task.IsCompleted, "Task must have been completed by now.");
            Contract.Requires(task.Status != TaskStatus.RanToCompletion, "Task should not be completed successfully.");

            // Handle whether the task has been canceled or faulted
            switch (task.Status)
            {
                // If the task completed in a canceled state, throw an OperationCanceledException.
                // This will either be the OCE that actually caused the task to cancel, or it will be a new
                // TaskCanceledException. TCE derives from OCE, and by throwing it we automatically pick up the
                // completed task's CancellationToken if it has one, including that CT in the OCE.
                case TaskStatus.Canceled:
                    var oceEdi = task.GetCancellationExceptionDispatchInfo();
                    if (oceEdi != null)
                    {
                        oceEdi.Throw();
                        Contract.Assert(false, "Throw() should have thrown");
                    }
                    throw new TaskCanceledException(task);

                // If the task faulted, throw its first exception,
                // even if it contained more than one.
                case TaskStatus.Faulted:
                    var edis = task.GetExceptionDispatchInfos();
                    if (edis.Count > 0)
                    {
                        edis[0].Throw();
                        Contract.Assert(false, "Throw() should have thrown");
                        break; // Necessary to compile: non-reachable, but compiler can't determine that
                    }
                    else
                    {
                        Contract.Assert(false, "There should be exceptions if we're Faulted.");
                        throw task.Exception;
                    }
            }
        }

        /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
        /// <param name="task">The task being awaited.</param>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <param name="continueOnCapturedContext">Whether to capture and marshal back to the current context.</param>
        /// <param name="flowExecutionContext">Whether to flow ExecutionContext across the await.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var have to be marked non-inlineable         
        [SecurityCritical]
        internal static void OnCompletedInternal(Task task, Action continuation, bool continueOnCapturedContext, bool flowExecutionContext)
        {
            if (continuation == null)
                throw new ArgumentNullException("continuation");
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;

            // If TaskWait* ETW events are enabled, trace a beginning event for this await
            // and set up an ending event to be traced when the asynchronous await completes.
            if (TplEtwProvider.Log.IsEnabled() || Task.s_asyncDebuggingEnabled)
            {
                continuation = OutputWaitEtwEvents(task, continuation);
            }

            // Set the continuation onto the awaited task.
            task.SetContinuationForAwait(continuation, continueOnCapturedContext, flowExecutionContext, ref stackMark);
        }

        /// <summary>
        /// Outputs a WaitBegin ETW event, and augments the continuation action to output a WaitEnd ETW event.
        /// </summary>
        /// <param name="task">The task being awaited.</param>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <returns>The action to use as the actual continuation.</returns>
        private static Action OutputWaitEtwEvents(Task task, Action continuation)
        {
            Contract.Requires(task != null, "Need a task to wait on");
            Contract.Requires(continuation != null, "Need a continuation to invoke when the wait completes");

            if (Task.s_asyncDebuggingEnabled)
            {
                Task.AddToActiveTasks(task);
            }

            var etwLog = TplEtwProvider.Log;

            if (etwLog.IsEnabled())
            {
                // ETW event for Task Wait Begin
                var currentTaskAtBegin = Task.InternalCurrent;

                // If this task's continuation is another task, get it.
                var continuationTask = AsyncMethodBuilderCore.TryGetContinuationTask(continuation);
                etwLog.TaskWaitBegin(
                    (currentTaskAtBegin != null ? currentTaskAtBegin.m_taskScheduler.Id : TaskScheduler.Default.Id),
                    (currentTaskAtBegin != null ? currentTaskAtBegin.Id : 0),
                    task.Id, TplEtwProvider.TaskWaitBehavior.Asynchronous,
                    (continuationTask != null ? continuationTask.Id : 0), System.Threading.Thread.GetDomainID());
            }

            // Create a continuation action that outputs the end event and then invokes the user
            // provided delegate.  This incurs the allocations for the closure/delegate, but only if the event
            // is enabled, and in doing so it allows us to pass the awaited task's information into the end event
            // in a purely pay-for-play manner (the alternatively would be to increase the size of TaskAwaiter
            // just for this ETW purpose, not pay-for-play, since GetResult would need to know whether a real yield occurred).
            return AsyncMethodBuilderCore.CreateContinuationWrapper(continuation, () =>
            {
                if (Task.s_asyncDebuggingEnabled)
                {
                    Task.RemoveFromActiveTasks(task.Id);
                }

                // ETW event for Task Wait End.
                Guid prevActivityId = new Guid();
                bool bEtwLogEnabled = etwLog.IsEnabled();
                if (bEtwLogEnabled)
                {
                    var currentTaskAtEnd = Task.InternalCurrent;
                    etwLog.TaskWaitEnd(
                        (currentTaskAtEnd != null ? currentTaskAtEnd.m_taskScheduler.Id : TaskScheduler.Default.Id),
                        (currentTaskAtEnd != null ? currentTaskAtEnd.Id : 0),
                        task.Id);

                    // Ensure the continuation runs under the activity ID of the task that completed for the
                    // case the antecendent is a promise (in the other cases this is already the case).
                    if (etwLog.TasksSetActivityIds && (task.Options & (TaskCreationOptions)InternalTaskOptions.PromiseTask) != 0)
                        EventSource.SetCurrentThreadActivityId(TplEtwProvider.CreateGuidForTaskID(task.Id), out prevActivityId);
                }
                // Invoke the original continuation provided to OnCompleted.
                continuation();

                if (bEtwLogEnabled)
                {
                    etwLog.TaskWaitContinuationComplete(task.Id);
                    if (etwLog.TasksSetActivityIds && (task.Options & (TaskCreationOptions)InternalTaskOptions.PromiseTask) != 0)
                        EventSource.SetCurrentThreadActivityId(prevActivityId);
                }
            });
        }
    }

    /// <summary>Provides an awaiter for awaiting a <see cref="System.Threading.Tasks.Task{TResult}"/>.</summary>
    /// <remarks>This type is intended for compiler use only.</remarks>
    [HostProtection(Synchronization = true, ExternalThreading = true)]
    public struct TaskAwaiter<TResult>: ICriticalNotifyCompletion
    {
        /// <summary>The task being awaited.</summary>
        private readonly Task<TResult> m_task;

        /// <summary>Initializes the <see cref="TaskAwaiter{TResult}"/>.</summary>
        /// <param name="task">The <see cref="System.Threading.Tasks.Task{TResult}"/> to be awaited.</param>
        internal TaskAwaiter(Task<TResult> task)
        {
            Contract.Requires(task != null, "Constructing an awaiter requires a task to await.");
            m_task = task;
        }

        /// <summary>Gets whether the task being awaited is completed.</summary>
        /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        public bool IsCompleted
        {
            get
            {
                return m_task.IsCompleted;
            }
        }

        /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        [SecuritySafeCritical]
        public void OnCompleted(Action continuation)
        {
            TaskAwaiter.OnCompletedInternal(m_task, continuation, continueOnCapturedContext: true, flowExecutionContext: true);
        }

        /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        [SecurityCritical]
        public void UnsafeOnCompleted(Action continuation)
        {
            TaskAwaiter.OnCompletedInternal(m_task, continuation, continueOnCapturedContext: true, flowExecutionContext: false);
        }

        /// <summary>Ends the await on the completed <see cref="System.Threading.Tasks.Task{TResult}"/>.</summary>
        /// <returns>The result of the completed <see cref="System.Threading.Tasks.Task{TResult}"/>.</returns>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <exception cref="System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
        /// <exception cref="System.Exception">The task completed in a Faulted state.</exception>
        public TResult GetResult()
        {
            TaskAwaiter.ValidateEnd(m_task);
            return m_task.ResultOnSuccess;
        }
    }

    /// <summary>Provides an awaitable object that allows for configured awaits on <see cref="System.Threading.Tasks.Task"/>.</summary>
    /// <remarks>This type is intended for compiler use only.</remarks>
    public struct ConfiguredTaskAwaitable
    {
        /// <summary>The task being awaited.</summary>
        private readonly ConfiguredTaskAwaiter m_configuredTaskAwaiter;

        /// <summary>Initializes the <see cref="ConfiguredTaskAwaitable"/>.</summary>
        /// <param name="task">The awaitable <see cref="System.Threading.Tasks.Task"/>.</param>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
        /// </param>
        internal ConfiguredTaskAwaitable(Task task, bool continueOnCapturedContext)
        {
            Contract.Requires(task != null, "Constructing an awaitable requires a task to await.");
            m_configuredTaskAwaiter = new ConfiguredTaskAwaiter(task, continueOnCapturedContext);
        }

        /// <summary>Gets an awaiter for this awaitable.</summary>
        /// <returns>The awaiter.</returns>
        public ConfiguredTaskAwaiter GetAwaiter()
        {
            return m_configuredTaskAwaiter;
        }

        /// <summary>Provides an awaiter for a <see cref="ConfiguredTaskAwaitable"/>.</summary>
        /// <remarks>This type is intended for compiler use only.</remarks>
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        public struct ConfiguredTaskAwaiter: ICriticalNotifyCompletion
        {
            /// <summary>The task being awaited.</summary>
            private readonly Task m_task;

            /// <summary>Whether to attempt marshaling back to the original context.</summary>
            private readonly bool m_continueOnCapturedContext;

            /// <summary>Initializes the <see cref="ConfiguredTaskAwaiter"/>.</summary>
            /// <param name="task">The <see cref="System.Threading.Tasks.Task"/> to await.</param>
            /// <param name="continueOnCapturedContext">
            /// true to attempt to marshal the continuation back to the original context captured
            /// when BeginAwait is called; otherwise, false.
            /// </param>
            internal ConfiguredTaskAwaiter(Task task, bool continueOnCapturedContext)
            {
                Contract.Requires(task != null, "Constructing an awaiter requires a task to await.");
                m_task = task;
                m_continueOnCapturedContext = continueOnCapturedContext;
            }

            /// <summary>Gets whether the task being awaited is completed.</summary>
            /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            public bool IsCompleted
            {
                get
                {
                    return m_task.IsCompleted;
                }
            }

            /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
            /// <param name="continuation">The action to invoke when the await operation completes.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
            [SecuritySafeCritical]
            public void OnCompleted(Action continuation)
            {
                TaskAwaiter.OnCompletedInternal(m_task, continuation, m_continueOnCapturedContext, flowExecutionContext: true);
            }

            /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
            /// <param name="continuation">The action to invoke when the await operation completes.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
            [SecurityCritical]
            public void UnsafeOnCompleted(Action continuation)
            {
                TaskAwaiter.OnCompletedInternal(m_task, continuation, m_continueOnCapturedContext, flowExecutionContext: false);
            }

            /// <summary>Ends the await on the completed <see cref="System.Threading.Tasks.Task"/>.</summary>
            /// <returns>The result of the completed <see cref="System.Threading.Tasks.Task{TResult}"/>.</returns>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            /// <exception cref="System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
            /// <exception cref="System.Exception">The task completed in a Faulted state.</exception>
            public void GetResult()
            {
                TaskAwaiter.ValidateEnd(m_task);
            }
        }
    }

    /// <summary>Provides an awaitable object that allows for configured awaits on <see cref="System.Threading.Tasks.Task{TResult}"/>.</summary>
    /// <remarks>This type is intended for compiler use only.</remarks>
    public struct ConfiguredTaskAwaitable<TResult>
    {
        /// <summary>The underlying awaitable on whose logic this awaitable relies.</summary>
        private readonly ConfiguredTaskAwaiter m_configuredTaskAwaiter;

        /// <summary>Initializes the <see cref="ConfiguredTaskAwaitable{TResult}"/>.</summary>
        /// <param name="task">The awaitable <see cref="System.Threading.Tasks.Task{TResult}"/>.</param>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
        /// </param>
        internal ConfiguredTaskAwaitable(Task<TResult> task, bool continueOnCapturedContext)
        {
            m_configuredTaskAwaiter = new ConfiguredTaskAwaiter(task, continueOnCapturedContext);
        }

        /// <summary>Gets an awaiter for this awaitable.</summary>
        /// <returns>The awaiter.</returns>
        public ConfiguredTaskAwaiter GetAwaiter()
        {
            return m_configuredTaskAwaiter;
        }

        /// <summary>Provides an awaiter for a <see cref="ConfiguredTaskAwaitable{TResult}"/>.</summary>
        /// <remarks>This type is intended for compiler use only.</remarks>
        [HostProtection(Synchronization = true, ExternalThreading = true)]
        public struct ConfiguredTaskAwaiter: ICriticalNotifyCompletion
        {
            /// <summary>The task being awaited.</summary>
            private readonly Task<TResult> m_task;

            /// <summary>Whether to attempt marshaling back to the original context.</summary>
            private readonly bool m_continueOnCapturedContext;

            /// <summary>Initializes the <see cref="ConfiguredTaskAwaiter"/>.</summary>
            /// <param name="task">The awaitable <see cref="System.Threading.Tasks.Task{TResult}"/>.</param>
            /// <param name="continueOnCapturedContext">
            /// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
            /// </param>
            internal ConfiguredTaskAwaiter(Task<TResult> task, bool continueOnCapturedContext)
            {
                Contract.Requires(task != null, "Constructing an awaiter requires a task to await.");
                m_task = task;
                m_continueOnCapturedContext = continueOnCapturedContext;
            }

            /// <summary>Gets whether the task being awaited is completed.</summary>
            /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            public bool IsCompleted
            {
                get
                {
                    return m_task.IsCompleted;
                }
            }

            /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
            /// <param name="continuation">The action to invoke when the await operation completes.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
            [SecuritySafeCritical]
            public void OnCompleted(Action continuation)
            {
                TaskAwaiter.OnCompletedInternal(m_task, continuation, m_continueOnCapturedContext, flowExecutionContext: true);
            }

            /// <summary>Schedules the continuation onto the <see cref="System.Threading.Tasks.Task"/> associated with this <see cref="TaskAwaiter"/>.</summary>
            /// <param name="continuation">The action to invoke when the await operation completes.</param>
            /// <exception cref="System.ArgumentNullException">The <paramref name="continuation"/> argument is null (Nothing in Visual Basic).</exception>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
            [SecurityCritical]
            public void UnsafeOnCompleted(Action continuation)
            {
                TaskAwaiter.OnCompletedInternal(m_task, continuation, m_continueOnCapturedContext, flowExecutionContext: false);
            }

            /// <summary>Ends the await on the completed <see cref="System.Threading.Tasks.Task{TResult}"/>.</summary>
            /// <returns>The result of the completed <see cref="System.Threading.Tasks.Task{TResult}"/>.</returns>
            /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
            /// <exception cref="System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
            /// <exception cref="System.Exception">The task completed in a Faulted state.</exception>
            public TResult GetResult()
            {
                TaskAwaiter.ValidateEnd(m_task);
                return m_task.ResultOnSuccess;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate,
        Inherited = false, AllowMultiple = false)]
    public sealed class TypeForwardedFromAttribute: Attribute
    {
        string assemblyFullName;

        private TypeForwardedFromAttribute()
        {
            // Disallow default constructor
        }


        public TypeForwardedFromAttribute(string assemblyFullName)
        {
            if (string.IsNullOrEmpty(assemblyFullName))
            {
                throw new ArgumentNullException("assemblyFullName");
            }
            this.assemblyFullName = assemblyFullName;
        }

        public string AssemblyFullName
        {
            get
            {
                return assemblyFullName;
            }
        }
    }
    using

    System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class TypeForwardedToAttribute: Attribute
    {
        private Type _destination;

        public TypeForwardedToAttribute(Type destination)
        {
            _destination = destination;
        }

        public Type Destination
        {
            get
            {
                return _destination;
            }
        }

        [SecurityCritical]
        internal static TypeForwardedToAttribute[] GetCustomAttribute(RuntimeAssembly assembly)
        {
            Type[] types = null;
            RuntimeAssembly.GetForwardedTypes(assembly.GetNativeHandle(), JitHelpers.GetObjectHandleOnStack(ref types));

            TypeForwardedToAttribute[] attributes = new TypeForwardedToAttribute[types.Length];
            for (int i = 0; i < types.Length; ++i)
                attributes[i] = new TypeForwardedToAttribute(types[i]);

            return attributes;
        }

    }

    [Serializable]
    [AttributeUsage(AttributeTargets.Struct)]
    sealed public class UnsafeValueTypeAttribute: Attribute
    {
    }

    public struct YieldAwaitable
    {
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