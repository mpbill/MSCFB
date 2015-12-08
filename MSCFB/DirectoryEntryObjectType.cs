namespace MSCFB
{
    public enum DirectoryEntryObjectType : byte
    {
        UnknownOrUnallocated = 0x00,
        StorageObject = 0x01,
        StreamObject = 0x02,
        RootStorageObject = 0x03
    }
}