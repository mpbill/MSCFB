using System;
using System.Security.Cryptography.X509Certificates;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    public interface IDirectoryEntry : IComparable<IDirectoryEntry>, IComparable<String>
    {
        StreamID StreamId { get; set; }
        long StreamIdLong { get; }
        uint StartingSectorLocation { get; set; }
        ulong StreamSize { get; set; }
        byte[] ModifiedTime { get; set; }
        byte[] CreationTime { get; set; }
        byte[] StateBits { get; set; }
        byte[] CLSID { get; set; }
        StreamID ChildID { get; set; }
        StreamID LeftSiblingID { get; set; }
        StreamID RightSiblingID { get; set; }
        RedBlackDirectoryTree ChildRedBlackDirectoryTree { get; set; }
        DirectoryEntry LeftSiblingDirectoryEntry { get; set; }
        DirectoryEntry RightSiblingDirectoryEntry { get; set; }
        DirectoryEntry ParentDirectoryEntry { get; set; }
        CompoundFile CompoundFile { get; }
        ColorFlag ColorFlag { get; set; }
        DirectoryEntryObjectType ObjectType { get; }
        DirectoryEntryName Name { get; set; }
        
            
    }
}