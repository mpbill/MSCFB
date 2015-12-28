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
                Parent.LeftSiblingDirectoryEntry.DirectoryEntryTreeParentNode = Parent;
                LoadTree(Parent.LeftSiblingDirectoryEntry);
            }
            if (Parent.RightSiblingID != StreamID.NoStream)
            {
                Parent.RightSiblingDirectoryEntry = DirectoryEntryFactory.LoadDirectoryEntry(CompoundFile, Parent.RightSiblingID, Parent);
                Parent.RightSiblingDirectoryEntry.DirectoryEntryTreeParentNode = Parent;
                Count++;
                LoadTree(Parent.RightSiblingDirectoryEntry);
            }
            if (Parent.ChildID != StreamID.NoStream)
            {
                Parent.ChildRedBlackDirectoryTree = new RedBlackDirectoryTree(Parent);
                
            }


        }
        private void LeftRotate(DirectoryEntry x)
        {
            DirectoryEntry y = x.RightSiblingDirectoryEntry;
            x.RightSiblingDirectoryEntry = y.LeftSiblingDirectoryEntry;
            if (y.LeftSiblingDirectoryEntry != null)
                y.LeftSiblingDirectoryEntry.DirectoryEntryTreeParentNode = x;
            if (x !=RootNode)
                RootNode = y;
            else if (x == x.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry)
                x.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry = y;
            else
            {
                x.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry = y;
            }
            y.LeftSiblingDirectoryEntry = x;
            x.DirectoryEntryTreeParentNode = y;


        }
        private void InsertFix(DirectoryEntry z)
        {
            while (z.DirectoryEntryTreeParentNode.ColorFlag == ColorFlag.Red)
            {
                if (z.DirectoryEntryTreeParentNode == z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry)
                {
                    DirectoryEntry y = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry;
                    if(y.ColorFlag==ColorFlag.Red)
                    {
                        z.DirectoryEntryTreeParentNode.ColorFlag= ColorFlag.Black;
                        y.ColorFlag=ColorFlag.Black;
                        z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag=ColorFlag.Red;
                        z = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode;

                    }
                    else 
                    {
                        if (z == z.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry)
                        {
                            z = z.DirectoryEntryTreeParentNode;
                            LeftRotate(z);
                        }
                        z.DirectoryEntryTreeParentNode.ColorFlag=ColorFlag.Black;
                        z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag=ColorFlag.Red;
                        RightRotate(z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode);
                    }
                    
                }
                else
                {
                    DirectoryEntry y = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry;
                    if (y.ColorFlag == ColorFlag.Red)
                    {
                        z.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Black;
                        y.ColorFlag = ColorFlag.Black;
                        z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Red;
                        z = z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode;

                    }
                    else
                    {
                        if (z == z.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry)
                        {
                            z = z.DirectoryEntryTreeParentNode;
                            RightRotate(z);
                        }
                        z.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Black;
                        z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode.ColorFlag = ColorFlag.Red;
                        LeftRotate(z.DirectoryEntryTreeParentNode.DirectoryEntryTreeParentNode);
                    }
                }
                

            }
            RootNode.ColorFlag = ColorFlag.Black;
            RecurseAndFixParents(RootNode);
        }
        private void RecurseAndFixParents(DirectoryEntry entry)
        {
            if (entry != RootNode)
            {
                entry.ParentDirectoryEntry = entry.DirectoryEntryTreeParentNode;
            }
            RecurseAndFixParents(entry.LeftSiblingDirectoryEntry);
            RecurseAndFixParents(entry.RightSiblingDirectoryEntry);
        }
        private void RightRotate(DirectoryEntry x)
        {
            DirectoryEntry y = x.LeftSiblingDirectoryEntry;
            y.LeftSiblingDirectoryEntry = x.RightSiblingDirectoryEntry;
            if (x.RightSiblingDirectoryEntry != null)
                x.RightSiblingDirectoryEntry.DirectoryEntryTreeParentNode = y;
            x.DirectoryEntryTreeParentNode = y.DirectoryEntryTreeParentNode;
            if (y == RootNode)
                RootNode = x;
            else if (y == y.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry)
                y.DirectoryEntryTreeParentNode.RightSiblingDirectoryEntry = x;
            else
            {
                y.DirectoryEntryTreeParentNode.LeftSiblingDirectoryEntry = x;
            }
            x.RightSiblingDirectoryEntry = y;
            y.DirectoryEntryTreeParentNode = x;
        }
        public void Insert(DirectoryEntry z)
        {
            DirectoryEntry y = null;
            DirectoryEntry x = RootNode;
            while (x!=null)
            {
                y = x;
                if (z.CompareTo(x) < 0)
                    x = x.LeftSiblingDirectoryEntry;
                else
                {
                    x = x.RightSiblingDirectoryEntry;
                }
            }
            z.ChildID=StreamID.NoStream;
            z.LeftSiblingID=StreamID.NoStream; ;
            z.RightSiblingID = StreamID.NoStream;
            z.ColorFlag = ColorFlag.Red; //1
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