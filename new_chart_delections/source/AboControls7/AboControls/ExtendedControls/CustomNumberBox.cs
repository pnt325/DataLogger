using System;
using System.Drawing;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    class CustomNumberBox : Control
    {
        private BufferedGraphics _bufGraphics;

        public CustomNumberBox()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateGraphicsBuffer();
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                BufferedGraphicsContext bufContext = BufferedGraphicsManager.Current;
                bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateGraphicsBuffer();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _bufGraphics.Graphics.Clear(this.BackColor);
            //base.OnPaint(e);
            _bufGraphics.Render(e.Graphics);
        }
    }
}
