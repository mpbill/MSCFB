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
            return (uint)((CompoundFile.Header.SectorSize * CompoundFile.Header.NumberOfMiniFatSectors) / CompoundFile.Header.MiniSectorShift);
        }
        private void MoveToIndex(SectorType N)
        {

            var num = (SectorType)((CompoundFile.Header.Int32sInSector) / (uint)N);
            var remainder = (CompoundFile.Header.SectorSize / 4) % (uint)N;
            CompoundFile.MoveReaderToSector(remainder == 0
                ? CompoundFile.DifatChain[num]
                : CompoundFile.DifatChain[num + 1]);
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
