using System;
using System.IO;
using MSCFB.Enum;
using MSCFB.Static;

namespace MSCFB.Directory
{
    //public class DirectoryTree
    //{
    //    public CompoundFile CompoundFile { get; set; }
    //    public RootDirectoryEntry RootDirectoryEntry { get; private set; }
    //    public DirectoryTree(CompoundFile compoundFile)
    //    {
    //        CompoundFile = compoundFile;
    //        RootDirectoryEntry = new RootDirectoryEntry(CompoundFile);

    //    }
    //    public static IDirectoryEntry ChildFromStream(IDirectoryEntry Parent, StreamID streamId)
    //    {
    //        MoveReaderToOffset(streamId, Parent);
    //        Parent.CompoundFile.Seek(66, SeekOrigin.Current);
    //        DirectoryEntryObjectType objectType = Parent.CompoundFile.FileReader.ReadDirectoryEntryObjectType();
    //        if (objectType == DirectoryEntryObjectType.StorageObject)
    //        {
    //            StorageEntry entry = new StorageEntry(Parent, streamId);
    //            return entry;

    //        }
    //        else if (objectType == DirectoryEntryObjectType.StreamObject)
    //        {
    //            StreamEntry entry = new StreamEntry(Parent, streamId);
    //            return entry;
    //        }
    //        else
    //        {
    //            throw new InvalidOperationException($"Factory can only create {DirectoryEntryObjectType.StorageObject} or{DirectoryEntryObjectType.StreamObject} not {objectType}");
    //        }


    //    }
    //    private static void MoveReaderToOffset(StreamID streamId, IDirectoryEntry Parent)
    //    {
    //        var StreamIdLong = (long)streamId;
    //        var NextSector = Parent.CompoundFile.Header.FirstDirectorySectorLocation;
    //        if (StreamIdLong == 0)
    //        {
    //            Parent.CompoundFile.MoveReaderToSector(NextSector);

    //        }
    //        else
    //        {
    //            var divided = StreamIdLong / Parent.CompoundFile.Header.DirectoryEntriesInSector;
    //            var remainder = StreamIdLong % Parent.CompoundFile.Header.DirectoryEntriesInSector;
    //            long i = 0;
    //            while (NextSector <= SectorType.MaxRegSect)
    //            {

    //                if (i == divided && NextSector <= SectorType.MaxRegSect)
    //                {
    //                    Parent.CompoundFile.MoveReaderToSector(NextSector);
    //                    Parent.CompoundFile.Seek(remainder * 128, SeekOrigin.Current);
    //                    return;
    //                }
    //                else
    //                {
    //                    i++;
    //                    NextSector = Parent.CompoundFile.FatChain[NextSector];
    //                }



    //            }
    //            throw new IndexOutOfRangeException("Invalid Stream ID");
    //        }


    //    }
    //}
}