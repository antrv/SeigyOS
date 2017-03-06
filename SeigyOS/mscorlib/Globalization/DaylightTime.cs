using System.Runtime.InteropServices;

namespace System.Globalization
{
    [Serializable]
    [ComVisible(true)]
    public class DaylightTime
    {
        private readonly DateTime _start;
        private readonly DateTime _end;
        private readonly TimeSpan _delta;

        private DaylightTime()
        {
        }

        public DaylightTime(DateTime start, DateTime end, TimeSpan delta)
        {
            _start = start;
            _end = end;
            _delta = delta;
        }

        public DateTime Start => _start;
        public DateTime End => _end;
        public TimeSpan Delta => _delta;
    }
}