using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BrightIdeasSoftware;

namespace TreeListViewDragDrop {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

            // Remember: TreeListViews must have a small image list assigned to them.
            // If they do not, the hit testing will be wildly off

            // Allow all models to be expanded and each model will show Children as its sub-branches
            treeListView1.CanExpandGetter = delegate(object x) { return true; };
            treeListView1.ChildrenGetter = delegate(object x) { return ((ModelWithChildren)x).Children; };
            treeListView2.CanExpandGetter = delegate(object x) { return true; };
            treeListView2.ChildrenGetter = delegate(object x) { return ((ModelWithChildren)x).Children; };

            // In the Designer, set IsSimpleDropSink to true for both tree list views.
            // That creates an appropriately configured SimpleDropSink. 
            // Here, we are just configuring it a little more
            SimpleDropSink sink1 = (SimpleDropSink)treeListView1.DropSink;
            sink1.AcceptExternal = true;
            sink1.CanDropBetween = true;
            sink1.CanDropOnBackground = true;

            SimpleDropSink sink2 = (SimpleDropSink)treeListView2.DropSink;
            sink2.AcceptExternal = true;
            sink2.CanDropBetween = true;
            sink2.CanDropOnBackground = true;

            // Give each tree its top level objects to get things going
            treeListView1.Roots = ModelWithChildren.CreateModels(null, new ArrayList { 0, 1, 2, 3, 4, 5 });
            treeListView2.Roots = ModelWithChildren.CreateModels(null, new ArrayList { "A", "B ", "C", "D", "E" });
        }

        private void HandleModelCanDrop(object sender, BrightIdeasSoftware.ModelDropEventArgs e) {
            e.Handled = true;
            e.Effect = DragDropEffects.None;
            if (e.SourceModels.Contains(e.TargetModel))
                e.InfoMessage = "Cannot drop on self";
            else {
                IEnumerable<ModelWithChildren> sourceModels = e.SourceModels.Cast<ModelWithChildren>();
                if (e.DropTargetLocation == DropTargetLocation.Background) {
                    if (e.SourceListView == e.ListView && sourceModels.All(x => x.Parent == null))
                        e.InfoMessage = "Dragged objects are already roots";
                    else {
                        e.Effect = DragDropEffects.Move;
                        e.InfoMessage = "Drop on background to promote to roots";
                    }
                } else {
                    ModelWithChildren target = (ModelWithChildren)e.TargetModel;
                    if (sourceModels.Any(x => target.IsAncestor(x)))
                        e.InfoMessage = "Cannot drop on descendant (think of the temporal paradoxes!)";
                    else
                        e.Effect = DragDropEffects.Move;
                }
            }
        }
    
        private void HandleModelDropped(object sender, BrightIdeasSoftware.ModelDropEventArgs e) {
            e.Handled = true;
            switch (e.DropTargetLocation)
            {
                case DropTargetLocation.AboveItem:
                    MoveObjectsToSibling(
                        e.ListView as TreeListView,
                        e.SourceListView as TreeListView, 
                        (ModelWithChildren)e.TargetModel, 
                        e.SourceModels, 
                        0);
                    break;
                case DropTargetLocation.BelowItem:
                    MoveObjectsToSibling(
                        e.ListView as TreeListView,
                        e.SourceListView as TreeListView, 
                        (ModelWithChildren)e.TargetModel, 
                        e.SourceModels, 
                        1);
                    break;
                case DropTargetLocation.Background:
                    MoveObjectsToRoots(
                        e.ListView as TreeListView, 
                        e.SourceListView as TreeListView, 
                        e.SourceModels);
                    break;
                case DropTargetLocation.Item:
                    MoveObjectsToChildren(
                        e.ListView as TreeListView, 
                        e.SourceListView as TreeListView, 
                        (ModelWithChildren)e.TargetModel, 
                        e.SourceModels);
                    break;
                default:
                    return;
            }

            e.RefreshObjects();
        }

        private void HandleCanDrop(object sender, OlvDropEventArgs e)
        {
            // This will only be triggered if HandleModelCanDrop doesn't set Handled to true.
            // In practice, this will only be called when the source of the drag is not an ObjectListView

            IDataObject data = e.DataObject as IDataObject;
            if (data == null || !data.GetDataPresent(DataFormats.UnicodeText))
                return;

            string str = data.GetData(DataFormats.UnicodeText) as string;
            e.Effect = String.IsNullOrEmpty(str) ? DragDropEffects.None : DragDropEffects.Copy;

            switch (e.DropTargetLocation)
            {
                case DropTargetLocation.AboveItem:
                case DropTargetLocation.BelowItem:
                    e.InfoMessage = "Cannot drop between items -- because I haven't written the logic :)";
                    break;
                case DropTargetLocation.Background:
                    e.InfoMessage = "Drop here to create a new root item called '" + str + "'";
                    break;
                case DropTargetLocation.Item:
                    e.InfoMessage = "Drop here to create a new child item called '" + str + "'";
                    break;
                default:
                    return;
            }
        }

        private void HandleDropped(object sender, OlvDropEventArgs e)
        {
            // This will only be triggered if HandleModelDropped doesn't set Handled to true.
            // In practice, this will only be called when the source of the drag is not an ObjectListView

            DataObject data = e.DataObject as DataObject;
            if (data == null || String.IsNullOrEmpty(data.GetText()))
                return;

            TreeListView treeListView = e.ListView as TreeListView;
            if (treeListView == null)
                return;

            ModelWithChildren newModel = new ModelWithChildren {
                Label = data.GetText(),
                DataForChildren = new ArrayList {"A", "B ", "C", "D", "E"}
            };

            switch (e.DropTargetLocation)
            {
                case DropTargetLocation.AboveItem:
                    break;
                case DropTargetLocation.BelowItem:
                    break;
                case DropTargetLocation.Background:
                    treeListView.AddObject(newModel);
                    break;
                case DropTargetLocation.Item:
                    ModelWithChildren targetModel = e.DropTargetItem.RowObject as ModelWithChildren;
                    if (targetModel != null)
                    {
                        newModel.Parent = targetModel;
                        targetModel.Children.Add(newModel);
                        treeListView.RefreshObject(targetModel);
                    }
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Move the given collection of model objects so that they become roots of the target tree
        /// </summary>
        /// <param name="targetTree"></param>
        /// <param name="sourceTree"></param>
        /// <param name="toMove"></param>
        private static void MoveObjectsToRoots(TreeListView targetTree, TreeListView sourceTree, IList toMove) {
            if (sourceTree == targetTree) {
                foreach (ModelWithChildren x in toMove) {
                    if (x.Parent != null) {
                        x.Parent.Children.Remove(x);
                        x.Parent = null;
                        sourceTree.AddObject(x);
                    }
                }
            } else {
                foreach (ModelWithChildren x in toMove) {
                    if (x.Parent == null) {
                        sourceTree.RemoveObject(x);
                    } else {
                        x.Parent.Children.Remove(x);
                    }
                    x.Parent = null;
                    targetTree.AddObject(x);
                }
            }
        }

        /// <summary>
        /// Move the given collection of model so that they become children of the target
        /// </summary>
        /// <param name="targetTree"></param>
        /// <param name="sourceTree"></param>
        /// <param name="target"></param>
        /// <param name="toMove"></param>
        private void MoveObjectsToChildren(TreeListView targetTree, TreeListView sourceTree, ModelWithChildren target, IList toMove) {
            foreach (ModelWithChildren x in toMove) {
                if (x.Parent == null) 
                    sourceTree.RemoveObject(x);
                else
                    x.Parent.Children.Remove(x);
                x.Parent = target;
                target.Children.Add(x);
            }
        }
 
        /// <summary>
        /// Move the given collection of model objects so that they become the siblings of the target.
        /// </summary>
        /// <param name="targetTree"></param>
        /// <param name="sourceTree"></param>
        /// <param name="target"></param>
        /// <param name="toMove"></param>
        /// <param name="siblingOffset">0 indicates that the siblings should appear before the target,
        /// 1 indicates that the siblings should appear after the target</param>
        private void MoveObjectsToSibling(TreeListView targetTree, TreeListView sourceTree, ModelWithChildren target, IList toMove, int siblingOffset) {

            // There are lots of things to get right here:
            // - sourceTree and targetTree may be the same
            // - target may be a root (which means that all moved objects will also become roots)
            // - one or more moved objects may be roots (which means the roots of the sourceTree will change)

            ArrayList sourceRoots = sourceTree.Roots as ArrayList;
            ArrayList targetRoots = targetTree == sourceTree ? sourceRoots : targetTree.Roots as ArrayList;
            bool sourceRootsChanged = false;
            bool targetRootsChanged = false;

            // We want to make the moved objects to be siblings of the target. So, we have to 
            // remove the moved objects from their old parent and give them the same parent as the target.
            // If the target is a root, then the moved objects have to become roots too.
            foreach (ModelWithChildren x in toMove) {
                if (x.Parent == null) {
                    sourceRootsChanged = true;
                    sourceRoots.Remove(x);
                } else
                    x.Parent.Children.Remove(x);
                x.Parent = target.Parent;
            }

            // Now add to the moved objects to children of their parent (or to the roots collection
            // if the target is a root)
            if (target.Parent == null) {
                targetRootsChanged = true;
                targetRoots.InsertRange(targetRoots.IndexOf(target) + siblingOffset, toMove);
            } else {
                target.Parent.Children.InsertRange(target.Parent.Children.IndexOf(target) + siblingOffset, toMove.Cast<ModelWithChildren>());
            }
            if (targetTree == sourceTree) {
                if (sourceRootsChanged || targetRootsChanged)
                    sourceTree.Roots = sourceRoots;
            } else {
                if (sourceRootsChanged)
                    sourceTree.Roots = sourceRoots;
                if (targetRootsChanged)
                    targetTree.Roots = targetRoots;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            this.treeListView1.RebuildAll(true);
        }

        private void button2_Click(object sender, EventArgs e) {
            this.treeListView1.RefreshObjects(this.treeListView1.SelectedObjects);
        }
    }

    public class ModelWithChildren {

        public DateTime Now { get { return DateTime.Now; } }
        public DateTime Update { get { return lastChildrenFetch; } }
        private DateTime lastChildrenFetch;

        public string Label { get; set; }
        public ModelWithChildren Parent { get; set; }
        public string ParentLabel {
            get { return this.Parent == null ? "none" : this.Parent.Label; }
        }
        public int ChildCount {
            get {
                return this.children == null ? 0 : this.children.Count;
            }
        }
        public List<ModelWithChildren> Children {
            get {
                lastChildrenFetch = DateTime.Now;
                if (this.children == null)
                    children = CreateModels(this, this.DataForChildren);
                return this.children;
            }
            set { this.children = value; }
        }
        private List<ModelWithChildren> children;

        public ArrayList DataForChildren { get; set; }

        public bool IsAncestor(ModelWithChildren model) {
            if (this == model)
                return true;
            if (this.Parent == null)
                return false;
            return this.Parent.IsAncestor(model);
        }

        public override string ToString() {
            return String.Format("{0}, parent: {1}", Label ?? "[]", ParentLabel);
        }

        public static List<ModelWithChildren> CreateModels(ModelWithChildren parent, ArrayList data) {
            List<ModelWithChildren> models = new List<ModelWithChildren>();
            foreach (object x in data) {
                models.Add(new ModelWithChildren {
                    Label = (parent == null ? x.ToString() : parent.Label + "-" + x.ToString()),
                    Parent = parent,
                    DataForChildren = data
                });
            }
            return models;
        }
    }
}
