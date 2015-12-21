﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCFB.Enum;

namespace MSCFB
{
    public class DirectoryChain
    {
        public CompoundFile CompoundFile { get; private set; }
        private List<uint> SectorNumbers { get; set; } 
        public uint this[int index]
        {
            get
            {
                return SectorNumbers[index];
            }
            set
            {
                SectorNumbers[index] = value;
            }
        }
        public DirectoryChain(CompoundFile compoundFile)
        {
            CompoundFile = compoundFile;
            SectorNumbers = new List<uint>();
            SectorNumbers.Add((uint)CompoundFile.Header.FirstDirectorySectorLocation);
            var nextSector = CompoundFile.FatSectorChain[(int)CompoundFile.Header.FirstDirectorySectorLocation];
            while (nextSector != SectorType.EndOfChain)
            {
                SectorNumbers.Add((uint)nextSector);
                nextSector = CompoundFile.FatSectorChain[(int)SectorNumbers.Last()];
            }
        }
        
    }
}
