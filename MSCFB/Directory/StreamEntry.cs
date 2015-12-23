using MSCFB.Enum;

namespace MSCFB.Directory
{
    public class StreamEntry : DirectoryEntryParent
    {
        public StreamEntry(CompoundFile compoundFile, StreamID streamId) : base(compoundFile, streamId)
        {
        }

        public StreamEntry(DirectoryEntryObjectType type, string name, IDirectoryEntry Parent) : base(type, name, Parent)
        {
        }

        public StreamEntry(IDirectoryEntry Parent, StreamID streamId) : base(Parent, streamId)
        {
        }
    }
}