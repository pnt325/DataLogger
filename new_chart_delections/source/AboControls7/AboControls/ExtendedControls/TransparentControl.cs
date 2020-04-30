using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    /// <summary>
    /// Represents base drawing for a transparent windows forms control
    /// </summary>
    abstract class TransparentControl : Control
    {
        private BufferedGraphics _bufGraphics;

        protected TransparentControl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint 
                | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor, true);

            base.BackColor = Color.Transparent;
            UpdateGraphicsBuffer();
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                BufferedGraphicsContext bufContext = BufferedGraphicsManager.Current;
                bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                _bufGraphics.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                AdjustGraphics(_bufGraphics.Graphics);
            }
        }

        /// <summary>
        /// For adjusting the graphics object after it is created
        /// </summary>
        protected virtual void AdjustGraphics(Graphics graphics) {}

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateGraphicsBuffer();
        }

        private void DrawOtherControl(Control cont)
        {
            if (!cont.Equals(this) && cont.Bounds.IntersectsWith(this.Bounds) && cont.Visible)
            {
                using (Bitmap bitmap = new Bitmap(cont.Width, cont.Height, PixelFormat.Format32bppPArgb))
                {
                    cont.DrawToBitmap(bitmap, new Rectangle(Point.Empty, cont.Size));
                    int x = cont.Location.X - this.Location.X;
                    int y = cont.Location.Y - this.Location.Y;
                    _bufGraphics.Graphics.DrawImageUnscaled(bitmap, x, y);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Parent == null) return;
            Color color = (BackColor.Equals(Color.Transparent)) ? Parent.BackColor : this.BackColor;
            _bufGraphics.Graphics.Clear(color);

            if (_drawParentImage && this.Parent.BackgroundImage != null)
            {
                int x = -this.Location.X;
                int y = -this.Location.Y;
                _bufGraphics.Graphics.DrawImageUnscaled(this.Parent.BackgroundImage, x, y);
            }

            for (int i = this.Parent.Controls.Count - 1; i > 0; i--)
            {
                DrawOtherControl(this.Parent.Controls[i]);
            }

            OnRender(_bufGraphics.Graphics);
            _bufGraphics.Render(e.Graphics);
        }

        /// <summary>
        /// Implements drawing code
        /// </summary>
        protected abstract void OnRender(Graphics graphics);

        private bool _drawParentImage = false;
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description(@"Determines whether to draw the parent controls background. 
                If set to false, then the BackColor of this control will be drawn")]
        public bool DrawParentBackgroundImage
        {
            get { return _drawParentImage; }
            set 
            { 
                _drawParentImage = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
    }
}
