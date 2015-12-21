using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    public class MiniFatChain
    {
        private CompoundFile CompoundFile { get; set; }
        private List<uint> SectorNumbers { get; set; }
        public MiniFatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
            SectorNumbers = new List<uint>();
            if (CompoundFile.Header.FirstMiniFatSectorLocation <= SectorType.MaxRegSect)
            {
                CompoundFile.MoveReaderToSector((uint)CompoundFile.Header.FirstMiniFatSectorLocation);
                for(int i = 0; i < CompoundFile.Header.SectorSize / 4; i++)
                {
                    SectorNumbers.Add(CompoundFile.FileReader.ReadUInt32());
                }
            }
            CompoundFile.

        }
    }
}
