using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    class HotToolStripTextBox : ToolStripTextBox
    {
        private Graphics _graphics;
        private float _stroke = 0.5f;

        private void Parent_Paint(object sender, PaintEventArgs e)
        {
            if (TextBox.ContainsFocus && _graphics != null)
                DrawHotTrack(Brushes.Goldenrod);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            _graphics = this.Parent.CreateGraphics();

            if (!this.TextBox.ContainsFocus)
                DrawHotTrack(Brushes.CornflowerBlue);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (!this.Focused)
            {
                ClearHotTrack();
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            DrawHotTrack(Brushes.Goldenrod);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            if (_graphics != null)
            {
                ClearHotTrack();
            }
        }

        private void ClearHotTrack()
        {
            this.Parent.Invalidate();
        }

        private void DrawHotTrack(Brush brush)
        {
            if (_graphics != null)
            {
                _graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                float xPos = this.TextBox.Location.X - _stroke;
                float yPos = this.TextBox.Location.Y - _stroke;
                float width = this.Size.Width + _stroke * 2;
                float height = this.Size.Height + _stroke * 2;
                _graphics.FillRectangle(brush, xPos, yPos, width, height);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
                this.Parent.Paint += Parent_Paint;
        }

        [Category("Appearance")]
        [DefaultValue(1)]
        public float Stroke
        {
            get { return _stroke; }
            set
            {
                if (value < 0)
                {
                    _stroke = 0f;
                }
                else
                {
                    _stroke = value;
                }
            }
        }

    }
}
