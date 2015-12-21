using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSCFB.Enum;
using MSCFB.Exception;

namespace MSCFB
{
    public class CompoundFileHeader
    {

        /// <summary>
        /// Number of Directory Sectors (4 bytes): This integer field contains the count of the number of directory sectors in the compound file.
        /// </summary>
        public UInt32 NumberOfDirectorySectors { get; private set; }
        /// <summary>
        /// Number of FAT Sectors (4 bytes): This integer field contains the count of the number of FAT sectors in the compound file.
        /// </summary>
        public UInt32 NumberOfFatSectors { get; private set; }
        /// <summary>
        /// First Directory Sector Location (4 bytes): This integer field contains the starting sector number for the directory stream.
        /// </summary>
        public SectorType FirstDirectorySectorLocation { get; private set; }
        /// <summary>
        /// Transaction Signature Number (4 bytes): 
        /// This integer field MAY contain a sequence number that is incremented every time the compound file is saved by an implementation that supports file transactions.
        /// This is the field that MUST be set to all zeroes if file transactions are not implemented.
        /// </summary>
        public UInt32 TransactionSignatureNumber { get; private set; }
        /// <summary>
        /// Mini Stream Cutoff Size (4 bytes): This integer field MUST be set to 0x00001000. 
        /// This field specifies the maximum size of a user-defined data stream that is allocated from the mini FAT and mini stream, and that cutoff is 4,096 bytes.
        /// Any user-defined data stream that is larger than or equal to this cutoff size must be allocated as normal sectors from the FAT.
        /// </summary>
        public UInt32 MiniStreamCutoffSize
        {
            get { return _miniStreamCutoffSize; }
        }
        /// <summary>
        /// First Mini FAT Sector Location (4 bytes): This integer field contains the starting sector number for the mini FAT.
        /// </summary>
        public SectorType FirstMiniFatSectorLocation { get; private set; }
        /// <summary>
        /// Number of Mini FAT Sectors (4 bytes): This integer field contains the count of the number of mini FAT sectors in the compound file.
        /// </summary>
        public UInt32 NumberOfMiniFatSectors { get; private set; }
        /// <summary>
        /// First DIFAT Sector Location (4 bytes): This integer field contains the starting sector number for the DIFAT.
        /// </summary>
        public SectorType FirstDifatSectorLocation { get; private set; }
        /// <summary>
        /// Number of DIFAT Sectors (4 bytes): This integer field contains the count of the number of DIFAT sectors in the compound file.
        /// </summary>
        public UInt32 NumberOfDifatSectors { get; private set; }
        /// <summary>
        /// DIFAT (436 bytes): This array of 32-bit integer fields contains the first 109 FAT sector locations of the compound file.
        /// </summary>
        /// <summary>
        /// DIFAT (436 bytes): This array of 32-bit integer fields contains the first 109 FAT sector locations of the compound file. Calculated from <see cref="DifatByteArray"/>.
        /// </summary>
        public List<SectorType> DifatArray { get; private set; }
        /// <summary>
        /// Mini Sector Shift (2 bytes): This field MUST be set to 0x0006. This field specifies the sector size of the Mini Stream as a power of 2. The sector size of the Mini Stream MUST be 64 bytes.
        /// </summary>
        public ushort MiniSectorShift
        {
            get { return _miniSectorShift; }
        }
        /// <summary>
        /// Sector Shift (2 bytes): This field MUST be set to 0x0009, or 0x000c, depending on the Major Version field. This field specifies the sector size of the compound file as a power of 2.
        ///     - If Major Version is 3, the Sector Shift MUST be 0x0009, specifying a sector size of 512 bytes.
        ///     - If Major Version is 4, the Sector Shift MUST be 0x000C, specifying a sector size of 4096 bytes.
        /// </summary>
        public SectorShift SectorShift { get; private set; }
        /// <summary>
        /// Byte Order (2 bytes): This field MUST be set to 0xFFFE. This field is a byte order mark for all integer fields, specifying little-endian byte order.
        /// </summary>
        public ByteOrder ByteOrder
        {
            get { return _byteOrder; }
        }

        public uint SectorSize { get; private set; }

        /// <summary>
        /// Major Version (2 bytes): Version number for breaking changes. This field MUST be set to either 0x0003 (version 3) or 0x0004 (version 4).
        /// </summary>
        public MajorVersion MajorVersion { get; private set; }
        private readonly byte[] _headerSignature = new byte[8] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        private readonly ByteOrder _byteOrder = ByteOrder.LittleEndian;
        private readonly ushort _miniSectorShift = 0x0006;
        private readonly uint _miniStreamCutoffSize = 0x00001000;
        /// <summary>
        /// For version 4 compound files, the header size (512 bytes) is less than the sector size (4,096 bytes),
        /// so the remaining part of the header (3,584 bytes) MUST be filled with all zeroes.
        /// </summary>

