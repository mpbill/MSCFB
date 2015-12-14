namespace MSCFB.Enum
{
    /// <summary>
    /// Object Type (1 byte): This field MUST be 0x00, 0x01, 0x02, or 0x05, depending on the actual type of object. All other values are not valid.
    /// MS-CFB 2.6.1
    /// </summary>
    public enum DirectoryEntryObjectType : byte
    {
        UnknownOrUnallocated = 0x00,
        StorageObject = 0x01,
        StreamObject = 0x02,
        RootStorageObject = 0x05
    }
}