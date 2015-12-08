using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCFB
{
    class InvalidMcdfHeaderException : Exception
    {
        public InvalidMcdfHeaderException()
        {

        }

        public InvalidMcdfHeaderException(string message) : base(message)
        {

        }

    }
}
