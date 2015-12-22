using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSCFB.Static;

namespace MSCFB.Chains
{
    
    public class DifatChain : IEnumerable<SectorType>
    {
        public CompoundFile CompoundFile { get; private set; }
        private long position = -1;

        public long Count
        {
            get
            {
                return CalculateCount();
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
        public DifatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
            AddNextSector();


        }
        private void AddNextSector()
        {

        }
        private long CalculateCount()
        {
            CompoundFile.Seek(508, SeekOrigin.Begin);
            SectorType nextSector = CompoundFile.FileReader.ReadSectorType();
            long count = 108;
            if (nextSector > SectorType.MaxRegSect)
            {
                return count;
            }
            else
            {
                while (nextSector <= SectorType.MaxRegSect)
                {
                    
                    CompoundFile.MoveReaderToSector((uint)nextSector);
                    CompoundFile.Seek(CompoundFile.Header.SectorSize - 4, SeekOrigin.Current);
                    nextSector = CompoundFile.FileReader.ReadSectorType();
                    count += ((long)((CompoundFile.Header.SectorSize) - 1) / 4);
                    continue;
                }
            }
            return count;
        }
        public void MoveToIndex(SectorType N)
        {
            if (N <= (SectorType)107)
            {
                //if N is in the Difat Sector Array contained in the header return it.
                CompoundFile.Seek(76, SeekOrigin.Begin);
                CompoundFile.Seek(((long)N)*4, SeekOrigin.Current);
                
            }
            else
            {
                CompoundFile.Seek(508, SeekOrigin.Begin);
                SectorType nextSector = CompoundFile.FileReader.ReadSectorType();
                SectorType LastIndexInThisSector =(SectorType)(107 + 126);
                while (nextSector <= SectorType.MaxRegSect)
                {
                    if(N<LastIndexInThisSector)
                    {
                        CompoundFile.MoveReaderToSector((uint)nextSector);
                        CompoundFile.Seek((long)N*4, SeekOrigin.Current);
                        break;
                    }
                    else
                    {
                        CompoundFile.MoveReaderToSector((uint)nextSector);
                        CompoundFile.Seek(CompoundFile.Header.SectorSize-4, SeekOrigin.Current);
                        nextSector = CompoundFile.FileReader.ReadSectorType();
                        LastIndexInThisSector += 126;
                        continue;
                    }
                }
            }
            
        }

        public void Dispose()
        {
            return;
        }

        public bool MoveNext()
        {
            position++;
            return position < Count;
        }

        public void Reset()
        {
            position = -1;
        }

        public SectorType Current
        {
            get
            {
                if(position==-1)
                    throw new InvalidOperationException();
                return this[(SectorType)position];
            }
        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            var c = Count;
            for(uint i = 0; i < c; i++)
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
