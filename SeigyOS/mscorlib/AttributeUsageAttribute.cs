using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    [ComVisible(true)]
    public sealed class AttributeUsageAttribute: Attribute
    {
        private readonly AttributeTargets _attributeTarget;
        private bool _allowMultiple = false;
        private bool _inherited = true;

        //Constructors 
        public AttributeUsageAttribute(AttributeTargets validOn)
        {
            _attributeTarget = validOn;
        }

        public AttributeTargets ValidOn => _attributeTarget;

        public bool AllowMultiple
        {
            get
            {
                return _allowMultiple;
            }
            set
            {
                _allowMultiple = value;
            }
        }

        public bool Inherited
        {
            get
            {
                return _inherited;
            }
            set
            {
                _inherited = value;
            }
        }
    }
}