using System;
using System.IO;
using System.Text;
using MSCFB.Enum;

namespace MSCFB.Chains
{
    public class DirectoryEntry : IComparable<DirectoryEntry>
    {
        
        public StreamID StreamId { get; private set; }
        public long StreamIdLong => (long) StreamId;
        public uint StartingSectorLocation { get; private set; }
        public ulong StreamSize { get; private set; }
        public byte[] ModifiedTime { get; private set; }
        public byte[] CreationTime { get; private set; }
        public byte[] StateBits { get; private set; }
        public byte[] CLSID { get; private set; }
        public StreamID ChildID { get; private set; }
        public StreamID LeftSiblingID { get; private set; }
        public StreamID RightSiblingID { get; private set; }
        public DirectoryEntry LeftSiblingDirectoryEntry { get; set; }
        public DirectoryEntry RightSiblingDirectoryEntry { get; set; }
        public CompoundFile CompoundFile { get; private set; }
        
        public DirectoryEntry ChildDirectoryEntry { get; set; }
        public ColorFlag ColorFlag { get; private set; }
        public DirectoryEntryObjectType ObjectType { get; private set; }
        
        public string RawName { get; private set; }
        public string Name { get { return RawName.TrimEnd(new char[1] {'\0'}); } }
        public ushort NameLength { get; private set; }
        public DirectoryEntry ParentDirectoryEntry { get; private set; }
        public DirectoryEntry(CompoundFile compoundFile, StreamID streamId)
        {
            StreamId = streamId;
            CompoundFile = compoundFile;
            LoadDirectoryEntry();
            
        }
        
        public DirectoryEntry(DirectoryEntry Parent, StreamID streamId)
        {
            ParentDirectoryEntry = Parent;
            this.CompoundFile = Parent.CompoundFile;
            this.StreamId = streamId;
            LoadDirectoryEntry();
        }

        private void LoadDirectoryEntry()
        {
            MoveReaderToOffset();
            RawName = Encoding.Unicode.GetString(CompoundFile.FileReader.ReadBytes(64));
            NameLength = BitConverter.ToUInt16(CompoundFile.FileReader.ReadBytes(2), 0);
            ObjectType = (DirectoryEntryObjectType)CompoundFile.FileReader.ReadByte();
            ColorFlag = (ColorFlag)CompoundFile.FileReader.ReadByte();
            LeftSiblingID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            RightSiblingID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            ChildID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            CLSID = CompoundFile.FileReader.ReadBytes(16);
            StateBits = CompoundFile.FileReader.ReadBytes(4);
            CreationTime = CompoundFile.FileReader.ReadBytes(8);
            ModifiedTime = CompoundFile.FileReader.ReadBytes(8);
            StartingSectorLocation = BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            StreamSize = BitConverter.ToUInt64(CompoundFile.FileReader.ReadBytes(8), 0);
            if (LeftSiblingID != StreamID.NoStream)
                LeftSiblingDirectoryEntry = new DirectoryEntry(this, LeftSiblingID);
            if (RightSiblingID != StreamID.NoStream)
                RightSiblingDirectoryEntry = new DirectoryEntry(this, RightSiblingID);
            if (ChildID != StreamID.NoStream)
                ChildDirectoryEntry = new DirectoryEntry(this, ChildID);
            
        }
        #region compare
        public int CompareTo(DirectoryEntry other)
        {
           
            if (this.NameLength < other.NameLength)
                return -1;
            else if (this.NameLength > other.NameLength)
                return 1;
            else
            {
                Char a, b;
                for (int i = 0; i < this.NameLength; i++)
                {
                    a = Char.ToUpper(this.RawName[i]);
                    b = Char.ToUpper(other.RawName[i]);
                    if(a==b)
                        continue;
                    else
                    {
                        return a.CompareTo(b);
                    }

                }
                return 0;
            }
        }

        private void MoveReaderToOffset()
        {
            var NextSector = CompoundFile.Header.FirstDirectorySectorLocation;
            if (this.StreamIdLong==0)
            {
                CompoundFile.MoveReaderToSector(NextSector);
                
            }
            else
            {
                var divided = StreamIdLong/CompoundFile.Header.DirectoryEntriesInSector;
                var remainder = StreamIdLong%CompoundFile.Header.DirectoryEntriesInSector;
                long i = 0;
                while (NextSector<=SectorType.MaxRegSect)
                {
                    
                    if (i == divided && NextSector<=SectorType.MaxRegSect)
                    {
                        CompoundFile.MoveReaderToSector(NextSector);
                        CompoundFile.Seek(remainder*128, SeekOrigin.Current);
                        return;
                    }
                    else
                    {
                        i++;
                        NextSector = CompoundFile.FatChain[NextSector];
                    }
                    


                }
                throw new IndexOutOfRangeException("Invalid Stream ID");
            }


        }
        public static bool operator < (DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return operand1 != null && operand1.CompareTo(operand2) < 0;
        }
        public static bool operator >(DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return operand1 != null && operand1.CompareTo(operand2) > 0;
        }
        public static bool operator ==(DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return operand1 != null && operand1.CompareTo(operand2) == 0;
        }
        public static bool operator !=(DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return !(operand1 == operand2);
        }
        public static bool operator <=(DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return ((operand1 < operand2) || (operand1 == operand2));
        }
        public static bool operator >=(DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return ((operand1 >= operand2) || (operand1 == operand2));
        }
        #endregion
        //public void Insert
        public override string ToString()
        {
            return $"[{this.ObjectType}][{this.Name}]";
        }
        public void AddEntry()
        {
            if(this.ObjectType!=DirectoryEntryObjectType.StorageObject || this.ObjectType!=DirectoryEntryObjectType.RootStorageObject)
                return;
            else
            {
                
            }
        }
        
    }
}
