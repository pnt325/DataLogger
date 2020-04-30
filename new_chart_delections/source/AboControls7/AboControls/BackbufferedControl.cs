using System;
using System.Drawing;
using System.Windows.Forms;

namespace AboControls
{
    /// <summary>
    /// Represents an abstract control which supports efficient back-buffer drawing
    /// </summary>
    abstract class BackbufferedControl : Control
    {
        protected BufferedGraphics BackBuffer;

        protected BackbufferedControl()
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
                BackBuffer = bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                OnBackBufferUpdated();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateGraphicsBuffer();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            BackBuffer.Graphics.Clear(this.BackColor);
            Render();
            BackBuffer.Render(e.Graphics);
        }

        /// <summary>
        /// Provides implementation after the BackBuffer has been update
        /// (which occurs on size and in the constructor)
        /// </summary>
        protected virtual void OnBackBufferUpdated() { }

        /// <summary>
        /// Implement drawing logic here
        /// </summary>
        protected abstract void Render();
    }
}
