namespace MSCFB
{
    /// <summary>
    /// REGSECT         0x00000000 - 0xFFFFFFF9     When cast to this enum, regular sector numbers will be represented without a name.
    /// </summary>
    public enum SectorType : uint
    {
        
        /// <summary>
        /// MAXREGSECT      0xFFFFFFFA                  Maximum regular sector number.
        /// </summary>
        MaxRegSect= 0xFFFFFFFA,
        /// <summary>
        /// Not applicable  0xFFFFFFFB                  Reserved for future use.
        /// </summary>
        NotApplicable= 0xFFFFFFFB,
        /// <summary>
        /// DIFSECT         0xFFFFFFFC                  Specifies a DIFAT sector in the FAT.
        /// </summary>
        DifSect= 0xFFFFFFFC,
        /// <summary>
        /// FATSECT         0xFFFFFFFD                  Specifies a FAT sector in the FAT.
        /// </summary>
        FatSect= 0xFFFFFFFD,
        /// <summary>
        /// ENDOFCHAIN      0xFFFFFFFE                  End of a linked chain of sectors.
        /// </summary>
        EndOfChain= 0xFFFFFFFE,
        /// <summary>
        /// FREESECT        0xFFFFFFFF                  Specifies an unallocated sector in the FAT, Mini FAT, or DIFAT.
        /// </summary>
        FreeSect= 0xFFFFFFFF
    }
}