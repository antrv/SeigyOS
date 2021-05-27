using System;
using System.Runtime.CompilerServices;

namespace SeigyOS
{
    [Serializable]
    public readonly struct NativeSize: IEquatable<NativeSize>, IComparable<NativeSize>
    {
        private readonly IntPtr _ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NativeSize(IntPtr ptr) => _ptr = ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe NativeSize operator +(NativeSize a, NativeSize b) =>
            sizeof(void*) == 4 ? (uint)a._ptr + (uint)b._ptr : (ulong)a._ptr + (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe NativeSize operator -(NativeSize a, NativeSize b) =>
            sizeof(void*) == 4 ? (uint)a._ptr - (uint)b._ptr : (ulong)a._ptr - (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe NativeSize operator *(NativeSize a, NativeSize b) =>
            sizeof(void*) == 4 ? (uint)a._ptr * (uint)b._ptr : (ulong)a._ptr * (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe NativeSize operator /(NativeSize a, NativeSize b) =>
            sizeof(void*) == 4 ? (uint)a._ptr / (uint)b._ptr : (ulong)a._ptr / (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe NativeSize operator %(NativeSize a, NativeSize b) =>
            sizeof(void*) == 4 ? (uint)a._ptr % (uint)b._ptr : (ulong)a._ptr % (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NativeSize(uint value) => new NativeSize((IntPtr)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NativeSize(ulong value) => new NativeSize((IntPtr)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NativeSize(int value) => new NativeSize((IntPtr)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NativeSize(long value) => new NativeSize((IntPtr)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NativeSize(IntPtr value) => new NativeSize(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IntPtr(NativeSize value) => value._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(NativeSize a, NativeSize b) => a._ptr == b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(NativeSize a, NativeSize b) => a._ptr != b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool operator >(NativeSize a, NativeSize b) =>
            sizeof(IntPtr) == 4 ? (uint)a._ptr > (uint)b._ptr : (ulong)a._ptr > (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool operator <(NativeSize a, NativeSize b) =>
            sizeof(IntPtr) == 4 ? (uint)a._ptr < (uint)b._ptr : (ulong)a._ptr < (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool operator >=(NativeSize a, NativeSize b) =>
            sizeof(IntPtr) == 4 ? (uint)a._ptr >= (uint)b._ptr : (ulong)a._ptr >= (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool operator <=(NativeSize a, NativeSize b) =>
            sizeof(IntPtr) == 4 ? (uint)a._ptr <= (uint)b._ptr : (ulong)a._ptr <= (ulong)b._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(NativeSize other) => _ptr == other._ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int CompareTo(NativeSize other) =>
            sizeof(IntPtr) == 4 ? ((uint)_ptr).CompareTo((uint)other._ptr) : ((ulong)_ptr).CompareTo((ulong)other._ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => _ptr.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) =>
            obj is NativeSize other && _ptr == other._ptr;
    }
}