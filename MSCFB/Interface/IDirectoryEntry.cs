using System.Security.Cryptography.X509Certificates;
using MSCFB.Enum;

namespace MSCFB.Interface
{
    public interface IDirectoryEntry
    {
        uint StartingSectorLocation { get; }
        ulong StreamSize { get;}
        byte[] ModifiedTime { get;}
        byte[] CreationTime { get;}
        byte[] StateBits { get;}
        byte[] CLSID { get;}
        uint ChildID { get;}
        uint LeftSiblingID { get;}
        uint RightSiblingID { get; }
        DirectoryEntry LeftSiblingDirectoryEntry { get;}
        DirectoryEntry RightSiblingDirectoryEntry { get;}
        DirectoryEntry ChildDirectoryEntry { get; }
        ColorFlag ColorFlag { get; }
        DirectoryEntryObjectType ObjectType { get;  }
        byte[] DirectoryEntryBytes { get; }
        string RawName { get; }
        string Name { get; }
        ushort NameLength { get; }
        


    }
}