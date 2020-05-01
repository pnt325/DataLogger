using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

using BrightIdeasSoftware;

namespace FoobarLookalike
{
    public partial class Form1 : Form
    {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e) {
            this.InitializeObjectListView();
            this.objectListView1.SetObjects(Song.GetSongs());
        }

        private void InitializeObjectListView() {

            // Prevent the picture from showing selection
            this.olvColumnImage.Renderer = new NonSelectableRenderer();

            // Initialize hot item style
            this.hotItemStyle1.ForeColor = highlightCellColor;
            RowBorderDecoration rbd = new RowBorderDecoration();
            rbd.BorderPen = new Pen(primaryCellColor, 0.5f);
            rbd.FillBrush = new SolidBrush(Color.FromArgb(32, Color.White));
            rbd.CornerRounding = 0;
            rbd.BoundsPadding = new Size(0, 0);
            rbd.LeftColumn = 1;
            this.hotItemStyle1.Decoration = rbd;
        }

        Color highlightCellColor = Color.FromArgb(255, 250, 250, 250);
        Color primaryCellColor = Color.FromArgb(255, 189, 193, 202);
        Color otherCellColor = Color.FromArgb(255, 140, 140, 140);

        private void objectListView1_FormatCell(object sender, BrightIdeasSoftware.FormatCellEventArgs e) {
            if (e.ColumnIndex == 2)
                e.SubItem.ForeColor = primaryCellColor;
            else
                e.SubItem.ForeColor = otherCellColor;

            if (e.ColumnIndex != 0)
                return;

            // Setup album artwork column
            Song song = (Song)e.Model;
            if (song.TrackNumber == 1) {
                Image albumCover = this.GetAlbumArtwork(song);
                if (albumCover != null) {
                    // albumCover = albumCover.GetThumbnailImage(120, 120, delegate { return false; }, IntPtr.Zero);
                    ImageDecoration decoration = new ImageDecoration(albumCover);
                    decoration.ShrinkToWidth = true;
                    decoration.AdornmentCorner = ContentAlignment.BottomCenter;
                    decoration.ReferenceCorner = ContentAlignment.TopCenter;
                    e.SubItem.Decoration = decoration;
                    albumImageDecorations[song.Album] = new ForwardingDecoration(e.Item, e.SubItem, decoration);

                    TextDecoration td = new TextDecoration(song.Album, ContentAlignment.BottomCenter);
                    td.Font = this.objectListView1.Font;
                    td.Wrap = false;

                    td.TextColor = primaryCellColor;
                    td.BackColor = Color.FromArgb(255, 16, 16, 16);
                    td.CornerRounding = 4;

                    //td.BackColor = otherCellColor;
                    //td.TextColor = Color.FromArgb(255, 16, 16, 16);
                    //td.CornerRounding = 0;
                    e.SubItem.Decorations.Add(td);
                }
            } else {
                if (albumImageDecorations.ContainsKey(song.Album)) {
                    e.SubItem.Decoration = albumImageDecorations[song.Album];
                }
            }
        }

        private Image GetAlbumArtwork(Song song) {
            // Make an key from the album name so that the key is a valid identifier
            string artworkKey = song.Album.Replace(" ", "");
            artworkKey = artworkKey.Replace("'", "");
            artworkKey = artworkKey.Replace(",", "");
            artworkKey = artworkKey.Replace("&", "");
            artworkKey = artworkKey.Replace("(", "");
            artworkKey = artworkKey.Replace(")", "");
            artworkKey = artworkKey.Replace("/", "");
            Image artwork = Resource1.ResourceManager.GetObject(artworkKey) as Image;
            if (artwork == null)
                System.Diagnostics.Debug.WriteLine(String.Format("missing {0}", artworkKey));
            return artwork;
        }

        private Dictionary<string, ForwardingDecoration> albumImageDecorations = new Dictionary<string, ForwardingDecoration>();
    }

    /// <summary>
    /// This renderer doesn't ever draw it's cells as selected.
    /// </summary>
    public class NonSelectableRenderer : BaseRenderer
    {
        public override void Render(Graphics g, Rectangle r) {
            this.IsItemSelected = false;
            base.Render(g, r);
        }
    }

    /// <summary>
    /// This decoration simply forward its draw request to a different target decoration.
    /// This can be used to force another decoration to be drawn when a cell is shown
    /// (useful when you know that the target decoration will overlap another cell).
    /// </summary>
    public class ForwardingDecoration : AbstractDecoration
    {
        public ForwardingDecoration(OLVListItem item, OLVListSubItem subitem, ImageDecoration decoration) {
            this.item = item;
            this.subitem = subitem;
            this.decoration = decoration;
        }

        public override void Draw(ObjectListView olv, Graphics g, Rectangle r) {
            // Forward the drawing request to the real target
            decoration.ListItem = this.item;
            decoration.SubItem = this.subitem;
            decoration.Draw(olv, g, r);
        }

        private OLVListItem item;
        private OLVListSubItem subitem;
        private IDecoration decoration;
    }
}
