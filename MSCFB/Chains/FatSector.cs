using System.Collections.Generic;

namespace MSCFB.Chains
{
    public class FatSectorChain
    {
        public CompoundFile CompoundFile { get; private set; }
        private SortedDictionary<SectorType, SectorType> SectorsDict { get; set; } 
        public List<SectorType> SectorsList { get; private set; }
        private SectorType NextIndex { get; set; } = (SectorType) 0;
        public FatSectorChain(CompoundFile compoundFile)
        {
            this.CompoundFile = compoundFile;
            SectorsDict = new SortedDictionary<SectorType, SectorType>();
            SectorsList = new List<SectorType>();
            LoadSectors();
            
        }
        public SectorType this[int index]
        {
            get { return SectorsList[index]; }
            set { SectorsList[index] = value; }
        }
        private void LoadSectors()
        {
            CompoundFile.MoveReaderToSector((uint)CompoundFile.Header.DifatArray[0]);
            while (true)
            {
                
                for (int i = 0; i < CompoundFile.Header.SectorSize/4; i++)
                {
                    this.Add((SectorType) CompoundFile.FileReader.ReadUInt32());
                }
                break;
            }
        }

        public void Add(SectorType sectorType)
        {
            
            SectorsDict.Add(NextIndex, sectorType);
            NextIndex++;
        }

        //public byte[] this[int index]
        //{
        //    get
        //    {
        //        long offset = (index + 1)*SectorSize;
        //        BinaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
        //        return BinaryReader.ReadBytes(SectorSize);
        //    }
        //    set
        //    {
                
        //    }
        //}
    }
}