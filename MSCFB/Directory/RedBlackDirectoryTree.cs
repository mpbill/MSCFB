using System;
using System.Net.Http.Headers;
using MSCFB.Enum;

namespace MSCFB.Directory
{
    public class RedBlackDirectoryTree
    {
        private DirectoryEntry TreeParent { get; set; }
        private DirectoryEntry RootNode { get; set; }
        private CompoundFile CompoundFile { get; set; }
        public long Count { get; private set; }
        public RedBlackDirectoryTree(DirectoryEntry Parent)
        {
            CompoundFile = Parent.CompoundFile;
            TreeParent = Parent;
            RootNode = DirectoryEntryFactory.LoadDirectoryEntry(CompoundFile, TreeParent.ChildID, TreeParent);
            Count++;
            RootNode.DirectoryEntryTreeParentNode = null;
            LoadTree(RootNode);

        }
        private void LoadTree(DirectoryEntry Parent)
        {
            if (Parent.LeftSiblingID != StreamID.NoStream)
            {
                Parent.LeftSiblingDirectoryEntry = DirectoryEntryFactory.LoadDirectoryEntry(CompoundFile, Parent.LeftSiblingID, Parent);
                Count++;
                
                LoadTree(Parent.LeftSiblingDirectoryEntry);
            }
            if (Parent.RightSiblingID != StreamID.NoStream)
            {
                Parent.RightSiblingDirectoryEntry = DirectoryEntryFactory.LoadDirectoryEntry(CompoundFile, Parent.RightSiblingID, Parent);
                
                Count++;
                LoadTree(Parent.RightSiblingDirectoryEntry);
            }
            if (Parent.ChildID != StreamID.NoStream)
            {
                Parent.ChildRedBlackDirectoryTree = new RedBlackDirectoryTree(Parent);
                
            }


        }
        //private void LeftRotate(DirectoryEntry x)
        //{
        //    DirectoryEntry y = x.RightSiblingDirectoryEntry;
        //    x.RightSiblingDirectoryEntry = y.LeftSiblingDirectoryEntry;
        //    if (y.LeftSiblingDirectoryEntry != null)
        //        y.LeftSiblingDirectoryEntry.DirectoryEntryTreeParentNode = x;
        //    if (x !=RootNode)
        //        RootNode = y;
        //    else if (x == x.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry)
        //        x.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry = y;
        //    else
        //    {
        //        x.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry = y;
        //    }
        //    y.LeftSiblingDirectoryEntry = x;
        //    x.DirectoryEntryTreeParentNode = y;


        //}
        //private void InsertFix(DirectoryEntry z)
        //{
        //    while (z.DirectoryEntryTreeParentNode.ColorFlag == ColorFlag.Red)
        //    {
        //        if (z.DirectoryEntryTreeParentNode == z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry)
        //        {
        //            DirectoryEntry y = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry;
        //            if(y.ColorFlag==ColorFlag.Red)
        //            {
        //                z.DirectoryEntryTreeParentNode.ColorFlag= ColorFlag.Black;
        //                y.ColorFlag=ColorFlag.Black;
        //                z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag=ColorFlag.Red;
        //                z = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode;

        //            }
        //            else 
        //            {
        //                if (z == z.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry)
        //                {
        //                    z = z.DirectoryEntryTreeParentNode;
        //                    LeftRotate(z);
        //                }
        //                z.DirectoryEntryTreeParentNode.ColorFlag=ColorFlag.Black;
        //                z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag=ColorFlag.Red;
        //                RightRotate(z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode);
        //            }
                    
        //        }
        //        else
        //        {
        //            DirectoryEntry y = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry;
        //            if (y.ColorFlag == ColorFlag.Red)
        //            {
        //                z.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Black;
        //                y.ColorFlag = ColorFlag.Black;
        //                z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Red;
        //                z = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode;

        //            }
        //            else
        //            {
        //                if (z == z.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry)
        //                {
        //                    z = z.DirectoryEntryTreeParentNode;
        //                    RightRotate(z);
        //                }
        //                z.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Black;
        //                z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Red;
        //                LeftRotate(z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode);
        //            }
        //        }
                

