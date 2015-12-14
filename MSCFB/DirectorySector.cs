using System;
using System.Linq;
using MSCFB.Enum;
using MSCFB.Exception;

namespace MSCFB
{
    public class DirectorySector
    {

        public CompoundFile CompoundFile { get; private set; }
        public uint SectorNumber { get; private set; }
        public DirectoryEntry[] DirectoryEntries { get; private set; }
        public DirectorySector(uint sectorNumber, CompoundFile compoundFile)
        {
            SectorNumber = sectorNumber;
            CompoundFile = compoundFile;
            var bytes = CompoundFile.ReadSector(sectorNumber);
            switch (CompoundFile.Header.MajorVersion)
            {
                default:
                {
                    throw new InvalidMcdfHeaderException("Versions Incompatible");
                }
                case MajorVersion.Version3:
                {
                    int n = 4;
                    int s = 512;
                    DirectoryEntries = new DirectoryEntry[n];
                    for (int i = 0; i < n; i++)
                    {
                            DirectoryEntries[i] = new DirectoryEntry(bytes.Skip(i*s/n).Take(s / n).ToArray());
                        
                    }
                    break;
                }
                case MajorVersion.Version4:
                {
                        int n = 32;
                        int s = 4096;
                        DirectoryEntries = new DirectoryEntry[n];
                        for (int i = 0; i < n; i++)
                        {
                            DirectoryEntries[i] = new DirectoryEntry(bytes.Skip(i * s / n).Take(s / n).ToArray());

                        }
                        break;
                }
                    
            }
        }
    }
}