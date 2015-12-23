using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    public class RootDirectoryEntry : DirectoryEntryParent
    {
        public RootDirectoryEntry(CompoundFile compoundFile) : base(compoundFile, 0)
        {
        }

        public RootDirectoryEntry() : base(DirectoryEntryObjectType.RootStorageObject, "Root Entry", null)
        {
        }
        internal void InsertNode(string name, DirectoryEntryObjectType type)
        {
            switch (type)
            {
                case default:
                {
                    throw new InvalidOperationException($"Can only insert {DirectoryEntryObjectType.StorageObject} or {DirectoryEntryObjectType.StreamObject}");
                    break;
                }
                case DirectoryEntryObjectType.StreamObject:
                {
                    break;
                }
                case DirectoryEntryObjectType.StorageObject:
                {
                    break;
                }
            }
            
                

        }
        private void InsertStream(string name)
        {
            StreamEntry entry = new StreamEntry();
        }
    }
}
