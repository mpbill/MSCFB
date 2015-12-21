using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using MSCFB;
namespace test
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var fs = File.OpenRead("empty.msg"))
            {
                CompoundFile f = new CompoundFile(fs);
            }

        }
        
    }
}
