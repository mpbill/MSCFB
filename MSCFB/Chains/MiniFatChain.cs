using System.Collections.Generic;

namespace MSCFB.Chains
{
    public class MiniFatChain
    {
        private CompoundFile CompoundFile { get; set; }
        private List<uint> SectorNumbers { get; set; }
        public MiniFatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
            SectorNumbers = new List<uint>();
            SectorType NextSector = SectorType.EndOfChain;
            if (CompoundFile.Header.FirstMiniFatSectorLocation <= SectorType.MaxRegSect)
            {
                SectorNumbers.Capacity += (int)(CompoundFile.Header.SectorSize/4);
                CompoundFile.MoveReaderToSector((uint)CompoundFile.Header.FirstMiniFatSectorLocation);
                for(int i = 0; i < CompoundFile.Header.SectorSize / 4; i++)
                {
                    SectorNumbers.Add(CompoundFile.FileReader.ReadUInt32());
                }
                //SectorType = CompoundFile.FatSectorChain[]
            }
            //CompoundFile.

        }
    }
}
