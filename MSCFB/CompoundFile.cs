using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MSCFB
{
    public class CompoundFile
    {
        private CompoundFileHeader Header { get; set; }
        private Stream FileStream { get; set; }
        private BinaryReader FileReader { get; set; }
        
        public CompoundFile(Stream fileStream)
        {
            FileStream = fileStream;
            FileStream.Seek(0, SeekOrigin.Begin);
            FileReader = new BinaryReader(FileStream);
            Load();
        }

        private void Load()
        {
            Header = new CompoundFileHeader(FileReader);
            List<CompoundFileSector> Sectors = new List<CompoundFileSector>(200);
            switch(Header.SectorShift)
            {
                case SectorShift.Shift512:
                {
                        
                        if (FileStream.Length%512!=0)
                            throw new SectorSizeFileLengthException();
                        while (FileStream.Position != FileStream.Length)
                        {
                            Sectors.Add(new CompoundFileSector(FileReader.ReadBytes(512)));
                        }

                        break;
                }
                case SectorShift.Shift4096:
                {
                        if(FileStream.Length % 4096 != 0)
                            throw new SectorSizeFileLengthException();
                        while (FileStream.Position != FileStream.Length)
                        {
                            Sectors.Add(new CompoundFileSector(FileReader.ReadBytes(4096)));
                        }
                        break;
                }
                default:
                {
                        break;
                }
            }
            
            
            DirectorySector sector = new DirectorySector(Sectors[2].SectorBytes, Header.MajorVersion);
            Console.WriteLine("");
            DirectorySector sector2 = new DirectorySector(Sectors[3].SectorBytes, MajorVersion.Version3);


            var fatSecOne = Sectors[3];
            var nextSect = BitConverter.ToUInt32(fatSecOne.SectorBytes, 0);
            for (int i = 0; i < Sectors.Count; i++)
            {
                DirectorySector sector3 = new DirectorySector(Sectors[i].SectorBytes, MajorVersion.Version3);
            }

            Console.WriteLine("");

        }


    }
}
