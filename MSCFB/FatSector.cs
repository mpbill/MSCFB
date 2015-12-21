using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSCFB.Enum;
namespace MSCFB
{
    public class FatSectorChain
    {
        public CompoundFile CompoundFile { get; private set; }
        public List<SectorType> SectorsList { get; private set; }
        public FatSectorChain(CompoundFile compoundFile)
        {
            this.CompoundFile = compoundFile;
            SectorsList = new List<SectorType>();
            LoadSectors();
            
        }
        public SectorType this[int index]
        {
            get { return SectorsList[index]; }
            set { SectorsList[index] = value; }
        }
        private void LoadSectors()
        {
            CompoundFile.MoveReaderToSector((uint)CompoundFile.Header.DifatArray[0]);
            while (true)
            {
                var bytes =
                    CompoundFile.FileReader.ReadBytes(
                        (int) Resources.UIntPow(2, (UInt32) CompoundFile.Header.SectorShift));
                for (int i = 0; i < bytes.Length/4; i++)
                {
                    SectorsList.Add((SectorType) BitConverter.ToUInt32(bytes.Skip(i*4).Take(4).ToArray(), 0));
                }
                break;
            }
        }

        //public byte[] this[int index]
        //{
        //    get
        //    {
        //        long offset = (index + 1)*SectorSize;
        //        BinaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
        //        return BinaryReader.ReadBytes(SectorSize);
        //    }
        //    set
        //    {
                
        //    }
        //}
    }
}