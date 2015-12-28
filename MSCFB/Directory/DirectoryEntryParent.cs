using System;
using System.IO;
using System.Text;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    public class DirectoryEntryParent : IDirectoryEntry
    {
        
        public DirectoryEntry DirectoryEntryTreeParentNode { get; set; }
        public StreamID StreamId { get;  set; }
        public long StreamIdLong => (long) StreamId;
        public uint StartingSectorLocation { get;  set; }
        public ulong StreamSize { get;  set; }
        public byte[] ModifiedTime { get;  set; }
        public byte[] CreationTime { get;  set; }
        public byte[] StateBits { get; set; }
        public byte[] CLSID { get;  set; }
        public StreamID ChildID { get;  set; }
        public StreamID LeftSiblingID { get;  set; }
        public StreamID RightSiblingID { get;  set; }
        public DirectoryEntry LeftSiblingDirectoryEntry { get; set; }
        public DirectoryEntry RightSiblingDirectoryEntry { get; set; }
        public CompoundFile CompoundFile { get;  set; }
        public RedBlackDirectoryTree ChildRedBlackDirectoryTree { get; set; }
        public ColorFlag ColorFlag { get; set; }
        public DirectoryEntryObjectType ObjectType { get;  set; }
        public DirectoryEntryName Name { get; set; }
        public DirectoryEntry ParentDirectoryEntry { get; set; }

       
        public DirectoryEntryParent()
        {
            
        }
        

        protected void LoadDirectoryEntry()
        {
            MoveReaderToOffset();
            Name = new DirectoryEntryName(CompoundFile.FileReader.ReadBytes(64), BitConverter.ToUInt16(CompoundFile.FileReader.ReadBytes(2), 0));
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
            //if (LeftSiblingID != StreamID.NoStream)
            //    LeftSiblingDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, LeftSiblingID);
            //if (RightSiblingID != StreamID.NoStream)
            //    RightSiblingDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, RightSiblingID);
            //if (ChildID != StreamID.NoStream)
            //    ChildDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, ChildID);
            
        }
        
        
        

        #region compare
        public int CompareTo(IDirectoryEntry other)
        {
            
           
            if (this.Name.Length < other.Name.Length)
                return -1;
            else if (this.Name.Length > other.Name.Length)
                return 1;
            else
            {
                Char a, b;
                for (int i = 0; i < this.Name.Length; i++)
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
        #endregion
        //public void Insert
        

        public override string ToString()
        {
            return $"{this.Name}";
        }
        public void AddEntry()
        {
            if(this.ObjectType!=DirectoryEntryObjectType.StorageObject || this.ObjectType!=DirectoryEntryObjectType.RootStorageObject)
                return;
            else
            {
                
            }
        }
        public int CompareTo(string other)
        {
            if (this.Name.Length < other.Length)
                return -1;
            else if (this.Name.Length > other.Length)
                return 1;
            else
            {
                Char a, b;
                for (int i = 0; i < this.Name.Length; i++)
                {
                    a = Char.ToUpper(this.Name[i]);
                    b = Char.ToUpper(other[i]);
                    if (a == b)
                        continue;
                    else
                    {
                        return a.CompareTo(b);
                    }

                }
                return 0;
            }
        }
    }
}
