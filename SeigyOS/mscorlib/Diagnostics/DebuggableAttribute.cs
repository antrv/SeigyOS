using System.Runtime.InteropServices;

namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
    [ComVisible(true)]
    public sealed class DebuggableAttribute: Attribute
    {
        [Flags]
        [ComVisible(true)]
        public enum DebuggingModes
        {
            None = 0x0,
            Default = 0x1,
            DisableOptimizations = 0x100,
            IgnoreSymbolStoreSequencePoints = 0x2,
            EnableEditAndContinue = 0x4
        }

        private readonly DebuggingModes _debuggingModes;

        // ReSharper disable InconsistentNaming
        public DebuggableAttribute(bool isJITTrackingEnabled,
            bool isJITOptimizerDisabled)
        {
            _debuggingModes = 0;
            if (isJITTrackingEnabled)
                _debuggingModes |= DebuggingModes.Default;
            if (isJITOptimizerDisabled)
                _debuggingModes |= DebuggingModes.DisableOptimizations;
        }

        public DebuggableAttribute(DebuggingModes modes)
        {
            _debuggingModes = modes;
        }

        public bool IsJITTrackingEnabled => (_debuggingModes & DebuggingModes.Default) != 0;
        public bool IsJITOptimizerDisabled => (_debuggingModes & DebuggingModes.DisableOptimizations) != 0;
        public DebuggingModes DebuggingFlags => _debuggingModes;
    }
}