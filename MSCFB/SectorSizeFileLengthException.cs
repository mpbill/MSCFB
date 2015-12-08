using System;

namespace MSCFB
{
    class SectorSizeFileLengthException:Exception
    {
        public SectorSizeFileLengthException():base()
        {
            
        }
        public SectorSizeFileLengthException(string message) : base(message)
        {
            
        }
    }
}