using System;
using System.IO;
using System.Text;
using MSCFB.Enum;

namespace MSCFB.Chains
{
    public class DirectoryEntry : IComparable<DirectoryEntry>
    {
        public StreamID EntryID { get; private set; }
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
        
        public DirectoryEntry(CompoundFile compoundFile, StreamID entryID)
        {
            EntryID = entryID;
            CompoundFile = compoundFile;
            
            CompoundFile.Seek(CompoundFile.DirectoryChain[entryID], SeekOrigin.Begin);
            RawName = Encoding.Unicode.GetString(CompoundFile.FileReader.ReadBytes(64));
            NameLength = BitConverter.ToUInt16(CompoundFile.FileReader.ReadBytes(2), 0);
            ObjectType = (DirectoryEntryObjectType)CompoundFile.FileReader.ReadByte();
            ColorFlag = (ColorFlag)CompoundFile.FileReader.ReadByte();
            LeftSiblingID =(StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            RightSiblingID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            ChildID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            CLSID = CompoundFile.FileReader.ReadBytes(16);
            StateBits = CompoundFile.FileReader.ReadBytes(4);
            CreationTime = CompoundFile.FileReader.ReadBytes(8);
            ModifiedTime = CompoundFile.FileReader.ReadBytes(8);
            StartingSectorLocation = BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
            StreamSize = BitConverter.ToUInt64(CompoundFile.FileReader.ReadBytes(8), 0);
            if(LeftSiblingID!=StreamID.NoStream)
                LeftSiblingDirectoryEntry = new DirectoryEntry(CompoundFile, LeftSiblingID);
            if(RightSiblingID!=StreamID.NoStream)
                RightSiblingDirectoryEntry = new DirectoryEntry(CompoundFile, RightSiblingID);
            if(ChildID!=StreamID.NoStream)
                ChildDirectoryEntry = new DirectoryEntry(CompoundFile, ChildID);
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
        public static bool operator < (DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return operand1.CompareTo(operand2) < 0;
        }
        public static bool operator >(DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return operand1.CompareTo(operand2) > 0;
        }
        public static bool operator ==(DirectoryEntry operand1, DirectoryEntry operand2)
        {
            return operand1.CompareTo(operand2) == 0;
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
