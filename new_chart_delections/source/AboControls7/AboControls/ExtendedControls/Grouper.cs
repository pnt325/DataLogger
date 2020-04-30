using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace AboControls.ExtendedControls
{
    class Grouper : ContainerControl
    {
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private readonly GraphicsPath _path = new GraphicsPath();
        private readonly GraphicsPath _innerPath = new GraphicsPath();
        private RectangleF _stringRect;

        public Grouper()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            this.Padding = new Padding(3);
            UpdateGraphics();
        }

        private void UpdateStringRect() 
        {
            // Update string rect
            if (_bufGraphics != null)
            {
                SizeF size = _bufGraphics.Graphics.MeasureString(this.Text, this.Font);
                this.Padding = new Padding(Padding.Left, (int)size.Height, Padding.Right, Padding.Left);
                PointF pos = new PointF(10, 0);
                _stringRect = new RectangleF(pos, size);
            }
        }

        private void UpdateGraphics()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                if (this.Text.Equals(string.Empty))
                {
                    this.Text = "Grouper";
                }

                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;   

                UpdateStringRect();

                // Draw border
                int y = (int)(_stringRect.Height / 2 + 0.5);
                int width = this.Width - 1;
                int height = this.Height - y - 1;

                _path.Reset();

                _path.AddPath(RoundedRectangle.Create(0, y, width, height, 4,
                    RoundedRectangle.RectangleCorners.All), false);

                _innerPath.Reset();

                _innerPath.AddPath(RoundedRectangle.Create(1, y + 1, width - 2, height - 2, 4, 
                    RoundedRectangle.RectangleCorners.All), false); 
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateGraphics();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_bufGraphics != null)
            {
                _bufGraphics.Graphics.Clear(this.BackColor);
                _bufGraphics.Graphics.DrawPath(Pens.Silver, _path);
                _bufGraphics.Graphics.DrawPath(Pens.White, _innerPath);
                _bufGraphics.Graphics.FillRectangle(new SolidBrush(this.BackColor), _stringRect);
                _bufGraphics.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), _stringRect);
                _bufGraphics.Render(e.Graphics);
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateStringRect();
            this.Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            UpdateStringRect();
            this.Invalidate();
        }

        private abstract class RoundedRectangle
        {
            public enum RectangleCorners
            {
                None = 0, TopLeft = 1, TopRight = 2, BottomLeft = 4, BottomRight = 8,
                All = TopLeft | TopRight | BottomLeft | BottomRight
            }

            public static GraphicsPath Create(int x, int y, int width, int height,
                                              int radius, RectangleCorners corners)
            {
                int xw = x + width;
                int yh = y + height;
                int xwr = xw - radius;
                int yhr = yh - radius;
                int xr = x + radius;
                int yr = y + radius;
                int r2 = radius * 2;
                int xwr2 = xw - r2;
                int yhr2 = yh - r2;

                GraphicsPath p = new GraphicsPath();
                p.StartFigure();

                //Top Left Corner
                if ((RectangleCorners.TopLeft & corners) == RectangleCorners.TopLeft)
                {
                    p.AddArc(x, y, r2, r2, 180, 90);
                }
                else
                {
                    p.AddLine(x, yr, x, y);
                    p.AddLine(x, y, xr, y);
                }

                //Top Edge
                p.AddLine(xr, y, xwr, y);

                //Top Right Corner
                if ((RectangleCorners.TopRight & corners) == RectangleCorners.TopRight)
                {
                    p.AddArc(xwr2, y, r2, r2, 270, 90);
                }
                else
                {
                    p.AddLine(xwr, y, xw, y);
                    p.AddLine(xw, y, xw, yr);
                }

                //Right Edge
                p.AddLine(xw, yr, xw, yhr);

                //Bottom Right Corner
                if ((RectangleCorners.BottomRight & corners) == RectangleCorners.BottomRight)
                {
                    p.AddArc(xwr2, yhr2, r2, r2, 0, 90);
                }
                else
                {
                    p.AddLine(xw, yhr, xw, yh);
                    p.AddLine(xw, yh, xwr, yh);
                }

                //Bottom Edge
                p.AddLine(xwr, yh, xr, yh);

                //Bottom Left Corner
                if ((RectangleCorners.BottomLeft & corners) == RectangleCorners.BottomLeft)
                {
                    p.AddArc(x, yhr2, r2, r2, 90, 90);
                }
                else
                {
                    p.AddLine(xr, yh, x, yh);
                    p.AddLine(x, yh, x, yhr);
                }

                //Left Edge
                p.AddLine(x, yhr, x, yr);

                p.CloseFigure();
                return p;
            }

            public static GraphicsPath Create(int x, int y, int width, int height, int radius)
            { return Create(x, y, width, height, radius, RectangleCorners.All); }
        }
    }
}
