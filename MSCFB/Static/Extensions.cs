using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSCFB.Enum;

namespace MSCFB.Static
{
    public static class Extensions
    {
        public static void Seek(this BinaryReader binaryReader, long offset, SeekOrigin origin)
        {

            binaryReader.BaseStream.Seek(offset, origin);
        }
        public static void Seek(this BinaryWriter binaryWriter, long offset, SeekOrigin origin)
        {

            binaryWriter.BaseStream.Seek(offset, origin);
        }
        public static SectorType ReadSectorType(this BinaryReader binaryReader)
        {
            
            return (SectorType)binaryReader.ReadUInt32();
        }
        public static void Write(this BinaryWriter binaryWriter, SectorType sectorType)
        {
            binaryWriter.Write((uint)sectorType);
        }
        public static StreamID ReadStreamID(this BinaryReader binaryReader)
        {
            return (StreamID)binaryReader.ReadUInt32();
        }
        public static void Write(this BinaryWriter binaryWriter, StreamID streamID)
        {
            binaryWriter.Write((uint)streamID);
        }
        public static UInt32 Power(this UInt32 a, UInt32 b)
        {
            UInt32 result = 1;
            for (long i = 0; i < b; i++)
                result *= a;
            return result;
        }
        public static UInt16 Power(this UInt16 a, UInt16 b)
        {
            UInt16 result = 1;
            for (long i = 0; i < b; i++)
                result *= a;
            return result;
        }
    }
}
