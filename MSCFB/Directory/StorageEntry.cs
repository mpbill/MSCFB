using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    class StorageEntry : DirectoryEntryParent
    {

        public StorageEntry(DirectoryEntryObjectType type, string name, IDirectoryEntry Parent) : base(type, name, Parent)
        {
        }

        public StorageEntry(IDirectoryEntry Parent, StreamID streamId) : base(Parent, streamId)
        {
        }
    }
}
