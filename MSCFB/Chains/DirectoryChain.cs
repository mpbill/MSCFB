using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSCFB.Static;

namespace MSCFB.Chains
{
    public class DirectoryChain : IEnumerable<SectorType>
    {
        public CompoundFile CompoundFile { get; private set; }
        
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
        public uint this[uint index]
        {
            get { return (uint)this[(SectorType)index]; }
            set { this[(SectorType)index] = (SectorType)value; }
        }
        public DirectoryChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
        }
        private uint CalculateCount()
        {
            uint count = CompoundFile.Header.NumberOfDirectorySectors * (CompoundFile.Header.SectorSize / 128);
            return count;
            
        }
        private void MoveToIndex(SectorType N)
        {

            var num = (CompoundFile.Header.SectorSize / 128) / (uint)N;
            var remainder = (CompoundFile.Header.SectorSize / 128) % (uint)N;
            if (remainder == 0)
                CompoundFile.MoveReaderToSector(CompoundFile.FatChain[num]);
            else
            {
                CompoundFile.MoveReaderToSector(CompoundFile.FatChain[num + 1]);
                CompoundFile.Seek(remainder*128, SeekOrigin.Current);
            }

        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            var c = CalculateCount();
            for(uint i = 0; i < c; i++)
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
