namespace System.Runtime.Serialization
{
    public sealed class SafeSerializationEventArgs: EventArgs
    {
        private readonly StreamingContext _streamingContext;
        private readonly List<object> _serializedStates = new List<object>();

        internal SafeSerializationEventArgs(StreamingContext streamingContext)
        {
            _streamingContext = streamingContext;
        }

        public void AddSerializedState(ISafeSerializationData serializedState)
        {
            if (serializedState == null)
                throw new ArgumentNullException(nameof(serializedState));
            if (!serializedState.GetType().IsSerializable)
                throw new ArgumentException(__Resources.GetResourceString("Serialization_NonSerType", serializedState.GetType(),
                    serializedState.GetType().Assembly.FullName));

            _serializedStates.Add(serializedState);
        }

        public StreamingContext StreamingContext => _streamingContext;
    }
}