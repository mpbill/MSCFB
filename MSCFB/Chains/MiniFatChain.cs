using System.Collections;
using System.Collections.Generic;
using System.IO;
using MSCFB.Static;

namespace MSCFB.Chains
{
    public class MiniFatChain : IEnumerable<SectorType>
    {
        private CompoundFile CompoundFile { get; set; }
        private long Count => CompoundFile.Header.NumberOfMiniFatSectors * CompoundFile.Header.Int32sInSector;
        public MiniFatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;

        }
        private SectorType this[SectorType index]
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
        private void MoveToIndex(SectorType N)
        {
            
            var num = (SectorType)((uint)N/(CompoundFile.Header.Int32sInSector));
            var remainder = (uint)N % (CompoundFile.Header.SectorSize / 4);
            CompoundFile.MoveReaderToSector(CompoundFile.FatChain[num]);
            CompoundFile.Seek(remainder*4, SeekOrigin.Current);
        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            for(uint i = 0; i < Count; i++)
            {
                yield return this[(SectorType)i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
