using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var fs = File.OpenRead("empty.msg"))
            {
                BinaryReader br = new BinaryReader(fs);
                var watch = Stopwatch.StartNew();
                CompoundFileHeader header = new CompoundFileHeader(br);
                watch.Stop();
                return;
            }

        }
    }

    enum MajorVersion : ushort
    {
        Version3 = 0x0003,
        Version4 = 0x0004
    }

    enum SectorShift
    {
        Shift512 = 0x0009,
        Shift4096 = 0x000C
    }
    enum ByteOrder : ushort
    {
        LittleEndian = 0xFFFE
    }
    class CompoundFileHeader
    {
        private UInt32 NumberOfDirectorySectors { get; set; }
        private UInt32 NumberOfFatSectors { get; set; }
        private UInt32 FirstDirectorySectorLocation { get; set; }
        private UInt32 TransactionSignatureNumber { get; set; }

        private UInt32 MiniStreamCutoffSize
        {
            get { return _miniStreamCutoffSize; }
        }

        private UInt32 FirstMiniFatSectorLocation { get; set; }
        private UInt32 NumberOfMiniFatSectors { get; set; }
        private UInt32 FirstDifatSectorLocation { get; set; }
        private UInt32 NumberOfDifatSectors { get; set; }
        private byte[] DifatByteArray { get; set; }
        private UInt32[] DifatIntArray { get; set; }
        private ushort MiniSectorShift
        {
            get { return _miniSectorShift; }
        }

        private SectorShift SectorShift { get; set; }
        private byte[] SectorShiftBytes { get; set; }
        private ByteOrder ByteOrder
        {
            get { return _byteOrder; }
        }


        private MajorVersion MajorVersion { get; set; }
        private readonly byte[] _headerSignature = new byte[8] {0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1};
        private readonly byte[] _headerClsid = Enumerable.Repeat((byte)0x00, 16).ToArray();
        private readonly ByteOrder _byteOrder = ByteOrder.LittleEndian;
        private readonly ushort _miniSectorShift = 0x0006;
        private readonly uint _miniStreamCutoffSize = 0x00001000;

        public byte[] HeaderSignature
        {
            get { return _headerSignature; }
        }

        public byte[] HeaderClsid
        {
            get { return _headerClsid; }
        }
        
        private byte[] MajorVersionBits { get; set; }
        private byte[] MinorVersionBits { get; set; }
        private ushort MinorVersion { get; set; }
        public CompoundFileHeader(BinaryReader FileReader)
        {
            LoadHeader(FileReader);
        }
        private void LoadHeader(BinaryReader FileReader)
        {
            //Header Signature (8 bytes): Identification signature for the compound file structure, and MUST be set to the value 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1.
            if (!Enumerable.SequenceEqual(FileReader.ReadBytes(8), HeaderSignature))
                throw new InvalidMcdfHeaderException("Header signature section was not 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1.");
            //Header CLSID (16 bytes): Reserved and unused class ID that MUST be set to all zeroes(CLSID_NULL).
            if (!Enumerable.SequenceEqual(FileReader.ReadBytes(16), HeaderClsid))
                throw new InvalidMcdfHeaderException("CLSID section was not all zeroes.");
            //Minor Version (2 bytes): Version number for nonbreaking changes. This field SHOULD be set to 0x003E if the major version field is either 0x0003 or 0x0004.
            MinorVersionBits = FileReader.ReadBytes(2);
            //Major Version (2 bytes): Version number for breaking changes. This field MUST be set to either 0x0003 (version 3) or 0x0004 (version 4).
            MajorVersionBits = FileReader.ReadBytes(2);
            ushort majorVersionUshort = BitConverter.ToUInt16(MajorVersionBits, 0);
            if(!(majorVersionUshort==0x0003 | majorVersionUshort==0x0004))
                throw new InvalidMcdfHeaderException("Major Version was not 0x0003 or 0x0004");
            MajorVersion = (MajorVersion)majorVersionUshort;
            MinorVersion = BitConverter.ToUInt16(MinorVersionBits, 0);
            //Byte Order (2 bytes): This field MUST be set to 0xFFFE. This field is a byte order mark for all integer fields, specifying little-endian byte order.
            if (BitConverter.ToUInt16(FileReader.ReadBytes(2), 0)!=(ushort)ByteOrder.LittleEndian)
                throw new InvalidMcdfHeaderException("Byte order section was not set to 0xFFFE");
            /*
            Sector Shift (2 bytes): This field MUST be set to 0x0009, or 0x000c, depending on the Major Version field. This field specifies the sector size of the compound file as a power of 2.
                - If Major Version is 3, the Sector Shift MUST be 0x0009, specifying a sector size of 512 bytes.
                - If Major Version is 4, the Sector Shift MUST be 0x000C, specifying a sector size of 4096 bytes.
            */
            SectorShiftBytes = FileReader.ReadBytes(2);
            switch (BitConverter.ToUInt16(SectorShiftBytes, 0))
            {
                case 0x0009:
                {
                    if(MajorVersion!=MajorVersion.Version3)
                            throw new InvalidMcdfHeaderException("Sector shift section was 0x0009 but Major Version was not 0x003");
                        SectorShift = SectorShift.Shift512;
                    break;
                }
                case 0x000C:
                {
                    if(MajorVersion!=MajorVersion.Version4)
                            throw new InvalidMcdfHeaderException("Sector shift section was 0x000C but Major Version was not 0x004");
                    SectorShift = SectorShift.Shift4096;
                    break;
                }
                default:
                {
                    throw new InvalidMcdfHeaderException("Sector Shift Section was not 0x0009 or 0x000C.");
                    
                }
            }
            //Mini Sector Shift (2 bytes): This field MUST be set to 0x0006. This field specifies the sector size of the Mini Stream as a power of 2. The sector size of the Mini Stream MUST be 64 bytes.
            if (BitConverter.ToUInt16(FileReader.ReadBytes(2), 0) != MiniSectorShift)
                throw new InvalidMcdfHeaderException("Mini Sector Shift Section was not set to 0x0006.");
            //Reserved (6 bytes): This field MUST be set to all zeroes.
            if (!Enumerable.SequenceEqual(FileReader.ReadBytes(6), Resources.HeaderSectionReserved))
                throw new InvalidMcdfHeaderException("Reserved Section was not set to all zeroes.");
            NumberOfDirectorySectors = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            if(MajorVersion==MajorVersion.Version3 && NumberOfDirectorySectors!=0)
                throw new InvalidMcdfHeaderException("Major Version 3 does not support NumberOfDirectorySectors field.");
            NumberOfFatSectors = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            FirstDirectorySectorLocation = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            TransactionSignatureNumber = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            if(MiniStreamCutoffSize != BitConverter.ToUInt32(FileReader.ReadBytes(4), 0))
                throw new InvalidMcdfHeaderException("MiniStreamCutofSize Section was not equal to 0x00001000.");
            FirstMiniFatSectorLocation = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            NumberOfMiniFatSectors = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            FirstDifatSectorLocation = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            var bts = FileReader.ReadBytes(4);
            NumberOfDifatSectors = BitConverter.ToUInt32(bts, 0);
            DifatByteArray = FileReader.ReadBytes(436);
            DifatIntArray = new UInt32[109];
            

            for (int i = 0; i < 109; i++)
            {
                DifatIntArray[i] = BitConverter.ToUInt32(DifatByteArray.Skip(i).Take(4).ToArray(),0);
            }
            if (MajorVersion == MajorVersion.Version4 && Enumerable.SequenceEqual(Resources.RemainingBytes0, FileReader.ReadBytes(3584)))
                throw new InvalidMcdfHeaderException("Version 4 Header Must be followed by 3584 0x00 bytes");
                
            return;

        }
    }
    class CompoundFile
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
        }

        
    }

    static class Resources
    {
        private static readonly byte[] _remainingbytes0 = Enumerable.Repeat((byte)0x00, 3584).ToArray();
        public static byte[] RemainingBytes0 { get { return _remainingbytes0;} }
        private static readonly byte[] _headerSectionReserved = Enumerable.Repeat((byte) 0x00, 6).ToArray();
        public static byte[] HeaderSectionReserved { get { return _headerSectionReserved;} }
        private static readonly byte[] _mcdfHeaderSigniture = new byte[8]
        {
            0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1
        };

        private static readonly byte[] _mcdfHeaderClsid = Enumerable.Repeat((byte) 0x00, 16).ToArray();

        public static byte[] McdfHeaderSigniture
        {
            get { return _mcdfHeaderSigniture; }
            
        }

        public static byte[] McdfHeaderClsid
        {
            get { return _mcdfHeaderClsid; }
        }

    }

    class InvalidMcdfHeaderException : Exception
    {
        public InvalidMcdfHeaderException()
        {
            
        }

        public InvalidMcdfHeaderException(string message):base(message)
        {
            
        }

    }
    class InvalidMcdfHeaderSigniture : Exception
    {
        
    }

    class InvalidMcdfHeaderClsid : Exception
    {
        
    }

    class IncorrectMajorVersion : Exception
    {
        
    }

    class IncorrectByteOrder : Exception
    {
        
    }

    class IncorrectSectorShift : Exception
    {
        
    }

    class IncorrectMiniSectorShift : Exception
    {
        
    }
    
}
