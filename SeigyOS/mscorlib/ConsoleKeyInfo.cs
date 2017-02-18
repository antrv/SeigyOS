namespace System
{
    [Serializable]
    public struct ConsoleKeyInfo
    {
        private readonly char _keyChar;
        private readonly ConsoleKey _key;
        private readonly ConsoleModifiers _mods;

        public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
        {
            if (((int)key) < 0 || ((int)key) > 255)
                throw new ArgumentOutOfRangeException("key", Environment.GetResourceString("ArgumentOutOfRange_ConsoleKey"));
            Contract.EndContractBlock();

            _keyChar = keyChar;
            _key = key;
            _mods = 0;
            if (shift)
                _mods |= ConsoleModifiers.Shift;
            if (alt)
                _mods |= ConsoleModifiers.Alt;
            if (control)
                _mods |= ConsoleModifiers.Control;
        }

        public char KeyChar => _keyChar;
        public ConsoleKey Key => _key;
        public ConsoleModifiers Modifiers => _mods;

        public override bool Equals(object value)
        {
            return value is ConsoleKeyInfo && Equals((ConsoleKeyInfo)value);
        }

        public bool Equals(ConsoleKeyInfo obj)
        {
            return obj._keyChar == _keyChar && obj._key == _key && obj._mods == _mods;
        }

        public static bool operator ==(ConsoleKeyInfo a, ConsoleKeyInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ConsoleKeyInfo a, ConsoleKeyInfo b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return _keyChar | (int)_mods;
        }
    }
}