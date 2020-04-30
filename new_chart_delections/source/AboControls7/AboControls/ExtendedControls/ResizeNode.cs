using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    class ResizeNode : Control
    {
        [Description("Occurs when the user un-clicks after resizing")]
        public event EventHandler ParentResizeEnd;
        [Description("Occurs when the mouse moves when resizing")]
        public event EventHandler ParentResized;

        private Point _lastPos;
        private bool _triangular = true;

        public ResizeNode()
        {
            this.Size = new Size(20, 20);
            this.Cursor = Cursors.SizeNWSE;
            this.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
        }

        private void SetRegion()
        {
            if (_triangular)
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLine(new Point(0, this.Height), (Point)this.Size);
                    path.AddLine((Point)this.Size, new Point(this.Width, 0));
                    this.Region = new Region(path);
                }
            }
            else
            {
                this.Region = new Region(new Rectangle(0, 0, this.Width, this.Height));
            }
        }

        /// <summary>
        /// Positions the node at the bottom right corner
        /// Docking or anchoring logic is not needed as the anchor property is used for this
        /// </summary>
        private void PositionNode()
        {
            if (this.Parent != null)
            {
                // Use client size so that the control appears within the borders of certain controls
                // Ex: a form or a panel with a border
                int xPos = this.Parent.ClientSize.Width - this.Width - this.Padding.Right;
                int yPos = this.Parent.ClientSize.Height - this.Height - this.Padding.Bottom;
                this.Location = new Point(xPos, yPos);
            }
        }

        #region Overrides
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _lastPos = e.Location;
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            OnParentResizeEnd(e);
        }

        protected override void OnResize(EventArgs e)
        {
            SetRegion();
            PositionNode();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                int width = this.Parent.Width + e.Location.X - _lastPos.X;
                int height = this.Parent.Height + e.Location.Y - _lastPos.Y;
                this.Parent.Size = new Size(width, height);
                OnParentResized(EventArgs.Empty);
            }
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            PositionNode();
        }
        #endregion

        #region Events
        protected virtual void OnParentResizeEnd(EventArgs e)
        {
            if (ParentResizeEnd != null)
                ParentResizeEnd(this, e);
        }

        protected virtual void OnParentResized(EventArgs e)
        {
            if (ParentResized != null)
                ParentResized(this, e);
        }
        #endregion

        [Category("Appearance")]
        [Description("Determines if the control should appear as a triangle or square")]
        [DefaultValue(true)]
        public bool Triangular
        {
            get { return _triangular; }
            set
            {
                _triangular = value;
                SetRegion();
            }
        }

        public override AnchorStyles Anchor
        {
            get
            {
                return AnchorStyles.Bottom | AnchorStyles.Right;
            }
            set
            {
                base.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            }
        }
    }
}
