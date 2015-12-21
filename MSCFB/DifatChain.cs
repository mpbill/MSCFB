using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    public class DifatChain
    {
        public CompoundFile CompoundFile { get; private set; }
        private List<SectorType> SectorNumbers { get; set; }
        public uint this[int index]
        {
            get { return (uint)SectorNumbers[index]; }
            set { SectorNumbers[index] = (SectorType)value; }
        }
        public DifatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
            SectorNumbers = new List<SectorType>(CompoundFile.Header.DifatArray);
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
