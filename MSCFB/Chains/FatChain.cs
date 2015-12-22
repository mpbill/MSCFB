using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSCFB.Static;

namespace MSCFB.Chains
{
    public class FatChain : IEnumerable<SectorType>
    {
        public CompoundFile CompoundFile { get; private set; }
        public List<SectorType> List => this.ToList();
        public FatChain(CompoundFile compoundFile)
        {
            this.CompoundFile = compoundFile;
            
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
                    count += (uint)(CompoundFile.Header.Int32sInSector);
                else
                {
                    break;
                }
            }
            return count;
        }
        private void MoveToIndex(SectorType N)
        {
            if (N == 0)
                CompoundFile.MoveReaderToSector(CompoundFile.DifatChain[(SectorType)0]);
            else
            {
                var num = (SectorType)((uint)N/(CompoundFile.Header.Int32sInSector));
                var remainder = (uint)N%(CompoundFile.Header.Int32sInSector);
                CompoundFile.MoveReaderToSector(CompoundFile.DifatChain[num]);
                CompoundFile.Seek(remainder*4, SeekOrigin.Current);

            }
        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            var c = CalculateCount();
            for(uint i = 0;i<c; i++)
            {
                yield return (SectorType)this[(SectorType)i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}