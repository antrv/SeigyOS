using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    [ComVisible(true)]
    public sealed class DecimalConstantAttribute: Attribute
    {
        private readonly decimal _value;

        [CLSCompliant(false)]
        public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low)
        {
            _value = new decimal((int)low, (int)mid, (int)hi, sign != 0, scale);
        }

        public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low)
        {
            _value = new decimal(low, mid, hi, sign != 0, scale);
        }

        public decimal Value => _value;
    }
}