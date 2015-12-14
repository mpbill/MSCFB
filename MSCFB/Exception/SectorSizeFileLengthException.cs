using System;

namespace MSCFB
{
    class SectorSizeFileLengthException:System.Exception
    {
        public SectorSizeFileLengthException():base()
        {
            
        }
        public SectorSizeFileLengthException(string message) : base(message)
        {
            
        }
    }
}