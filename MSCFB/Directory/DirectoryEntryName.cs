using System;
using System.Text;

namespace MSCFB.Directory
{
    public struct DirectoryEntryName : IComparable<String>, IComparable
    {

        private string _name;
        public char this[int index]
        {
            get { return Name[index]; }
        }
        public int Length => (Name.Length + 1) * 2;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length > 31)
                {
                    throw new InvalidOperationException("Name cannot be more than 31 characters.");
                }
                foreach (string illegal in Resources.Illegal)
                {
                    if (value.Contains(illegal))
                        throw new InvalidOperationException($"String Cannot contain '{illegal}'");
                }
                _name = value;
                
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public DirectoryEntryName(string name)
        {
            if (name.Length > 31)
            {
                throw new InvalidOperationException("Name cannot be more than 31 characters.");
            }
            foreach (string illegal in Resources.Illegal)
            {
                if (name.Contains(illegal))
                    throw new InvalidOperationException($"String Cannot contain '{illegal}'");
            }
            _name = name;
        }
        public DirectoryEntryName(byte[] bytes, ushort length)
        {
           
            var value = Encoding.Unicode.GetString(bytes, 0, length-2);
            if (bytes.Length!=64)
            {
                throw new InvalidOperationException("Name cannot be more than 31 characters.");
            }
            foreach (string illegal in Resources.Illegal)
            {
                if (value.Contains(illegal))
                    throw new InvalidOperationException($"String Cannot contain '{illegal}'");
            }
            _name = value;
        }
        public static implicit operator DirectoryEntryName(string name)
        {
            return new DirectoryEntryName(name);
        }
        public int CompareTo(string other)
        {
            if (Name.Length < other.Length)
                return -1;
            else if (this.Name.Length > other.Length)
                return 1;
            else
            {
                Char a, b;
                for (int i = 0; i < Name.Length; i++)
                {
                    a = Char.ToUpper(Name[i]);
                    b = Char.ToUpper(other[i]);
                    if (a == b)
                        continue;
                    else
                    {
                        return a.CompareTo(b);
                    }

                }
                return 0;
            }
        }

        public int CompareTo(object obj)
        {

            return CompareTo(obj.ToString());
        }
        public static bool operator <(DirectoryEntryName one, DirectoryEntryName two)
        {
            return one.CompareTo(two) < 0;
        }

        public static bool operator >(DirectoryEntryName one, DirectoryEntryName two)
        {
            return one.CompareTo(two) > 0;
        }

        

    }
}