        //    }
        //    RootNode.ColorFlag = ColorFlag.Black;
            
        //}
        
        //private void RightRotate(DirectoryEntry x)
        //{
        //    DirectoryEntry y = x.LeftSiblingDirectoryEntry;
        //    y.LeftSiblingDirectoryEntry = x.RightSiblingDirectoryEntry;
        //    if (x.RightSiblingDirectoryEntry != null)
        //        x.RightSiblingDirectoryEntry.DirectoryEntryTreeParentNode = y;
        //    x.DirectoryEntryTreeParentNode = y.DirectoryEntryTreeParentNode;
        //    if (y == RootNode)
        //        RootNode = x;
        //    else if (y == y.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry)
        //        y.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry = x;
        //    else
        //    {
        //        y.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry = x;
        //    }
        //    x.RightSiblingDirectoryEntry = y;
        //    y.DirectoryEntryTreeParentNode = x;
        //}
        /// <summary>
        /// Recursivly inserts a new directory entry into the tree.
        /// </summary>
        /// <param name="root">the root to possibly be replaced if it is null, or the <see cref="RootNode"/> on the first recursion.</param>
        /// <param name="toInsert">the <see cref="DirectoryEntry"/> to insert</param>
        public void Insert(DirectoryEntry toInsert)
        {
            if(toInsert.ColorFlag!=ColorFlag.Red)
                toInsert.FlipColor();
            if (RootNode == null)
            {
                RootNode = toInsert;
                RootNode.ParentDirectoryEntry = TreeParent;
                PostInsert(toInsert);
            }
            else
            {
                DirectoryEntry nextNode = RootNode;
                DirectoryEntry nextNodesParent = RootNode.ParentDirectoryEntry;
                bool wentRight = false;
                while (true)
                {
                    if (nextNode == null)
                    {
                        if (wentRight)
                        {
                            toInsert.ParentDirectoryEntry = nextNodesParent;
                            nextNodesParent.RightSiblingDirectoryEntry = toInsert;
                        }
                        else
                        {
                            toInsert.ParentDirectoryEntry = nextNodesParent;
                            nextNodesParent.LeftSiblingDirectoryEntry = toInsert;
                        }
                        break;
                      
                    }
                    else if (toInsert < nextNode)
                    {
                        wentRight = true;
                        nextNodesParent = nextNode;
                        nextNode = nextNode.LeftSiblingDirectoryEntry;
                    }
                    else if (toInsert > nextNode)
                    {
                        wentRight = false;
                        nextNodesParent = nextNode;
                        nextNode = nextNode.RightSiblingDirectoryEntry;

                    }
                }
            }
            PostInsert(toInsert);

        }
        /// <summary>
        /// also known as case1.  
        /// </summary>
        /// <param name="insertedEntry"></param>
        private void PostInsert(DirectoryEntry insertedEntry)
        {
            if (insertedEntry == RootNode)
                insertedEntry.ColorFlag = ColorFlag.Black;
            else
            {
                insertCase2(insertedEntry);
            }
        }

        private void insertCase2(DirectoryEntry insertedEntry)
        {
            if(insertedEntry.ParentDirectoryEntry.ColorFlag==ColorFlag.Black)
                return;
            else
            {
                insertCase3(insertedEntry);
            }
        }

        private void insertCase3(DirectoryEntry insertedEntry)
        {
            DirectoryEntry uncle, grandparent;
            uncle= Uncle(insertedEntry);
            if (uncle != null && uncle.ColorFlag == ColorFlag.Red)
            {
                insertedEntry.ParentDirectoryEntry.ColorFlag = ColorFlag.Black;
                uncle.ColorFlag = ColorFlag.Black;
                grandparent = Grandparent(insertedEntry);
                grandparent.ColorFlag = ColorFlag.Red;
                PostInsert(insertedEntry);
            }
            else
            {
                insertCase4(insertedEntry);
            }
        }

