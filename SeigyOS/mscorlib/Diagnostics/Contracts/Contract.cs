using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace System.Diagnostics.Contracts
{
    public static partial class Contract
    {
        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Assume(bool condition)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assume, null, null, null);
        }

        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Assume(bool condition, string userMessage)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assume, userMessage, null, null);
        }

        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Assert(bool condition)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assert, null, null, null);
        }

        [Pure]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Assert(bool condition, string userMessage)
        {
            if (!condition)
                ReportFailure(ContractFailureKind.Assert, userMessage, null, null);
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Requires(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Requires(bool condition, string userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Requires<TException>(bool condition)
            where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Requires<TException>(bool condition, string userMessage)
            where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Ensures(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Ensures(bool condition, string userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void EnsuresOnThrow<TException>(bool condition)
            where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void EnsuresOnThrow<TException>(bool condition, string userMessage) 
            where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static T Result<T>()
        {
            return default(T);
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static T ValueAtReturn<T>(out T value)
        {
            value = default(T);
            return value;
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static T OldValue<T>(T value)
        {
            return default(T);
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Invariant(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
        }

        [Pure]
        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Invariant(bool condition, string userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static bool ForAll(int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (fromInclusive > toExclusive)
                throw new ArgumentException(__Resources.GetResourceString("Argument_ToExclusiveLessThanFromExclusive"));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            for (int i = fromInclusive; i < toExclusive; i++)
                if (!predicate(i))
                    return false;
            return true;
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            foreach (T t in collection)
                if (!predicate(t))
                    return false;
            return true;
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static bool Exists(int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (fromInclusive > toExclusive)
                throw new ArgumentException(__Resources.GetResourceString("Argument_ToExclusiveLessThanFromExclusive"));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            for (int i = fromInclusive; i < toExclusive; i++)
                if (predicate(i))
                    return true;
            return false;
        }

        [Pure]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Contract.EndContractBlock();

            foreach (T t in collection)
                if (predicate(t)) return true;
            return false;
        }

        [Conditional("CONTRACTS_FULL")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void EndContractBlock()
        {
        }

        static partial void ReportFailure(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException);
        static partial void AssertMustUseRewriter(ContractFailureKind kind, string contractKind);
    }
}