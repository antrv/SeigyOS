using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Security;

namespace System.Text
{
    [ComVisible(true)]
    [Serializable]
    public sealed class StringBuilder: ISerializable
    {
        private char[] _chunkChars;
        private StringBuilder _chunkPrevious;
        private int _chunkLength;
        private int _chunkOffset;
        private readonly int _maxCapacity;

        internal const int DefaultCapacity = 16;
        private const string cCapacityField = "Capacity";
        private const string cMaxCapacityField = "m_MaxCapacity";
        private const string cStringValueField = "m_StringValue";
        private const string cThreadIdField = "m_currentThread";
        private const int cMaxChunkSize = 8000;

        public StringBuilder()
            : this(DefaultCapacity)
        {
        }

        public StringBuilder(int capacity)
            : this(string.Empty, capacity)
        {
        }

        public StringBuilder(string value)
            : this(value, DefaultCapacity)
        {
        }

        public StringBuilder(string value, int capacity)
            : this(value, 0, value != null ? value.Length : 0, capacity)
        {
        }

        [SecuritySafeCritical]
        public StringBuilder(string value, int startIndex, int length, int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_MustBePositive, nameof(capacity)));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_MustBeNonNegNum, nameof(length)));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_StartIndex));
            Contract.EndContractBlock();

            if (value == null)
                value = string.Empty;
            if (startIndex > value.Length - length)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_IndexLength));
            _maxCapacity = int.MaxValue;
            if (capacity == 0)
                capacity = DefaultCapacity;
            if (capacity < length)
                capacity = length;

            _chunkChars = new char[capacity];
            _chunkLength = length;

            unsafe
            {
                fixed (char* sourcePtr = value)
                    ThreadSafeCopy(sourcePtr + startIndex, _chunkChars, 0, length);
            }
        }

        public StringBuilder(int capacity, int maxCapacity)
        {
            if (capacity > maxCapacity)
                throw new ArgumentOutOfRangeException(nameof(capacity), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Capacity));
            if (maxCapacity < 1)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_SmallMaxCapacity));
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_MustBePositive, nameof(capacity)));
            Contract.EndContractBlock();

            if (capacity == 0)
                capacity = Math.Min(DefaultCapacity, maxCapacity);
            this._maxCapacity = maxCapacity;
            _chunkChars = new char[capacity];
        }

        [SecurityCritical]
        private StringBuilder(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();

            int persistedCapacity = 0;
            string persistedString = null;
            int persistedMaxCapacity = int.MaxValue;
            bool capacityPresent = false;

            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case cMaxCapacityField:
                        persistedMaxCapacity = info.GetInt32(cMaxCapacityField);
                        break;
                    case cStringValueField:
                        persistedString = info.GetString(cStringValueField);
                        break;
                    case cCapacityField:
                        persistedCapacity = info.GetInt32(cCapacityField);
                        capacityPresent = true;
                        break;
                }
            }

            if (persistedString == null)
                persistedString = string.Empty;
            if (persistedMaxCapacity < 1 || persistedString.Length > persistedMaxCapacity)
                throw new SerializationException(__Resources.GetResourceString(__Resources.Serialization_StringBuilderMaxCapacity));

            if (!capacityPresent)
            {
                persistedCapacity = DefaultCapacity;
                if (persistedCapacity < persistedString.Length)
                    persistedCapacity = persistedString.Length;
                if (persistedCapacity > persistedMaxCapacity)
                    persistedCapacity = persistedMaxCapacity;
            }
            if (persistedCapacity < 0 || persistedCapacity < persistedString.Length || persistedCapacity > persistedMaxCapacity)
                throw new SerializationException(__Resources.GetResourceString(__Resources.Serialization_StringBuilderCapacity));

            // Assign
            _maxCapacity = persistedMaxCapacity;
            _chunkChars = new char[persistedCapacity];
            persistedString.CopyTo(0, _chunkChars, 0, persistedString.Length);
            _chunkLength = persistedString.Length;
            _chunkPrevious = null;
        }

        [SecurityCritical]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            Contract.EndContractBlock();
            info.AddValue(cMaxCapacityField, _maxCapacity);
            info.AddValue(cCapacityField, Capacity);
            info.AddValue(cStringValueField, ToString());
            info.AddValue(cThreadIdField, 0);
        }

        public int Capacity
        {
            get
            {
                return _chunkChars.Length + _chunkOffset;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NegativeCapacity));
                if (value > MaxCapacity)
                    throw new ArgumentOutOfRangeException(nameof(value), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Capacity));
                if (value < Length)
                    throw new ArgumentOutOfRangeException(nameof(value), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_SmallCapacity));
                Contract.EndContractBlock();

                if (Capacity != value)
                {
                    int newLen = value - _chunkOffset;
                    char[] newArray = new char[newLen];
                    Array.Copy(_chunkChars, newArray, _chunkLength);
                    _chunkChars = newArray;
                }
            }
        }

        public int MaxCapacity => _maxCapacity;

        public int EnsureCapacity(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NegativeCapacity));
            Contract.EndContractBlock();

            if (Capacity < capacity)
                Capacity = capacity;
            return Capacity;
        }

        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (Length == 0)
                return string.Empty;

            string ret = string.FastAllocateString(Length);
            StringBuilder chunk = this;
            unsafe
            {
                fixed (char* destinationPtr = ret)
                {
                    do
                    {
                        if (chunk._chunkLength > 0)
                        {
                            char[] sourceArray = chunk._chunkChars;
                            int chunkOffset = chunk._chunkOffset;
                            int chunkLength = chunk._chunkLength;
                            if ((uint)(chunkLength + chunkOffset) <= ret.Length && (uint)chunkLength <= (uint)sourceArray.Length)
                            {
                                fixed (char* sourcePtr = sourceArray)
                                    string.wstrcpy(destinationPtr + chunkOffset, sourcePtr, chunkLength);
                            }
                            else
                                throw new ArgumentOutOfRangeException("chunkLength", __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
                        }
                        chunk = chunk._chunkPrevious;
                    }
                    while (chunk != null);
                }
            }
            return ret;
        }

        [SecuritySafeCritical]
        public string ToString(int startIndex, int length)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            int currentLength = Length;
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_StartIndex));
            if (startIndex > currentLength)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_StartIndexLargerThanLength));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NegativeLength));
            if (startIndex > currentLength - length)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_IndexLength));

            StringBuilder chunk = this;
            int sourceEndIndex = startIndex + length;

            string ret = string.FastAllocateString(length);
            int curDestIndex = length;
            unsafe
            {
                fixed (char* destinationPtr = ret)
                {
                    while (curDestIndex > 0)
                    {
                        int chunkEndIndex = sourceEndIndex - chunk._chunkOffset;
                        if (chunkEndIndex >= 0)
                        {
                            if (chunkEndIndex > chunk._chunkLength)
                                chunkEndIndex = chunk._chunkLength;

                            int countLeft = curDestIndex;
                            int chunkCount = countLeft;
                            int chunkStartIndex = chunkEndIndex - countLeft;
                            if (chunkStartIndex < 0)
                            {
                                chunkCount += chunkStartIndex;
                                chunkStartIndex = 0;
                            }
                            curDestIndex -= chunkCount;

                            if (chunkCount > 0)
                            {
                                char[] sourceArray = chunk._chunkChars;
                                if ((uint)(chunkCount + curDestIndex) <= length && (uint)(chunkCount + chunkStartIndex) <= (uint)sourceArray.Length)
                                {
                                    fixed (char* sourcePtr = &sourceArray[chunkStartIndex])
                                        string.wstrcpy(destinationPtr + curDestIndex, sourcePtr, chunkCount);
                                }
                                else
                                    throw new ArgumentOutOfRangeException("chunkCount", __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
                            }
                        }
                        chunk = chunk._chunkPrevious;
                    }
                }
            }
            return ret;
        }

        public StringBuilder Clear()
        {
            Length = 0;
            return this;
        }

        public int Length
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return _chunkOffset + _chunkLength;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NegativeLength));
                if (value > MaxCapacity)
                    throw new ArgumentOutOfRangeException(nameof(value), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_SmallCapacity));
                Contract.EndContractBlock();

                int originalCapacity = Capacity;
                if (value == 0 && _chunkPrevious == null)
                {
                    _chunkLength = 0;
                    _chunkOffset = 0;
                    Contract.Assert(Capacity >= originalCapacity, "setting the Length should never decrease the Capacity");
                    return;
                }

                int delta = value - Length;
                if (delta > 0)
                    Append('\0', delta);
                else
                {
                    StringBuilder chunk = FindChunkForIndex(value);
                    if (chunk != this)
                    {
                        int newLen = originalCapacity - chunk._chunkOffset;
                        char[] newArray = new char[newLen];

                        Contract.Assert(newLen > chunk._chunkChars.Length, "the new chunk should be larger than the one it is replacing");
                        Array.Copy(chunk._chunkChars, newArray, chunk._chunkLength);

                        _chunkChars = newArray;
                        _chunkPrevious = chunk._chunkPrevious;
                        _chunkOffset = chunk._chunkOffset;
                    }
                    _chunkLength = value - chunk._chunkOffset;
                }
                Contract.Assert(Capacity >= originalCapacity, "setting the Length should never decrease the Capacity");
            }
        }

        [IndexerName("Chars")]
        public char this[int index]
        {
            get
            {
                StringBuilder chunk = this;
                for (;;)
                {
                    int indexInBlock = index - chunk._chunkOffset;
                    if (indexInBlock >= 0)
                    {
                        if (indexInBlock >= chunk._chunkLength)
                            throw new IndexOutOfRangeException();
                        return chunk._chunkChars[indexInBlock];
                    }
                    chunk = chunk._chunkPrevious;
                    if (chunk == null)
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                StringBuilder chunk = this;
                for (;;)
                {
                    int indexInBlock = index - chunk._chunkOffset;
                    if (indexInBlock >= 0)
                    {
                        if (indexInBlock >= chunk._chunkLength)
                            throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
                        chunk._chunkChars[indexInBlock] = value;
                        return;
                    }
                    chunk = chunk._chunkPrevious;
                    if (chunk == null)
                        throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
                }
            }
        }

        public StringBuilder Append(char value, int repeatCount)
        {
            if (repeatCount < 0)
                throw new ArgumentOutOfRangeException(nameof(repeatCount), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NegativeCount));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Contract.EndContractBlock();

            if (repeatCount == 0)
                return this;

            int idx = _chunkLength;
            while (repeatCount > 0)
            {
                if (idx < _chunkChars.Length)
                {
                    _chunkChars[idx++] = value;
                    --repeatCount;
                }
                else
                {
                    _chunkLength = idx;
                    ExpandByABlock(repeatCount);
                    Contract.Assert(_chunkLength == 0, "Expand should create a new block");
                    idx = 0;
                }
            }
            _chunkLength = idx;
            return this;
        }

        [SecuritySafeCritical]
        public StringBuilder Append(char[] value, int startIndex, int charCount)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Contract.EndContractBlock();

            if (value == null)
            {
                if (startIndex == 0 && charCount == 0)
                    return this;
                throw new ArgumentNullException(nameof(value));
            }
            if (charCount > value.Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(charCount), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));

            if (charCount == 0)
                return this;

            unsafe
            {
                fixed (char* valueChars = &value[startIndex])
                    Append(valueChars, charCount);
            }
            return this;
        }

        [SecuritySafeCritical]
        public StringBuilder Append(string value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            if (value != null)
            {
                char[] chunkChars = this._chunkChars;
                int chunkLength = this._chunkLength;
                int valueLen = value.Length;
                int newCurrentIndex = chunkLength + valueLen;
                if (newCurrentIndex < chunkChars.Length)
                {
                    if (valueLen <= 2)
                    {
                        if (valueLen > 0)
                            chunkChars[chunkLength] = value[0];
                        if (valueLen > 1)
                            chunkChars[chunkLength + 1] = value[1];
                    }
                    else
                    {
                        unsafe
                        {
                            fixed (char* valuePtr = value)
                            fixed (char* destPtr = &chunkChars[chunkLength])
                                string.wstrcpy(destPtr, valuePtr, valueLen);
                        }
                    }
                    this._chunkLength = newCurrentIndex;
                }
                else
                    AppendHelper(value);
            }
            return this;
        }

        [SecuritySafeCritical]
        private void AppendHelper(string value)
        {
            unsafe
            {
                fixed (char* valueChars = value)
                    Append(valueChars, value.Length);
            }
        }

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecurityCritical]
        internal extern unsafe void ReplaceBufferInternal(char* newBuffer, int newLength);

        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecurityCritical]
        internal extern unsafe void ReplaceBufferAnsiInternal(sbyte* newBuffer, int newLength);

        [SecuritySafeCritical]
        public StringBuilder Append(string value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);

            if (value == null)
            {
                if (startIndex == 0 && count == 0)
                    return this;
                throw new ArgumentNullException(nameof(value));
            }

            if (count == 0)
                return this;

            if (startIndex > value.Length - count)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));

            unsafe
            {
                fixed (char* valueChars = value)
                    Append(valueChars + startIndex, count);
            }
            return this;
        }

        [ComVisible(false)]
        public StringBuilder AppendLine()
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(Environment.NewLine);
        }

        [ComVisible(false)]
        public StringBuilder AppendLine(string value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Append(value);
            return Append(Environment.NewLine);
        }

        [ComVisible(false)]
        [SecuritySafeCritical]
        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), __Resources.GetResourceString(__Resources.Arg_NegativeArgCount));
            if (destinationIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(destinationIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_MustBeNonNegNum, nameof(destinationIndex)));
            if (destinationIndex > destination.Length - count)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.ArgumentOutOfRange_OffsetOut));
            if ((uint)sourceIndex > (uint)Length)
                throw new ArgumentOutOfRangeException(nameof(sourceIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (sourceIndex > Length - count)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_LongerThanSrcString));
            Contract.EndContractBlock();

            StringBuilder chunk = this;
            int sourceEndIndex = sourceIndex + count;
            int curDestIndex = destinationIndex + count;
            while (count > 0)
            {
                int chunkEndIndex = sourceEndIndex - chunk._chunkOffset;
                if (chunkEndIndex >= 0)
                {
                    if (chunkEndIndex > chunk._chunkLength)
                        chunkEndIndex = chunk._chunkLength;

                    int chunkCount = count;
                    int chunkStartIndex = chunkEndIndex - count;
                    if (chunkStartIndex < 0)
                    {
                        chunkCount += chunkStartIndex;
                        chunkStartIndex = 0;
                    }
                    curDestIndex -= chunkCount;
                    count -= chunkCount;

                    ThreadSafeCopy(chunk._chunkChars, chunkStartIndex, destination, curDestIndex, chunkCount);
                }
                chunk = chunk._chunkPrevious;
            }
        }

        [SecuritySafeCritical]
        public StringBuilder Insert(int index, string value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NeedNonNegNum));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Contract.EndContractBlock();

            int currentLength = Length;
            if ((uint)index > (uint)currentLength)
                throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));

            if (value == null || value.Length == 0 || count == 0)
                return this;

            long insertingChars = (long)value.Length * count;
            if (insertingChars > MaxCapacity - Length)
                throw new OutOfMemoryException();
            Contract.Assert(insertingChars + Length < int.MaxValue);

            StringBuilder chunk;
            int indexInChunk;
            MakeRoom(index, (int)insertingChars, out chunk, out indexInChunk, false);
            unsafe
            {
                fixed (char* valuePtr = value)
                {
                    while (count > 0)
                    {
                        ReplaceInPlaceAtChunk(ref chunk, ref indexInChunk, valuePtr, value.Length);
                        --count;
                    }
                }
            }
            return this;
        }

        public StringBuilder Remove(int startIndex, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NegativeLength));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_StartIndex));
            if (length > Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Contract.EndContractBlock();

            if (Length == length && startIndex == 0)
            {
                Length = 0;
                return this;
            }

            if (length > 0)
            {
                StringBuilder chunk;
                int indexInChunk;
                Remove(startIndex, length, out chunk, out indexInChunk);
            }
            return this;
        }

        public StringBuilder Append(bool value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Append(sbyte value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(byte value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(char value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);

            if (_chunkLength < _chunkChars.Length)
                _chunkChars[_chunkLength++] = value;
            else
                Append(value, 1);
            return this;
        }

        public StringBuilder Append(short value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(int value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(long value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(float value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(double value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(decimal value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        [CLSCompliant(false)]
        public StringBuilder Append(ushort value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        [CLSCompliant(false)]
        public StringBuilder Append(uint value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        [CLSCompliant(false)]
        public StringBuilder Append(ulong value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Append(value.ToString(CultureInfo.CurrentCulture));
        }

        public StringBuilder Append(object value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            if (null == value)
                return this;
            return Append(value.ToString());
        }

        [SecuritySafeCritical]
        public StringBuilder Append(char[] value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            if (null != value && value.Length > 0)
            {
                unsafe
                {
                    fixed (char* valueChars = &value[0])
                        Append(valueChars, value.Length);
                }
            }
            return this;
        }

        [SecuritySafeCritical]
        public StringBuilder Insert(int index, string value)
        {
            if ((uint)index > (uint)Length)
                throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Contract.EndContractBlock();
            if (value != null)
            {
                unsafe
                {
                    fixed (char* sourcePtr = value)
                        Insert(index, sourcePtr, value.Length);
                }
            }
            return this;
        }

        public StringBuilder Insert(int index, bool value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(), 1);
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, sbyte value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        public StringBuilder Insert(int index, byte value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        public StringBuilder Insert(int index, short value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        [SecuritySafeCritical]
        public StringBuilder Insert(int index, char value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            unsafe
            {
                Insert(index, &value, 1);
            }
            return this;
        }

        public StringBuilder Insert(int index, char[] value)
        {
            if ((uint)index > (uint)Length)
                throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Contract.EndContractBlock();

            if (value != null)
                Insert(index, value, 0, value.Length);
            return this;
        }

        [SecuritySafeCritical]
        public StringBuilder Insert(int index, char[] value, int startIndex, int charCount)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            int currentLength = Length;
            if ((uint)index > (uint)currentLength)
                throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));

            if (value == null)
            {
                if (startIndex == 0 && charCount == 0)
                    return this;
                throw new ArgumentNullException(__Resources.GetResourceString(__Resources.ArgumentNull_String));
            }

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_StartIndex));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_GenericPositive));
            if (startIndex > value.Length - charCount)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));

            if (charCount > 0)
            {
                unsafe
                {
                    fixed (char* sourcePtr = &value[startIndex])
                        Insert(index, sourcePtr, charCount);
                }
            }
            return this;
        }

        public StringBuilder Insert(int index, int value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        public StringBuilder Insert(int index, long value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        public StringBuilder Insert(int index, float value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        public StringBuilder Insert(int index, double value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        public StringBuilder Insert(int index, decimal value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, ushort value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, uint value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, ulong value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Insert(index, value.ToString(CultureInfo.CurrentCulture), 1);
        }

        public StringBuilder Insert(int index, object value)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            if (null == value)
                return this;
            return Insert(index, value.ToString(), 1);
        }

        public StringBuilder AppendFormat(string format, object arg0)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return AppendFormatHelper(null, format, new ParamsArray(arg0));
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return AppendFormatHelper(null, format, new ParamsArray(arg0, arg1));
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return AppendFormatHelper(null, format, new ParamsArray(arg0, arg1, arg2));
        }

        public StringBuilder AppendFormat(string format, params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException(format == null ? nameof(format) : nameof(args));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();
            return AppendFormatHelper(null, format, new ParamsArray(args));
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return AppendFormatHelper(provider, format, new ParamsArray(arg0));
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0, object arg1)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return AppendFormatHelper(provider, format, new ParamsArray(arg0, arg1));
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0, object arg1, object arg2)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return AppendFormatHelper(provider, format, new ParamsArray(arg0, arg1, arg2));
        }

        public StringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException(format == null ? nameof(format) : nameof(args));
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.EndContractBlock();
            return AppendFormatHelper(provider, format, new ParamsArray(args));
        }

        private static void FormatError()
        {
            throw new FormatException(__Resources.GetResourceString(__Resources.Format_InvalidString));
        }

        internal StringBuilder AppendFormatHelper(IFormatProvider provider, string format, ParamsArray args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            Contract.EndContractBlock();

            int pos = 0;
            int len = format.Length;
            char ch = '\x0';
            ICustomFormatter cf = null;
            if (provider != null)
                cf = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));

            while (true)
            {
                int p = pos;
                int i = pos;
                while (pos < len)
                {
                    ch = format[pos];

                    pos++;
                    if (ch == '}')
                    {
                        if (pos < len && format[pos] == '}')
                            pos++;
                        else
                            FormatError();
                    }

                    if (ch == '{')
                    {
                        if (pos < len && format[pos] == '{')
                            pos++;
                        else
                        {
                            pos--;
                            break;
                        }
                    }

                    Append(ch);
                }

                if (pos == len)
                    break;
                pos++;
                if (pos == len || (ch = format[pos]) < '0' || ch > '9')
                    FormatError();
                int index = 0;
                do
                {
                    index = index * 10 + ch - '0';
                    pos++;
                    if (pos == len)
                        FormatError();
                    ch = format[pos];
                }
                while (ch >= '0' && ch <= '9' && index < 1000000);
                if (index >= args.Length)
                    throw new FormatException(__Resources.GetResourceString("Format_IndexOutOfRange"));
                while (pos < len && (ch = format[pos]) == ' ')
                    pos++;
                bool leftJustify = false;
                int width = 0;
                if (ch == ',')
                {
                    pos++;
                    while (pos < len && format[pos] == ' ')
                        pos++;

                    if (pos == len)
                        FormatError();
                    ch = format[pos];
                    if (ch == '-')
                    {
                        leftJustify = true;
                        pos++;
                        if (pos == len)
                            FormatError();
                        ch = format[pos];
                    }
                    if (ch < '0' || ch > '9')
                        FormatError();
                    do
                    {
                        width = width * 10 + ch - '0';
                        pos++;
                        if (pos == len)
                            FormatError();
                        ch = format[pos];
                    }
                    while (ch >= '0' && ch <= '9' && width < 1000000);
                }

                while (pos < len && (ch = format[pos]) == ' ')
                    pos++;
                object arg = args[index];
                StringBuilder fmt = null;
                if (ch == ':')
                {
                    pos++;
                    p = pos;
                    i = pos;
                    while (true)
                    {
                        if (pos == len)
                            FormatError();
                        ch = format[pos];
                        pos++;
                        if (ch == '{')
                        {
                            if (pos < len && format[pos] == '{')
                                pos++;
                            else
                                FormatError();
                        }
                        else if (ch == '}')
                        {
                            if (pos < len && format[pos] == '}')
                                pos++;
                            else
                            {
                                pos--;
                                break;
                            }
                        }

                        if (fmt == null)
                            fmt = new StringBuilder();
                        fmt.Append(ch);
                    }
                }
                if (ch != '}')
                    FormatError();
                pos++;
                string sFmt = null;
                string s = null;
                if (cf != null)
                {
                    if (fmt != null)
                        sFmt = fmt.ToString();
                    s = cf.Format(sFmt, arg, provider);
                }

                if (s == null)
                {
                    IFormattable formattableArg = arg as IFormattable;
                    if (formattableArg != null)
                    {
                        if (sFmt == null && fmt != null)
                            sFmt = fmt.ToString();
                        s = formattableArg.ToString(sFmt, provider);
                    }
                    else if (arg != null)
                        s = arg.ToString();
                }

                if (s == null)
                    s = string.Empty;
                int pad = width - s.Length;
                if (!leftJustify && pad > 0)
                    Append(' ', pad);
                Append(s);
                if (leftJustify && pad > 0)
                    Append(' ', pad);
            }
            return this;
        }

        public StringBuilder Replace(string oldValue, string newValue)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            return Replace(oldValue, newValue, 0, Length);
        }

        public bool Equals(StringBuilder sb)
        {
            if (sb == null)
                return false;
            if (Capacity != sb.Capacity || MaxCapacity != sb.MaxCapacity || Length != sb.Length)
                return false;
            if (sb == this)
                return true;

            StringBuilder thisChunk = this;
            int thisChunkIndex = thisChunk._chunkLength;
            StringBuilder sbChunk = sb;
            int sbChunkIndex = sbChunk._chunkLength;
            for (;;)
            {
                --thisChunkIndex;
                --sbChunkIndex;

                while (thisChunkIndex < 0)
                {
                    thisChunk = thisChunk._chunkPrevious;
                    if (thisChunk == null)
                        break;
                    thisChunkIndex = thisChunk._chunkLength + thisChunkIndex;
                }

                while (sbChunkIndex < 0)
                {
                    sbChunk = sbChunk._chunkPrevious;
                    if (sbChunk == null)
                        break;
                    sbChunkIndex = sbChunk._chunkLength + sbChunkIndex;
                }

                if (thisChunkIndex < 0)
                    return sbChunkIndex < 0;
                if (sbChunkIndex < 0)
                    return false;
                if (thisChunk._chunkChars[thisChunkIndex] != sbChunk._chunkChars[sbChunkIndex])
                    return false;
            }
        }

        public StringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            int currentLength = Length;
            if ((uint)startIndex > (uint)currentLength)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (count < 0 || startIndex > currentLength - count)
                throw new ArgumentOutOfRangeException(nameof(count), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));
            if (oldValue.Length == 0)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Argument_EmptyName), nameof(oldValue));

            if (newValue == null)
                newValue = string.Empty;

            int[] replacements = null;
            int replacementsCount = 0;
            StringBuilder chunk = FindChunkForIndex(startIndex);
            int indexInChunk = startIndex - chunk._chunkOffset;
            while (count > 0)
            {
                if (StartsWith(chunk, indexInChunk, count, oldValue))
                {
                    if (replacements == null)
                        replacements = new int[5];
                    else if (replacementsCount >= replacements.Length)
                    {
                        int[] newArray = new int[replacements.Length * 3 / 2 + 4];
                        Array.Copy(replacements, newArray, replacements.Length);
                        replacements = newArray;
                    }
                    replacements[replacementsCount++] = indexInChunk;
                    indexInChunk += oldValue.Length;
                    count -= oldValue.Length;
                }
                else
                {
                    indexInChunk++;
                    --count;
                }

                if (indexInChunk >= chunk._chunkLength || count == 0)
                {
                    int index = indexInChunk + chunk._chunkOffset;
                    ReplaceAllInChunk(replacements, replacementsCount, chunk, oldValue.Length, newValue);
                    index += (newValue.Length - oldValue.Length) * replacementsCount;
                    replacementsCount = 0;
                    chunk = FindChunkForIndex(index);
                    indexInChunk = index - chunk._chunkOffset;
                    Contract.Assert(chunk != null || count == 0, "Chunks ended prematurely");
                }
            }
            return this;
        }

        public StringBuilder Replace(char oldChar, char newChar)
        {
            return Replace(oldChar, newChar, 0, Length);
        }

        public StringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
        {
            Contract.Ensures(Contract.Result<StringBuilder>() != null);
            int currentLength = Length;
            if ((uint)startIndex > (uint)currentLength)
                throw new ArgumentOutOfRangeException(nameof(startIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (count < 0 || startIndex > currentLength - count)
                throw new ArgumentOutOfRangeException(nameof(count), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));

            int endIndex = startIndex + count;
            StringBuilder chunk = this;
            for (;;)
            {
                int endIndexInChunk = endIndex - chunk._chunkOffset;
                int startIndexInChunk = startIndex - chunk._chunkOffset;
                if (endIndexInChunk >= 0)
                {
                    int curInChunk = Math.Max(startIndexInChunk, 0);
                    int endInChunk = Math.Min(chunk._chunkLength, endIndexInChunk);
                    while (curInChunk < endInChunk)
                    {
                        if (chunk._chunkChars[curInChunk] == oldChar)
                            chunk._chunkChars[curInChunk] = newChar;
                        curInChunk++;
                    }
                }
                if (startIndexInChunk >= 0)
                    break;
                chunk = chunk._chunkPrevious;
            }
            return this;
        }

        [SecurityCritical]
        [CLSCompliant(false)]
        public unsafe StringBuilder Append(char* value, int valueCount)
        {
            if (valueCount < 0)
                throw new ArgumentOutOfRangeException(nameof(valueCount), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_NegativeCount));
            int newIndex = valueCount + _chunkLength;
            if (newIndex <= _chunkChars.Length)
            {
                ThreadSafeCopy(value, _chunkChars, _chunkLength, valueCount);
                _chunkLength = newIndex;
            }
            else
            {
                int firstLength = _chunkChars.Length - _chunkLength;
                if (firstLength > 0)
                {
                    ThreadSafeCopy(value, _chunkChars, _chunkLength, firstLength);
                    _chunkLength = _chunkChars.Length;
                }
                int restLength = valueCount - firstLength;
                ExpandByABlock(restLength);
                Contract.Assert(_chunkLength == 0, "Expand did not make a new block");
                ThreadSafeCopy(value + firstLength, _chunkChars, 0, restLength);
                _chunkLength = restLength;
            }
            return this;
        }

        [SecurityCritical]
        private unsafe void Insert(int index, char* value, int valueCount)
        {
            if ((uint)index > (uint)Length)
                throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            if (valueCount > 0)
            {
                StringBuilder chunk;
                int indexInChunk;
                MakeRoom(index, valueCount, out chunk, out indexInChunk, false);
                ReplaceInPlaceAtChunk(ref chunk, ref indexInChunk, value, valueCount);
            }
        }

        [SecuritySafeCritical]
        private void ReplaceAllInChunk(int[] replacements, int replacementsCount, StringBuilder sourceChunk, int removeCount, string value)
        {
            if (replacementsCount <= 0)
                return;

            unsafe
            {
                fixed (char* valuePtr = value)
                {
                    int delta = (value.Length - removeCount) * replacementsCount;
                    StringBuilder targetChunk = sourceChunk;
                    int targetIndexInChunk = replacements[0];

                    if (delta > 0)
                        MakeRoom(targetChunk._chunkOffset + targetIndexInChunk, delta, out targetChunk, out targetIndexInChunk, true);
                    int i = 0;
                    for (;;)
                    {
                        ReplaceInPlaceAtChunk(ref targetChunk, ref targetIndexInChunk, valuePtr, value.Length);
                        int gapStart = replacements[i] + removeCount;
                        i++;
                        if (i >= replacementsCount)
                            break;

                        int gapEnd = replacements[i];
                        Contract.Assert(gapStart < sourceChunk._chunkChars.Length, "gap starts at end of buffer.  Should not happen");
                        Contract.Assert(gapStart <= gapEnd, "negative gap size");
                        Contract.Assert(gapEnd <= sourceChunk._chunkLength, "gap too big");
                        if (delta != 0)
                        {
                            fixed (char* sourcePtr = &sourceChunk._chunkChars[gapStart])
                                ReplaceInPlaceAtChunk(ref targetChunk, ref targetIndexInChunk, sourcePtr, gapEnd - gapStart);
                        }
                        else
                        {
                            targetIndexInChunk += gapEnd - gapStart;
                            Contract.Assert(targetIndexInChunk <= targetChunk._chunkLength, "gap not in chunk");
                        }
                    }
                    if (delta < 0)
                        Remove(targetChunk._chunkOffset + targetIndexInChunk, -delta, out targetChunk, out targetIndexInChunk);
                }
            }
        }

        private bool StartsWith(StringBuilder chunk, int indexInChunk, int count, string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (count == 0)
                    return false;
                if (indexInChunk >= chunk._chunkLength)
                {
                    chunk = Next(chunk);
                    if (chunk == null)
                        return false;
                    indexInChunk = 0;
                }
                if (value[i] != chunk._chunkChars[indexInChunk])
                    return false;
                indexInChunk++;
                --count;
            }
            return true;
        }

        [SecurityCritical]
        private unsafe void ReplaceInPlaceAtChunk(ref StringBuilder chunk, ref int indexInChunk, char* value, int count)
        {
            if (count != 0)
            {
                for (;;)
                {
                    int lengthInChunk = chunk._chunkLength - indexInChunk;
                    Contract.Assert(lengthInChunk >= 0, "index not in chunk");

                    int lengthToCopy = Math.Min(lengthInChunk, count);
                    ThreadSafeCopy(value, chunk._chunkChars, indexInChunk, lengthToCopy);

                    // Advance the index.
                    indexInChunk += lengthToCopy;
                    if (indexInChunk >= chunk._chunkLength)
                    {
                        chunk = Next(chunk);
                        indexInChunk = 0;
                    }
                    count -= lengthToCopy;
                    if (count == 0)
                        break;
                    value += lengthToCopy;
                }
            }
        }

        [SecurityCritical]
        private static unsafe void ThreadSafeCopy(char* sourcePtr, char[] destination, int destinationIndex, int count)
        {
            if (count > 0)
            {
                if ((uint)destinationIndex <= (uint)destination.Length && (destinationIndex + count) <= destination.Length)
                {
                    fixed (char* destinationPtr = &destination[destinationIndex])
                        string.wstrcpy(destinationPtr, sourcePtr, count);
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(destinationIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            }
        }

        [SecurityCritical]
        private static void ThreadSafeCopy(char[] source, int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            if (count > 0)
            {
                if ((uint)sourceIndex <= (uint)source.Length && (sourceIndex + count) <= source.Length)
                {
                    unsafe
                    {
                        fixed (char* sourcePtr = &source[sourceIndex])
                            ThreadSafeCopy(sourcePtr, destination, destinationIndex, count);
                    }
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(sourceIndex), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            }
        }

        [SecurityCritical]
        internal unsafe void InternalCopy(IntPtr dest, int len)
        {
            if (len == 0)
                return;

            bool isLastChunk = true;
            byte* dstPtr = (byte*)dest.ToPointer();
            StringBuilder currentSrc = FindChunkForByte(len);

            do
            {
                int chunkOffsetInBytes = currentSrc._chunkOffset * sizeof(char);
                int chunkLengthInBytes = currentSrc._chunkLength * sizeof(char);
                fixed (char* charPtr = &currentSrc._chunkChars[0])
                {
                    byte* srcPtr = (byte*)charPtr;
                    if (isLastChunk)
                    {
                        isLastChunk = false;
                        Buffer.Memcpy(dstPtr + chunkOffsetInBytes, srcPtr, len - chunkOffsetInBytes);
                    }
                    else
                    {
                        Buffer.Memcpy(dstPtr + chunkOffsetInBytes, srcPtr, chunkLengthInBytes);
                    }
                }
                currentSrc = currentSrc._chunkPrevious;
            }
            while (currentSrc != null);
        }

        private StringBuilder FindChunkForIndex(int index)
        {
            Contract.Assert(0 <= index && index <= Length, "index not in string");
            StringBuilder ret = this;
            while (ret._chunkOffset > index)
                ret = ret._chunkPrevious;
            Contract.Assert(ret != null, "index not in string");
            return ret;
        }

        private StringBuilder FindChunkForByte(int byteIndex)
        {
            Contract.Assert(0 <= byteIndex && byteIndex <= Length * sizeof(char), "Byte Index not in string");
            StringBuilder ret = this;
            while (ret._chunkOffset * sizeof(char) > byteIndex)
                ret = ret._chunkPrevious;
            Contract.Assert(ret != null, "Byte Index not in string");
            return ret;
        }

        private StringBuilder Next(StringBuilder chunk)
        {
            if (chunk == this)
                return null;
            return FindChunkForIndex(chunk._chunkOffset + chunk._chunkLength);
        }

        private void ExpandByABlock(int minBlockCharCount)
        {
            Contract.Requires(Capacity == Length, "Expand expect to be called only when there is no space left");
            Contract.Requires(minBlockCharCount > 0, "Expansion request must be positive");
            if (minBlockCharCount + Length > _maxCapacity)
                throw new ArgumentOutOfRangeException("requiredLength", __Resources.GetResourceString(__Resources.ArgumentOutOfRange_SmallCapacity));
            int newBlockLength = Math.Max(minBlockCharCount, Math.Min(Length, cMaxChunkSize));
            _chunkPrevious = new StringBuilder(this);
            _chunkOffset += _chunkLength;
            _chunkLength = 0;
            if (_chunkOffset + newBlockLength < newBlockLength)
            {
                _chunkChars = null;
                throw new OutOfMemoryException();
            }
            _chunkChars = new char[newBlockLength];
        }

        private StringBuilder(StringBuilder from)
        {
            _chunkLength = from._chunkLength;
            _chunkOffset = from._chunkOffset;
            _chunkChars = from._chunkChars;
            _chunkPrevious = from._chunkPrevious;
            _maxCapacity = from._maxCapacity;
        }

        [SecuritySafeCritical]
        private void MakeRoom(int index, int count, out StringBuilder chunk, out int indexInChunk, bool doneMoveFollowingChars)
        {
            Contract.Assert(count > 0, "Count must be strictly positive");
            Contract.Assert(index >= 0, "Index can't be negative");
            if (count + Length > _maxCapacity)
                throw new ArgumentOutOfRangeException("requiredLength", __Resources.GetResourceString(__Resources.ArgumentOutOfRange_SmallCapacity));
            chunk = this;
            while (chunk._chunkOffset > index)
            {
                chunk._chunkOffset += count;
                chunk = chunk._chunkPrevious;
            }
            indexInChunk = index - chunk._chunkOffset;
            if (!doneMoveFollowingChars && chunk._chunkLength <= DefaultCapacity * 2 && chunk._chunkChars.Length - chunk._chunkLength >= count)
            {
                for (int i = chunk._chunkLength; i > indexInChunk;)
                {
                    --i;
                    chunk._chunkChars[i + count] = chunk._chunkChars[i];
                }
                chunk._chunkLength += count;
                return;
            }
            StringBuilder newChunk = new StringBuilder(Math.Max(count, DefaultCapacity), chunk._maxCapacity, chunk._chunkPrevious);
            newChunk._chunkLength = count;
            int copyCount1 = Math.Min(count, indexInChunk);
            if (copyCount1 > 0)
            {
                unsafe
                {
                    fixed (char* chunkCharsPtr = chunk._chunkChars)
                    {
                        ThreadSafeCopy(chunkCharsPtr, newChunk._chunkChars, 0, copyCount1);
                        int copyCount2 = indexInChunk - copyCount1;
                        if (copyCount2 >= 0)
                        {
                            ThreadSafeCopy(chunkCharsPtr + copyCount1, chunk._chunkChars, 0, copyCount2);
                            indexInChunk = copyCount2;
                        }
                    }
                }
            }

            chunk._chunkPrevious = newChunk;
            chunk._chunkOffset += count;
            if (copyCount1 < count)
            {
                chunk = newChunk;
                indexInChunk = copyCount1;
            }
        }

        private StringBuilder(int size, int maxCapacity, StringBuilder previousBlock)
        {
            Contract.Assert(size > 0, "size not positive");
            Contract.Assert(maxCapacity > 0, "maxCapacity not positive");
            _chunkChars = new char[size];
            this._maxCapacity = maxCapacity;
            _chunkPrevious = previousBlock;
            if (previousBlock != null)
                _chunkOffset = previousBlock._chunkOffset + previousBlock._chunkLength;
        }

        [SecuritySafeCritical]
        private void Remove(int startIndex, int count, out StringBuilder chunk, out int indexInChunk)
        {
            Contract.Assert(startIndex >= 0 && startIndex < Length, "startIndex not in string");
            int endIndex = startIndex + count;
            chunk = this;
            StringBuilder endChunk = null;
            int endIndexInChunk = 0;
            for (;;)
            {
                if (endIndex - chunk._chunkOffset >= 0)
                {
                    if (endChunk == null)
                    {
                        endChunk = chunk;
                        endIndexInChunk = endIndex - endChunk._chunkOffset;
                    }
                    if (startIndex - chunk._chunkOffset >= 0)
                    {
                        indexInChunk = startIndex - chunk._chunkOffset;
                        break;
                    }
                }
                else
                {
                    chunk._chunkOffset -= count;
                }
                chunk = chunk._chunkPrevious;
            }
            Contract.Assert(chunk != null, "fell off beginning of string!");
            int copyTargetIndexInChunk = indexInChunk;
            int copyCount = endChunk._chunkLength - endIndexInChunk;
            if (endChunk != chunk)
            {
                copyTargetIndexInChunk = 0;
                chunk._chunkLength = indexInChunk;
                endChunk._chunkPrevious = chunk;
                endChunk._chunkOffset = chunk._chunkOffset + chunk._chunkLength;
                if (indexInChunk == 0)
                {
                    endChunk._chunkPrevious = chunk._chunkPrevious;
                    chunk = endChunk;
                }
            }
            endChunk._chunkLength -= (endIndexInChunk - copyTargetIndexInChunk);
            if (copyTargetIndexInChunk != endIndexInChunk) // Sometimes no move is necessary
                ThreadSafeCopy(endChunk._chunkChars, endIndexInChunk, endChunk._chunkChars, copyTargetIndexInChunk, copyCount);
            Contract.Assert(chunk != null, "fell off beginning of string!");
        }
    }
}