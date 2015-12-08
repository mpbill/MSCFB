using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class DirectoryEntry
    {
        public ushort NameLength { get; private set; }
        public byte[] DirectoryEntryBytes { get; private set; }
        public string Name { get; private set; }
        public DirectoryEntry(byte[] directoryEntryBytes)
        {
            this.DirectoryEntryBytes = directoryEntryBytes;
            BinaryReader br = new BinaryReader(new MemoryStream(DirectoryEntryBytes));
            Name = Encoding.Unicode.GetString(br.ReadBytes(64));
            NameLength = BitConverter.ToUInt16(br.ReadBytes(2), 0);

        }
    }
}
