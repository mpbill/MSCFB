using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
