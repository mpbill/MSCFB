using System;
using System.IO;
using System.Text;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    public class DirectoryEntryParent : IDirectoryEntry
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
        public IDirectoryEntry LeftSiblingDirectoryEntry { get; set; }
        public IDirectoryEntry RightSiblingDirectoryEntry { get; set; }
        public CompoundFile CompoundFile { get; private set; }
        public IDirectoryEntry ChildDirectoryEntry { get; set; }
        public ColorFlag ColorFlag { get; private set; }
        public DirectoryEntryObjectType ObjectType { get; private set; }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value.Length > 31)
                {
                    throw new InvalidOperationException("Name cannot be more than 31 characters.");
                }
                foreach (string illegal in Resources.Illegal)
                {
                    if(value.Contains(illegal))
                        throw new InvalidOperationException($"String Cannot contain '{illegal}'");
                }
                name = value;
                
            }
        }
        public ushort NameLength { get; private set; }
        public IDirectoryEntry ParentDirectoryEntry { get; set; }

       

        public DirectoryEntryParent(CompoundFile compoundFile, StreamID streamId)
        {
            StreamId = streamId;
            
            CompoundFile = compoundFile;
            LoadDirectoryEntry();
            
        }
        public DirectoryEntryParent(DirectoryEntryObjectType type, string name, IDirectoryEntry Parent)
        {
            this.Name = name;
        }
        public DirectoryEntryParent(IDirectoryEntry Parent, StreamID streamId)
        {
            ParentDirectoryEntry = Parent;
            this.CompoundFile = Parent.CompoundFile;
            this.StreamId = streamId;
            LoadDirectoryEntry();
        }

        private void LoadDirectoryEntry()
        {
            MoveReaderToOffset();
            var nbytes = CompoundFile.FileReader.ReadBytes(64);
            NameLength = BitConverter.ToUInt16(CompoundFile.FileReader.ReadBytes(2), 0);
            Name = Encoding.Unicode.GetString(nbytes, 0, NameLength - 2);
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
                LeftSiblingDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, LeftSiblingID);
            if (RightSiblingID != StreamID.NoStream)
                RightSiblingDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, RightSiblingID);
            if (ChildID != StreamID.NoStream)
                ChildDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, ChildID);
            
        }
        
        
        

        #region compare
        public int CompareTo(IDirectoryEntry other)
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
                    a = Char.ToUpper(this.Name[i]);
                    b = Char.ToUpper(other.Name[i]);
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
        //public void AddLeaf(DirectoryEntryParent Entry) { }
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


        public static bool operator < (DirectoryEntryParent operand1, DirectoryEntryParent operand2)
        {
            return operand1 != null && operand1.CompareTo(operand2) < 0;
        }
        public static bool operator >(DirectoryEntryParent operand1, DirectoryEntryParent operand2)
        {
            return operand1 != null && operand1.CompareTo(operand2) > 0;
        }
        public static bool operator ==(DirectoryEntryParent operand1, DirectoryEntryParent operand2)
        {
            if (ReferenceEquals(null, operand1))
                return ReferenceEquals(null, operand2);
            if (ReferenceEquals(operand2, null))
                return false;
            return operand1.CompareTo(operand2) == 0;
        }
        public static bool operator !=(DirectoryEntryParent operand1, DirectoryEntryParent operand2)
        {
            return !(operand1 == operand2);
        }
        public static bool operator <=(DirectoryEntryParent operand1, DirectoryEntryParent operand2)
        {
            return ((operand1 < operand2) || (operand1 == operand2));
        }
        public static bool operator >=(DirectoryEntryParent operand1, DirectoryEntryParent operand2)
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
