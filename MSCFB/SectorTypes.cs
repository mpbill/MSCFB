namespace MSCFB
{
    public enum SectorTypes
    {
        /// <summary>
        /// REGSECT         0x00000000 - 0xFFFFFFF9     Regular sector number.
        /// </summary>
        RegSect,
        /// <summary>
        /// MAXREGSECT      0xFFFFFFFA                  Maximum regular sector number.
        /// </summary>
        MaxRegSect,
        /// <summary>
        /// Not applicable  0xFFFFFFFB                  Reserved for future use.
        /// </summary>
        NotApplicable,
        /// <summary>
        /// DIFSECT         0xFFFFFFFC                  Specifies a DIFAT sector in the FAT.
        /// </summary>
        DifSect,
        /// <summary>
        /// FATSECT         0xFFFFFFFD                  Specifies a FAT sector in the FAT.
        /// </summary>
        FatSect,
        /// <summary>
        /// ENDOFCHAIN      0xFFFFFFFE                  End of a linked chain of sectors.
        /// </summary>
        EndOfChain,
        /// <summary>
        /// FREESECT        0xFFFFFFFF                  Specifies an unallocated sector in the FAT, Mini FAT, or DIFAT.
        /// </summary>
        FreeSect
    }
}