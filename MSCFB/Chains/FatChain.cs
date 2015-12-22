using System;
using System.Collections;
using System.Collections.Generic;
using MSCFB.Static;

namespace MSCFB.Chains
{
    public class FatChain : IEnumerable<SectorType>
    {
        public CompoundFile CompoundFile { get; private set; }
        
        public FatChain(CompoundFile compoundFile)
        {
            this.CompoundFile = compoundFile;
            
        }
        public uint this[uint index]
        {
            get
            {
                MoveToIndex((SectorType)index);
                return CompoundFile.FileReader.ReadUInt32();
            }
            set
            {
                MoveToIndex((SectorType)index);
                CompoundFile.FileWriter.Write(value);
            }
        }
        public SectorType this[SectorType index]
        {
            get
            {
                MoveToIndex(index);
                return CompoundFile.FileReader.ReadSectorType();
            }
            set
            {
                MoveToIndex(index);
                CompoundFile.FileWriter.Write(value);
            }
        }
        private uint CalculateCount()
        {
            uint count = 0;
            foreach (SectorType sectorType in CompoundFile.DifatChain)
            {
                if (sectorType <= SectorType.MaxRegSect)
                    count += (CompoundFile.Header.SectorSize / 4);
                else
                {
                    break;
                }
            }
            return count;
        }
        private void MoveToIndex(SectorType N)
        {
            if(N==0)
                CompoundFile.MoveReaderToSector(CompoundFile.DifatChain[0]);
            var num = (CompoundFile.Header.SectorSize / 4) / (uint)N;
            var remainder = (CompoundFile.Header.SectorSize / 4) % (uint)N;
            if(remainder==0)
                CompoundFile.MoveReaderToSector(CompoundFile.DifatChain[num]);
            else
            {
                CompoundFile.MoveReaderToSector(CompoundFile.DifatChain[num+1]);
            }
            
        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            var c = CalculateCount();
            for(uint i = 0;i<c; i++)
            {
                yield return (SectorType)this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}