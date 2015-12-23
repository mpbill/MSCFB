using System;
using System.Security.Cryptography.X509Certificates;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    public interface IDirectoryEntry : IComparable<IDirectoryEntry>
    {
        StreamID StreamId { get; }
        long StreamIdLong { get; }
        ushort NameLength { get; }
        uint StartingSectorLocation { get;  }
        ulong StreamSize { get;  }
        byte[] ModifiedTime { get;  }
        byte[] CreationTime { get;  }
        byte[] StateBits { get;  }
        byte[] CLSID { get;  }
        StreamID ChildID { get;  }
        StreamID LeftSiblingID { get; }
        StreamID RightSiblingID { get; }
        IDirectoryEntry LeftSiblingDirectoryEntry { get; set; }
        IDirectoryEntry RightSiblingDirectoryEntry { get; set; }
        IDirectoryEntry ParentDirectoryEntry { get; set; }
        CompoundFile CompoundFile { get;  }
        IDirectoryEntry ChildDirectoryEntry { get; set; }
        ColorFlag ColorFlag { get;  }
        DirectoryEntryObjectType ObjectType { get;  }
        String Name { get; set; }
        
            
    }
}