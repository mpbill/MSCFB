using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    public static class Resources
    {
        /// <summary>
        /// Version number for nonbreaking changes. This field SHOULD be set to 0x003E if the major version field is either 0x0003 or 0x0004.
        /// </summary>
        public static ushort CorrectMinorVersion
        {
            get { return _correctMinorVersion; }
        }

        public static byte[] ReservedBytes
        {
            get { return ReservedBytes1; }
        }

        private static readonly byte[] _remainingbytes0 = Enumerable.Repeat((byte)0x00, 3584).ToArray();
        public static byte[] RemainingBytes0 { get { return _remainingbytes0; } }
        private static readonly byte[] _headerSectionReserved = Enumerable.Repeat((byte)0x00, 6).ToArray();
        public static byte[] HeaderSectionReserved { get { return _headerSectionReserved; } }
        private static readonly byte[] _mcdfHeaderSigniture = new byte[8]
        {
            0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1
        };

        private static readonly byte[] _mcdfHeaderClsid = Enumerable.Repeat((byte)0x00, 16).ToArray();
        private static readonly ushort _correctMinorVersion = 0x003E;
        private static readonly byte[] ReservedBytes1 = Enumerable.Repeat((byte)0x00, 6).ToArray();

        public static byte[] McdfHeaderSigniture
        {
            get { return _mcdfHeaderSigniture; }

        }

        public static byte[] McdfHeaderClsid
        {
            get { return _mcdfHeaderClsid; }
        }
        
        
    }
}
