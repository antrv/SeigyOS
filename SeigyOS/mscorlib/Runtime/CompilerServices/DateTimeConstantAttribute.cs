using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    [ComVisible(true)]
    public sealed class DateTimeConstantAttribute: CustomConstantAttribute
    {
        private readonly DateTime _date;

        public DateTimeConstantAttribute(long ticks)
        {
            _date = new DateTime(ticks);
        }

        public override object Value => _date;
    }
}