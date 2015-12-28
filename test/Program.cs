using System;
using System.Collections.Generic;
using System.Collections.Generic.RedBlack;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Threading;
using MSCFB;
using MSCFB.Chains;
using MSCFB.Directory;
using RedBlackCS;

namespace test
{
    class Program
       
    {
        static void Main(string[] args)
        {
            MSCFB.Directory.DirectoryEntry e = new MSCFB.Directory.DirectoryEntry() { Name = "Michael" };
            MSCFB.Directory.DirectoryEntry f = new MSCFB.Directory.DirectoryEntry() { Name = "Michael" };
            var a = e == f;
            Thread t = new Thread(new ThreadStart(method), 100000000);
            t.Start();
            t.Join();
        }
        static void method()
        {
            using (var fs = File.OpenRead("empty.msg"))
            {
                CompoundFile f = new CompoundFile(fs);
                var difat = f.DifatChain.ToArray();
                var fat = f.FatChain.ToArray();
                return;


            }
        }
    }
   
}
