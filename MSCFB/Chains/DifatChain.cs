using System.Collections.Generic;
using System.Linq;

namespace MSCFB.Chains
{
    public class DifatChain
    {
        public CompoundFile CompoundFile { get; private set; }
        public SectorType this[SectorType index]
        {
            get
            {
                if (index < (SectorType)CompoundFile.Header.DifatArray.Count - 1)
                    return CompoundFile.Header.DifatArray[(int)index];
                else if()
            }
            set { SectorNumbers[index] = (SectorType)value; }
        }
        public uint this[uint index]
        {
            get
            {
                
            }
            set { }
        }
        public DifatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
            AddNextSector();


        }
        private void AddNextSector()
        {
            if (SectorNumbers.Last() != SectorType.FreeSect)
            {
                CompoundFile.MoveReaderToSector((uint)SectorNumbers.Last());
                SectorNumbers.RemoveAt(SectorNumbers.Count-1);
                for(int i=0; i < (CompoundFile.Header.SectorSize / 4); i++)
                {
                    SectorNumbers.Add((SectorType)CompoundFile.FileReader.ReadUInt32()); 
                }
                AddNextSector();
            }
            else
            {
                return;
            }
        }
    }
}
