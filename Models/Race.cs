using System;

namespace SessionProject2W5.Models
{
    public class Race : Datum
    {
        [Obsolete("This member is unused!", true)]
        public new string Name;
        public string CommonName;
        public string NativeName;
        public int Population = 0;
    }
}