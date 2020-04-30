using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    class KATButton : Control
    {
        private readonly Color _topColor = Color.FromArgb(223, 205, 117);
        private readonly Color _bottomColor = Color.FromArgb(207, 188, 99);
        private BufferedGraphics _bufGFX;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private GraphicsPath _path;
        private LinearGradientBrush _LGB, _lgbDesat;
        private readonly SolidBrush _textBrush = new SolidBrush(Color.FromArgb(83, 55, 19));
        private readonly Pen _shinePen = new Pen(Color.FromArgb(236, 225, 174), 2f);
        private bool _isMouseOver;

        public KATButton()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint, true);

            _shinePen.EndCap = LineCap.Round;
            _shinePen.StartCap = LineCap.Round;
            this.Font = new Font("Tahoma", 9f);
            UpdateGraphics();
        }

        private void UpdateGraphics()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGFX = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                //_bufGFX.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(Point.Empty, this.Size);
                _LGB = new LinearGradientBrush(rect, _topColor, _bottomColor, 90f);

                Color topDarkColor = ChangeColorBrightness(_topColor, -20);
                Color bottomDarkColor = ChangeColorBrightness(_bottomColor, -20);
                _lgbDesat = new LinearGradientBrush(rect, topDarkColor, bottomDarkColor, 90f);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            _path = RoundedRectangle.Create(0, 0, this.Width - 2, this.Height - 2, 5);
            this.Region = new Region(_path);
            UpdateGraphics();
        }

        static private Color ChangeColorBrightness(Color color, int factor)
        {
            int R = (color.R + factor > 255) ? 255 : color.R + factor;
            int G = (color.G + factor > 255) ? 255 : color.G + factor;
            int B = (color.B + factor > 255) ? 255 : color.B + factor;
            if (R < 0) R = 0;
            if (B < 0) B = 0;
            if (G < 0) G = 0;
            return Color.FromArgb(R, G, B);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _bufGFX.Graphics.Clear(this.BackColor);

            if (_isMouseOver)
                _bufGFX.Graphics.FillPath(_lgbDesat, _path);
            else
                _bufGFX.Graphics.FillPath(_LGB, _path);

            // Draw shine
            _bufGFX.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _bufGFX.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _bufGFX.Graphics.DrawLine(_shinePen, new Point(3, 2), new Point(this.Width - 6, 2));
            _bufGFX.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            _bufGFX.Graphics.SmoothingMode = SmoothingMode.Default;

            // Draw Image 
            if (this.BackgroundImage != null)
            {
                int x = this.Width / 2 - this.BackgroundImage.Width / 2;
                int y = this.Height / 2 - this.BackgroundImage.Height / 2;
                _bufGFX.Graphics.DrawImage(this.BackgroundImage, new Point(x, y));
            }

            // Draw string
            SizeF stringSize = _bufGFX.Graphics.MeasureString(this.Text, this.Font);
            PointF pos = new PointF(this.Width / 2f - stringSize.Width / 2f, this.Height / 2f - stringSize.Height / 2f);
            _bufGFX.Graphics.DrawString(this.Text, this.Font, _textBrush, pos);
            _bufGFX.Render(e.Graphics);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isMouseOver = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isMouseOver = false;
            //_borderPen.Color = Color.FromArgb(200, 200, 200);
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
