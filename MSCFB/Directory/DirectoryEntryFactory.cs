using System;
using System.IO;
using MSCFB.Enum;
using System.Text;
using MSCFB.Static;

namespace MSCFB.Directory
{
    public static class DirectoryEntryFactory
    {
        
        public static DirectoryEntry LoadDirectoryEntry(CompoundFile compoundFile, StreamID streamId, DirectoryEntry parEntry = null)
        {
            MoveReaderToOffset(streamId, compoundFile);
            DirectoryEntry entry = new DirectoryEntry()
            {
                CompoundFile = compoundFile,
                ParentDirectoryEntry =parEntry,
                Name = new DirectoryEntryName(compoundFile.FileReader.ReadBytes(64), BitConverter.ToUInt16(compoundFile.FileReader.ReadBytes(2), 0)),
                ObjectType = (DirectoryEntryObjectType)compoundFile.FileReader.ReadByte(),
                ColorFlag = (ColorFlag)compoundFile.FileReader.ReadByte(),
                LeftSiblingID = (StreamID)BitConverter.ToUInt32(compoundFile.FileReader.ReadBytes(4), 0),
                RightSiblingID = (StreamID)BitConverter.ToUInt32(compoundFile.FileReader.ReadBytes(4), 0),
                ChildID = (StreamID)BitConverter.ToUInt32(compoundFile.FileReader.ReadBytes(4), 0),
                CLSID = compoundFile.FileReader.ReadBytes(16),
                StateBits = compoundFile.FileReader.ReadBytes(4),
                CreationTime = compoundFile.FileReader.ReadBytes(8),
                ModifiedTime = compoundFile.FileReader.ReadBytes(8),
                StartingSectorLocation = BitConverter.ToUInt32(compoundFile.FileReader.ReadBytes(4), 0),
                StreamSize = BitConverter.ToUInt64(compoundFile.FileReader.ReadBytes(8), 0)
            };
            return entry;
        }


        //public static IDirectoryEntry ChildFromStream(IDirectoryEntry Parent, StreamID streamId)
        //{
        //    MoveReaderToOffset(streamId, Parent.CompoundFile);
        //    Parent.CompoundFile.Seek(66, SeekOrigin.Current);
        //    DirectoryEntryObjectType objectType = Parent.CompoundFile.FileReader.ReadDirectoryEntryObjectType();
        //    if(objectType==DirectoryEntryObjectType.StorageObject)
        //    {
        //        StorageEntry entry = new StorageEntry(Parent, streamId)
        //        {
                   
        //        Name = new DirectoryEntryName(CompoundFile.FileReader.ReadBytes(64), BitConverter.ToUInt16(CompoundFile.FileReader.ReadBytes(2), 0));
        //        ObjectType = (DirectoryEntryObjectType)CompoundFile.FileReader.ReadByte();
        //        ColorFlag = (ColorFlag)CompoundFile.FileReader.ReadByte();
        //        LeftSiblingID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
        //        RightSiblingID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
        //        ChildID = (StreamID)BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
        //        CLSID = CompoundFile.FileReader.ReadBytes(16);
        //        StateBits = CompoundFile.FileReader.ReadBytes(4);
        //        CreationTime = CompoundFile.FileReader.ReadBytes(8);
        //        ModifiedTime = CompoundFile.FileReader.ReadBytes(8);
        //        StartingSectorLocation = BitConverter.ToUInt32(CompoundFile.FileReader.ReadBytes(4), 0);
        //        StreamSize = BitConverter.ToUInt64(CompoundFile.FileReader.ReadBytes(8), 0);
        //        if (LeftSiblingID != StreamID.NoStream)
        //            LeftSiblingDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, LeftSiblingID);
        //        if (RightSiblingID != StreamID.NoStream)
        //            RightSiblingDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, RightSiblingID);
        //        if (ChildID != StreamID.NoStream)
        //            ChildDirectoryEntry = DirectoryEntryFactory.ChildFromStream(this, ChildID);
        //        }
        //        return entry;
                
        //    }
        //    else if (objectType == DirectoryEntryObjectType.StreamObject)
        //    {
        //        StreamEntry entry = new StreamEntry(Parent, streamId);
        //        return entry;
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException($"Factory can only create {DirectoryEntryObjectType.StorageObject} or{DirectoryEntryObjectType.StreamObject} not {objectType}");
        //    }
            

        //}
        
        private static void MoveReaderToOffset(StreamID streamId, CompoundFile compoundFile)
        {
            var StreamIdLong = (long)streamId;
            var NextSector = compoundFile.Header.FirstDirectorySectorLocation;
            if (StreamIdLong == 0)
            {
                compoundFile.MoveReaderToSector(NextSector);

            }
            else
            {
                var divided = StreamIdLong / compoundFile.Header.DirectoryEntriesInSector;
                var remainder = StreamIdLong % compoundFile.Header.DirectoryEntriesInSector;
                long i = 0;
                while (NextSector <= SectorType.MaxRegSect)
                {

                    if (i == divided && NextSector <= SectorType.MaxRegSect)
                    {
                        compoundFile.MoveReaderToSector(NextSector);
                        compoundFile.Seek(remainder * 128, SeekOrigin.Current);
                        return;
                    }
                    else
                    {
                        i++;
                        NextSector = compoundFile.FatChain[NextSector];
                    }



                }
                throw new IndexOutOfRangeException("Invalid Stream ID");
            }


        }
    }
}