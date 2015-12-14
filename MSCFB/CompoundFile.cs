using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSCFB.Enum;

namespace MSCFB
{
    public class CompoundFile
    {
        public CompoundFileHeader Header { get; private set; }
        private Stream FileStream { get; set; }
        public BinaryReader FileReader { get; private set; }
        public FatSectorChain FatSectorChain { get; private set; }
        public DirectoryChain DirectoryChain { get; private set; }
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
            FatSectorChain = new FatSectorChain(this);
            DirectoryChain = new DirectoryChain(this);
            return;
        }

        internal UInt32 SectorNumberToOffset(uint sectorNumber)
        {
            return (sectorNumber + 1)*512;
        }
        internal void MoveReaderToSector(uint sectorNumber)
        {
            MoveReaderToPosition(SectorNumberToOffset(sectorNumber));
        }

        internal void MoveReaderToPosition(uint position)
        {
            FileReader.BaseStream.Position = position;
        }

        internal byte[] ReadSector(uint sectorNumber)
        {
            MoveReaderToSector(sectorNumber);
            return FileReader.ReadBytes((int) Resources.UIntPow(2, (uint) Header.SectorShift));
        }

    }
}
