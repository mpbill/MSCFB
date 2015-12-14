using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    public class DirectoryChain
    {
        public CompoundFile CompoundFile { get; private set; }
        public List<DirectorySector> DirectorySectors { get; private set; }
        public DirectoryChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
            DirectorySectors = new List<DirectorySector>();
            DirectorySectors.Add(new DirectorySector((uint)CompoundFile.Header.FirstDirectorySectorLocation, CompoundFile));
            int i = 0;
            SectorType nextSectorInChain =
                CompoundFile.FatSectorChain.SectorsList[(int) DirectorySectors.Last().SectorNumber];
            while (nextSectorInChain!=SectorType.EndOfChain)
            {
                
                DirectorySectors.Add(new DirectorySector((uint)nextSectorInChain, CompoundFile));
                nextSectorInChain =
                CompoundFile.FatSectorChain.SectorsList[(int)DirectorySectors.Last().SectorNumber];
                i++;
            }
        }
    }
}
