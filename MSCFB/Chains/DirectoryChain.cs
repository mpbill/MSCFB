using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSCFB.Enum;
using MSCFB.Static;

namespace MSCFB.Chains
{
    /// <summary>
    /// A chain of byte offsets for the beginings of the Nth Directory Entry's
    /// </summary>
    public class DirectoryChain : IEnumerable<long>
    {
        public CompoundFile CompoundFile { get; private set; }
        public List<long> List => this.ToList(); 
        public long this[StreamID index]
        {
            get
            {
                
                return IndexToOffset(index);
                
            }
        }
        public DirectoryChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
        }
        private uint CalculateCount()
        {
            uint count =(uint)( CompoundFile.Header.NumberOfDirectorySectors * (CompoundFile.Header.DirectoryEntriesInSector));
            return count;
            
        }
        private long IndexToOffset(StreamID N)
        {
            
            if(N==0)
                return CompoundFile.SectorNumberToOffset(CompoundFile.FatChain[CompoundFile.Header.FirstDirectorySectorLocation]);
            else
            {
                //CompoundFile.SectorNumberToOffset(this[N - 1]);
                var num = (SectorType)((uint)N/(CompoundFile.Header.DirectoryEntriesInSector));
                var remainder = (uint)N%(CompoundFile.Header.DirectoryEntriesInSector);
                if (remainder == 0)
                    return CompoundFile.SectorNumberToOffset(CompoundFile.FatChain[num]);
                else
                {
                    return CompoundFile.SectorNumberToOffset(CompoundFile.FatChain[num + 1]) + remainder * 128;
                }
            }
            

        }
        public IEnumerator<long> GetEnumerator()
        {
            var c = CalculateCount();
            for(uint i = 0; i < c; i++)
            {
                yield return this[(StreamID)i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
