using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB.Chains
{
    class DirectorySectorChain : IEnumerable<SectorType>
    {
        public CompoundFile CompoundFile { get; private set; }
        public long Count
        {
            get
            {
                long result = 0;
                SectorType NextSect = CompoundFile.Header.FirstDirectorySectorLocation;
                while (NextSect <= SectorType.MaxRegSect)
                {
                    result++;
                    NextSect = CompoundFile.FatChain[NextSect];
                }
                return result;
            }
        }
        public SectorType this[long index]
        {
            get
            {
                long i = -1;
                foreach (SectorType sectorType in this)
                {
                    i++;
                    if (i == index)
                        return sectorType;
                    continue;
                }
                throw new IndexOutOfRangeException();
                
            }
            set
            {
                long i = -1;
                foreach (SectorType sectorType in this)
                {
                    i++;
                    if (i == index)
                        CompoundFile.FatChain[sectorType] = value;
                    continue;
                }
                throw new IndexOutOfRangeException();

            }
        }
        public SectorType IndexToFatIndex(long index)
        {
            long i = 0;
            SectorType NextSect = CompoundFile.Header.FirstDirectorySectorLocation;
            if (index == i)
                return NextSect;
            else
            {
                while (NextSect <= SectorType.MaxRegSect)
                {
                    NextSect = CompoundFile.FatChain[NextSect];
                    i++;
                    if (index == i && NextSect<=SectorType.MaxRegSect)
                        return NextSect;
                }
                throw new IndexOutOfRangeException();
            }
        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            SectorType NextSect = CompoundFile.Header.FirstDirectorySectorLocation;
            while (NextSect <= SectorType.MaxRegSect)
            {
                yield return NextSect;
                NextSect = CompoundFile.FatChain[NextSect];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
