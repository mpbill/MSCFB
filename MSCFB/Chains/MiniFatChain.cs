using System.Collections;
using System.Collections.Generic;

namespace MSCFB.Chains
{
    public class MiniFatChain : IEnumerable<SectorType>
    {
        private CompoundFile CompoundFile { get; set; }

        public MiniFatChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;

        }
        private uint CalculateCount()
        {
            return (CompoundFile.Header.SectorSize * CompoundFile.Header.NumberOfMiniFatSectors) / CompoundFile.Header.MiniSectorShift;
        }
        private void MoveToIndex(SectorType N)
        {

            var num = (CompoundFile.Header.SectorSize / 4) / (uint)N;
            var remainder = (CompoundFile.Header.SectorSize / 4) % (uint)N;
            if (remainder == 0)
                CompoundFile.MoveReaderToSector(CompoundFile.DifatChain[num]);
            else
            {
                CompoundFile.MoveReaderToSector(CompoundFile.DifatChain[num + 1]);
            }

        }
        public IEnumerator<SectorType> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
