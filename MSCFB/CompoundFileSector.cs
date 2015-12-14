using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    
    public class CompoundFileSector
    {
        public SectorType SectorType { get; private set; }
        public uint SectorNumber { get; private set; }
        public byte[] SectorBytes { get; private set; }
        public CompoundFileSector(byte[] bytes)
        {
            SectorBytes = bytes;
            
            SectorNumber = BitConverter.ToUInt32(SectorBytes, 0);
            SectorType = (SectorType) SectorNumber;


        }
    }
}
