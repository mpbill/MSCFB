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
        public List<SectorType> List => this.ToList(); 
        public CompoundFile CompoundFile { get; private set; }
        /// <summary>
        ///109 entries in header minus the last entry which points to next difat sector,
        ///plus number of difat sectors times the sector size in bytes divided by 4, the number of bytes in an unsigned int.
        ///minus one for each difat sector which marks the next difat sector.
        /// </summary>
        public long Count => 108 + (CompoundFile.Header.NumberOfDifatSectors) * CompoundFile.Header.SectorSize / 4 -
                             CompoundFile.Header.NumberOfDifatSectors;

        public SectorType this[SectorType index]
        {
            get
            {
                try
                {
                    MoveToIndex(index);
                    return CompoundFile.FileReader.ReadSectorType();
                }
                catch (IndexOutOfRangeException)
                {
                    throw;
                }
                
            }
            set
            {
                MoveToIndex(index);
                CompoundFile.FileWriter.Write(value);
            }
        }
        public DifatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
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
                        CompoundFile.MoveReaderToSector(nextSector);
                        CompoundFile.Seek((long)N*4, SeekOrigin.Current);
                        return;
                    }
                    else
                    {
                        CompoundFile.MoveReaderToSector(nextSector);
                        CompoundFile.Seek(CompoundFile.Header.SectorSize-4, SeekOrigin.Current);
                        nextSector = CompoundFile.FileReader.ReadSectorType();
                        LastIndexInThisSector += 126;
                        continue;
                    }
                }
                throw new IndexOutOfRangeException();
            }
            
        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            var c = (SectorType)Count;
            for(SectorType i = 0; i < c; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