        private void insertCase4(DirectoryEntry insertedEntry)
        {
            DirectoryEntry grandparent = Grandparent(insertedEntry);
            if (insertedEntry == insertedEntry.ParentDirectoryEntry.RightSiblingDirectoryEntry &&
                insertedEntry.ParentDirectoryEntry == grandparent.LeftSiblingDirectoryEntry)
            {
                //Left Rotate
                DirectoryEntry saved_p = grandparent.LeftSiblingDirectoryEntry,
                    saved_left_n = insertedEntry.LeftSiblingDirectoryEntry;
                grandparent.LeftSiblingDirectoryEntry = insertedEntry;
                insertedEntry.LeftSiblingDirectoryEntry = saved_p;
                saved_p.RightSiblingDirectoryEntry = saved_left_n;
                insertedEntry = insertedEntry.LeftSiblingDirectoryEntry;
            }
            else if (insertedEntry == insertedEntry.ParentDirectoryEntry.LeftSiblingDirectoryEntry &&
                     insertedEntry.ParentDirectoryEntry == grandparent.RightSiblingDirectoryEntry)
            {
                //Right rotate
                DirectoryEntry saved_p = grandparent.RightSiblingDirectoryEntry,
                    saved_right_n = insertedEntry.RightSiblingDirectoryEntry;
                grandparent.RightSiblingDirectoryEntry = insertedEntry;
                insertedEntry.RightSiblingDirectoryEntry = saved_p;
                saved_p.LeftSiblingDirectoryEntry = saved_right_n;
                insertedEntry = insertedEntry.RightSiblingDirectoryEntry;
            }
            insertCase5(insertedEntry);

        }

        private void insertCase5(DirectoryEntry insertedEntry)
        {
            DirectoryEntry grandparent = Grandparent(insertedEntry);
            insertedEntry.ParentDirectoryEntry.ColorFlag=ColorFlag.Black;
            grandparent.ColorFlag=ColorFlag.Red;
            if (insertedEntry == insertedEntry.ParentDirectoryEntry.LeftSiblingDirectoryEntry)
            {
                //right rotate
                DirectoryEntry saved_p = grandparent.RightSiblingDirectoryEntry,
                saved_right_n = insertedEntry.RightSiblingDirectoryEntry;
                grandparent.RightSiblingDirectoryEntry = insertedEntry;
                insertedEntry.RightSiblingDirectoryEntry = saved_p;
                saved_p.LeftSiblingDirectoryEntry = saved_right_n;
                insertedEntry = insertedEntry.RightSiblingDirectoryEntry;
            }
            else
            {
                //Left Rotate
                DirectoryEntry saved_p = grandparent.LeftSiblingDirectoryEntry,
                saved_left_n = insertedEntry.LeftSiblingDirectoryEntry;
                grandparent.LeftSiblingDirectoryEntry = insertedEntry;
                insertedEntry.LeftSiblingDirectoryEntry = saved_p;
                saved_p.RightSiblingDirectoryEntry = saved_left_n;
                insertedEntry = insertedEntry.LeftSiblingDirectoryEntry;
            }
        }

        public DirectoryEntry InsertNewDirectoryEntry(string name, DirectoryEntryObjectType type, Dire)
        private DirectoryEntry NewDirectoryEntry(string name, DirectoryEntryObjectType type, DirectoryEntry parent)
        {
            return new DirectoryEntry()
            {
                //Need to Figure out how to assign a new stream ID.
                //StreamId = 
                Name = name,
                ObjectType = type,
                CompoundFile = CompoundFile,
                ParentDirectoryEntry = parent,
                ChildID = StreamID.NoStream,
                LeftSiblingID = StreamID.NoStream,
                RightSiblingID = StreamID.NoStream,

            };
        }
        private DirectoryEntry Grandparent(DirectoryEntry e)
        {
            if (e==null || e == RootNode || e.ParentDirectoryEntry ==null|| e.ParentDirectoryEntry == RootNode)
                return null;
            else
            {
                return e.ParentDirectoryEntry.ParentDirectoryEntry;
            }
        }
        private DirectoryEntry Uncle(DirectoryEntry e)
        {
            var g = Grandparent(e);
            if (g == null)
            {
                return null;
            }
            else if (e.ParentDirectoryEntry == g.LeftSiblingDirectoryEntry)
                return g.RightSiblingDirectoryEntry;
            else
            {
                return g.LeftSiblingDirectoryEntry;
            }
        }
    }
}