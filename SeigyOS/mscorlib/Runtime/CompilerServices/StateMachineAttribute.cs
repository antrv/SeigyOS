namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class StateMachineAttribute: Attribute
    {
        public StateMachineAttribute(Type stateMachineType)
        {
            StateMachineType = stateMachineType;
        }

        public Type StateMachineType { get; private set; }
    }
}