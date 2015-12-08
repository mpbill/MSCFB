namespace MSCFB
{
    /// <summary>
    /// Major Version (2 bytes): Version number for breaking changes. This field MUST be set to either 0x0003 (version 3) or 0x0004 (version 4).
    /// </summary>
    public enum MajorVersion : ushort
    {
        Version3 = 0x0003,
        Version4 = 0x0004
    }
}