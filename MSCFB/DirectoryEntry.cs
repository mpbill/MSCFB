using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    public class DirectoryEntry
    {
        public uint StartingSectorLocation { get; private set; }
        public ulong StreamSize { get; private set; }


        public byte[] ModifiedTime { get; private set; }
        public byte[] CreationTime { get; private set; }
        public byte[] StateBits { get; private set; }
        public byte[] CLSID { get; private set; }
        public uint ChildID { get; private set; }
        public uint LeftSiblingID { get; private set; }
        public uint RightSiblingID { get; private set; }
        public ColorFlag ColorFlag { get; private set; }
        public DirectoryEntryObjectType ObjectType { get; private set; }
        public byte[] DirectoryEntryBytes { get; private set; }
        public string Name { get; private set; }
        public ushort NameLength { get; private set; }
        public DirectoryEntry(byte[] directoryEntryBytes)
        {
            this.DirectoryEntryBytes = directoryEntryBytes;
            BinaryReader br = new BinaryReader(new MemoryStream(DirectoryEntryBytes));
            Name = Encoding.Unicode.GetString(br.ReadBytes(64));
            NameLength = BitConverter.ToUInt16(br.ReadBytes(2), 0);
            ObjectType = (DirectoryEntryObjectType)br.ReadByte();
            ColorFlag = (ColorFlag)br.ReadByte();
            LeftSiblingID = BitConverter.ToUInt32(br.ReadBytes(4), 0);
            RightSiblingID = BitConverter.ToUInt32(br.ReadBytes(4), 0);
            ChildID = BitConverter.ToUInt32(br.ReadBytes(4), 0);
            CLSID = br.ReadBytes(16);
            StateBits = br.ReadBytes(4);
            CreationTime = br.ReadBytes(8);
            ModifiedTime = br.ReadBytes(8);
            StartingSectorLocation = BitConverter.ToUInt32(br.ReadBytes(4), 0);
            StreamSize = BitConverter.ToUInt64(br.ReadBytes(8), 0);
        }
    }
}
