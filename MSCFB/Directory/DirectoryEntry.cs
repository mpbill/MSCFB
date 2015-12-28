using System;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    public class DirectoryEntry : DirectoryEntryParent
    {
         public void FlipColor()
         {
            if (this.ColorFlag == ColorFlag.Red)
                this.ColorFlag = ColorFlag.Black;
            else if (this.ColorFlag == ColorFlag.Black)
                this.ColorFlag = ColorFlag.Red;
         }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
            {
                return (DirectoryEntry)obj == this;
            }
        }

        public static bool operator ==(DirectoryEntry one, DirectoryEntry two)
        {
            if (ReferenceEquals(null, one))
                return ReferenceEquals(null, two);
            else
            {
                return one.CompareTo(two) == 0;
            }
              
            
            
        }

        public static bool operator !=(DirectoryEntry one, DirectoryEntry two)
        {
            return !(one == two);
        }

        public static bool operator <(DirectoryEntry one, DirectoryEntry two)
        {
            if(ReferenceEquals(null, one) || ReferenceEquals(null, two))
                throw new NullReferenceException();
            return one.CompareTo(two) < 0;
            
        }

        public static bool operator >(DirectoryEntry one, DirectoryEntry two)
        {
            if (ReferenceEquals(null, one) || ReferenceEquals(null, two))
                throw new NullReferenceException();
            return one.CompareTo(two) > 0;
            
        }
    }
}