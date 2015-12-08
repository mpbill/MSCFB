using System.Linq;

namespace MSCFB
{
    public class DirectorySector
    {
        public DirectoryEntry[] DirectoryEntries { get; private set; }
        public DirectorySector(byte[] bytes, MajorVersion Version)
        {
            switch (Version)
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