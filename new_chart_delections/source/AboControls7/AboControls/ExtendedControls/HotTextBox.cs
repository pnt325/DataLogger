using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace AboControls.ExtendedControls
{
    class HotTextBox : TextBox
    {
        private Graphics _graphics;
        private float _strokeSize = 1;

        private void Parent_Paint(object sender, PaintEventArgs e)
        {
            if (this.ContainsFocus)
            {
                DrawHotTrack(Brushes.Goldenrod);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (!this.ContainsFocus)
                DrawHotTrack(Brushes.CornflowerBlue);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (!this.ContainsFocus) ClearHotTrack();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (_graphics == null)
            {
                _graphics = this.Parent.CreateGraphics();
            }

                this.Parent.Paint += Parent_Paint;
            DrawHotTrack(Brushes.Goldenrod);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ClearHotTrack();
        }

        private void ClearHotTrack()
        {
            Brush brush = new SolidBrush(this.Parent.BackColor);
            DrawHotTrack(brush);
        }

        private void DrawHotTrack(Brush brush)
        {
            if (_graphics != null)
            {
                _graphics.CompositingQuality = CompositingQuality.HighQuality;
                _graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                _graphics.SmoothingMode = SmoothingMode.AntiAlias;

                float xPos = this.Location.X - _strokeSize;
                float yPos = this.Location.Y - _strokeSize;
                float width = this.Size.Width + _strokeSize * 2f;
                float height = this.Size.Height + _strokeSize * 2f;
                _graphics.FillRectangle(brush, xPos, yPos, width, height);
            }
        }

        [Category("Appearance")]
        [DefaultValue(1)]
        public float StrokeSize
        {
            get { return _strokeSize; }
            set { _strokeSize = value; }
        }
    }
}
