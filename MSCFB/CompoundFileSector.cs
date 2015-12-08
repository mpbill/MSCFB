using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    class CompoundFileSector
    {
        public SectorTypes SectorType { get; private set; }
        public uint SectorNumber { get; private set; }
        public byte[] SectorBytes { get; private set; }
        public CompoundFileSector(byte[] bytes)
        {
            SectorBytes = bytes;
            
            SectorNumber = BitConverter.ToUInt32(SectorBytes, 0);
            switch(SectorNumber)
            {
                case 0xFFFFFFFA:
                {
                        SectorType = SectorTypes.MaxRegSect;
                        break;
                }
                case 0xFFFFFFFB:
                {
                    SectorType = SectorTypes.NotApplicable;
                        break;
                }
                case 0xFFFFFFFC:
                {
                    SectorType = SectorTypes.DifSect;
                        break;
                }
                case 0xFFFFFFFD:
                {
                    SectorType = SectorTypes.FatSect;
                        break;
                }
                case 0xFFFFFFFE:
                {
                    SectorType = SectorTypes.EndOfChain;
                        break;
                }
                case 0xFFFFFFFF:
                {
                        SectorType = SectorTypes.FreeSect;
                        break;
                }
                default:
                {
                        SectorType=SectorTypes.RegSect;
                        break;
                }
            }
            
        }
    }
}
