using MSCFB.Enum;

namespace MSCFB
{
    /// <summary>
    /// Sector Shift (2 bytes): This field MUST be set to 0x0009, or 0x000c, depending on the Major Version field.
    /// This field specifies the sector size of the compound file as a power of 2.
    /// </summary>
    public enum SectorShift
    {
        /// <summary>
        /// If <see cref="MajorVersion"/> is <see cref="MajorVersion.Version3"/>, the Sector Shift MUST be 0x0009, specifying a sector size of 512 bytes.
        /// </summary>
        Shift512 = 0x0009,
        /// <summary>
        /// If <see cref="MajorVersion"/> is <see cref="MajorVersion.Version4"/>, the Sector Shift MUST be 0x000C, specifying a sector size of 4096 bytes.
        /// </summary>
        Shift4096 = 0x000C
    }
}