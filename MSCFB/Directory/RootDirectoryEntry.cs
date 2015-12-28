//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MSCFB.Enum;

//namespace MSCFB.Directory
//{
//    public class RootDirectoryEntry : DirectoryEntryParent
//    {
//        public RootDirectoryEntry(CompoundFile compoundFile) : base(compoundFile, 0)
//        {

//        }

//        public RootDirectoryEntry() : base(DirectoryEntryObjectType.RootStorageObject, "Root Entry", null)
//        {

//        }
//        private void InsertStream(string name, IDirectoryEntry root)
//        {
//            DirectoryEntryName directoryEntryName = name;
//            if (root.ChildDirectoryEntry == null && root.ObjectType==DirectoryEntryObjectType.RootStorageObject)
//            {
//                root.ChildDirectoryEntry = new StreamEntry(directoryEntryName.ToString(), root);
                
//                return;
//            }
//            else if(root.ChildDirectoryEntry!=null && root.ObjectType==DirectoryEntryObjectType.RootStorageObject)
//            {
//                InsertStream(name, root.ChildDirectoryEntry);
//            }
//            else
//            {
//                if(root.Name<directoryEntryName)
//                    if(root.RightSiblingDirectoryEntry==null)
//                        root.RightSiblingDirectoryEntry = new StreamEntry(directoryEntryName.ToString(), root);
//                    else
//                        InsertStream(directoryEntryName.ToString(), root.RightSiblingDirectoryEntry);
//                else if(root.Name>directoryEntryName)
//                    if(root.RightSiblingDirectoryEntry==null)
//                        root.LeftSiblingDirectoryEntry = new StreamEntry(directoryEntryName.ToString(), root);
//                    else
//                        InsertStream(directoryEntryName.ToString(), root.LeftSiblingDirectoryEntry);
//                else
//                    throw new InvalidOperationException($"An entry with that name already exists");
                    
//            }

        
//        }
//        private void InsertStorage(string name)
//        {
            
//        }

//    }
//}
