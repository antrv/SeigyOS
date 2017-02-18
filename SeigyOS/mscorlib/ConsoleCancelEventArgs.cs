namespace System
{
    [Serializable]
    public sealed class ConsoleCancelEventArgs: EventArgs
    {
        private readonly ConsoleSpecialKey _type;
        private bool _cancel;

        internal ConsoleCancelEventArgs(ConsoleSpecialKey type)
        {
            _type = type;
            _cancel = false;
        }

        public bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }

        public ConsoleSpecialKey SpecialKey => _type;
    }
}