using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSCFB.Chains;
using MSCFB.Enum;
namespace MSCFB
{
    public class CompoundFile
    {
        public CompoundFileHeader Header { get; private set; }
        private Stream FileStream { get; set; }
        public BinaryReader FileReader { get; private set; }
        public BinaryWriter FileWriter { get; private set; }
        public FatChain FatChain { get; private set; }
        public DifatChain DifatChain { get; private set; }
        public DirectoryChain DirectoryChain { get; private set; }
        public DirectoryEntry RootDirectoryEntry { get; private set; }
        public bool CanWrite { get; private set; }
        public MiniFatChain MiniFatChain { get; private set; }
        public CompoundFile(Stream fileStream)
        {
            FileStream = fileStream;
            FileStream.Seek(0, SeekOrigin.Begin);
            FileReader = new BinaryReader(FileStream);
            if (FileStream.CanWrite)
            {
                FileWriter = new BinaryWriter(FileStream);
                CanWrite = true;
            }
            CanWrite = false;
            Load();
        }

        private void Load()
        {
            Header = new CompoundFileHeader(FileReader);
            DifatChain = new DifatChain(this);
            FatChain = new FatChain(this);
            DirectoryChain = new DirectoryChain(this);
            //MiniFatChain = new MiniFatChain(this);
            //RootDirectoryEntry = new DirectoryEntry(this, 0);
            return;
        }
        public void Seek(long offset, SeekOrigin origin)
        {
            FileStream.Seek(offset, origin);
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
        internal void MoveReaderToDirectoryEntry(uint index)
        {
            int Modulus;
            
            switch (Header.MajorVersion)
            {
                case (MajorVersion.Version3):
                {
                        Modulus =(int)index % 4;
                        MoveReaderToSector(DirectoryChain[index/4]);
                        FileReader.BaseStream.Seek(Modulus * 128, SeekOrigin.Current);
                        break;
                }
                default:
                {
                        Modulus = (int)index % 16;
                        MoveReaderToSector(DirectoryChain[index / 16]);
                        FileReader.BaseStream.Seek(Modulus * 128, SeekOrigin.Current);
                        break;
                }
            }

        }
        internal uint ReadNthUintFromSector(uint n, SectorType sector)
        {
            if(n*4>=Header.SectorSize)
                throw new IndexOutOfRangeException();
            MoveReaderToSector((uint)sector);
            FileStream.Seek(4 * n, SeekOrigin.Current);
            return FileReader.ReadUInt32();
        }
        internal byte[] ReadDirectoryEntry(uint index)
        {
            MoveReaderToDirectoryEntry(index);
            return FileReader.ReadBytes(128);
        }
    }
}