        /// <summary>
        /// Header Signature (8 bytes): Identification signature for the compound file structure, and MUST be set to the value 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1.
        /// </summary>
        public byte[] HeaderSignature
        {
            get { return _headerSignature; }
        }

        private byte[] MinorVersionBits { get; set; }
        /// <summary>
        /// Minor Version (2 bytes): Version number for nonbreaking changes. This field SHOULD be set to 0x003E if the major version field is either 0x0003 or 0x0004. 
        /// </summary>
        public ushort MinorVersion { get; private set; }
        /// <summary>
        /// TRUE if <see cref="MinorVersion"/> =  0x003E.
        /// </summary>
        public CompoundFileHeader(BinaryReader FileReader)
        {
            DifatArray = new List<SectorType>();
            LoadHeader(FileReader);
        }
        
        private void LoadHeader(BinaryReader FileReader)
        {
            //Header Signature (8 bytes): Identification signature for the compound file structure, and MUST be set to the value 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1.
            if (!Enumerable.SequenceEqual(FileReader.ReadBytes(8), HeaderSignature))
                throw new InvalidMcdfHeaderException("Header signature section was not 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1.");
            //Header CLSID (16 bytes): Reserved and unused class ID that MUST be set to all zeroes(CLSID_NULL).
            if (!Enumerable.SequenceEqual(FileReader.ReadBytes(16), Resources.McdfHeaderClsid))
                throw new InvalidMcdfHeaderException("CLSID section was not all zeroes.");
            //Minor Version (2 bytes): Version number for nonbreaking changes. This field SHOULD be set to 0x003E if the major version field is either 0x0003 or 0x0004.
            MinorVersion = BitConverter.ToUInt16(FileReader.ReadBytes(2), 0);
            //Major Version (2 bytes): Version number for breaking changes. This field MUST be set to either 0x0003 (version 3) or 0x0004 (version 4).
            MajorVersion = (MajorVersion)BitConverter.ToUInt16(FileReader.ReadBytes(2), 0);
            
            //Byte Order (2 bytes): This field MUST be set to 0xFFFE. This field is a byte order mark for all integer fields, specifying little-endian byte order.
            if (BitConverter.ToUInt16(FileReader.ReadBytes(2), 0) != (ushort)ByteOrder.LittleEndian)
                throw new InvalidMcdfHeaderException("Byte order section was not set to 0xFFFE");
            /*
            Sector Shift (2 bytes): This field MUST be set to 0x0009, or 0x000c, depending on the Major Version field. This field specifies the sector size of the compound file as a power of 2.
                - If Major Version is 3, the Sector Shift MUST be 0x0009, specifying a sector size of 512 bytes.
                - If Major Version is 4, the Sector Shift MUST be 0x000C, specifying a sector size of 4096 bytes.
            */
            SectorShift = (SectorShift)BitConverter.ToUInt16(FileReader.ReadBytes(2), 0);
            switch (SectorShift)
            {
                case SectorShift.Shift512:
                    {
                        if (MajorVersion != MajorVersion.Version3)
                            throw new InvalidMcdfHeaderException("Sector shift section was 0x0009 but Major Version was not 0x003");
                        SectorSize = 512;
                        break;
                    }
                case SectorShift.Shift4096:
                    {
                        if (MajorVersion != MajorVersion.Version4)
                            throw new InvalidMcdfHeaderException("Sector shift section was 0x000C but Major Version was not 0x004");
                        SectorSize = 4096;
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
            var t = FileReader.ReadBytes(4);
            NumberOfDirectorySectors = BitConverter.ToUInt32(t, 0);
            if (MajorVersion == MajorVersion.Version3 && NumberOfDirectorySectors != 0)
                throw new InvalidMcdfHeaderException("Major Version 3 does not support NumberOfDirectorySectors field.");
            NumberOfFatSectors = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            FirstDirectorySectorLocation =(SectorType)BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            TransactionSignatureNumber = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            if (MiniStreamCutoffSize != BitConverter.ToUInt32(FileReader.ReadBytes(4), 0))
                throw new InvalidMcdfHeaderException("MiniStreamCutofSize Section was not equal to 0x00001000.");
            FirstMiniFatSectorLocation = (SectorType)BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            NumberOfMiniFatSectors = BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            FirstDifatSectorLocation = (SectorType)BitConverter.ToUInt32(FileReader.ReadBytes(4), 0);
            var bts = FileReader.ReadBytes(4);
            NumberOfDifatSectors = BitConverter.ToUInt32(bts, 0);
            var DifatBytes = FileReader.ReadBytes(436);
            for (int i = 0; i < 109; i++)
            {
                DifatArray.Add((SectorType)BitConverter.ToUInt32(DifatBytes.Skip(i*4).Take(4).ToArray(), 0));
            }
            if (MajorVersion == MajorVersion.Version4 && Enumerable.SequenceEqual(Resources.RemainingBytes0, FileReader.ReadBytes(3584)))
                throw new InvalidMcdfHeaderException("Version 4 Header Must be followed by 3584 0x00 bytes");

            return;

        }
    }
}
