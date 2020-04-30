using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    class GoogleButton : Control
    {
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private GraphicsPath _path;
        private LinearGradientBrush _LGB;
        private SolidBrush _textBrush = new SolidBrush(Color.FromArgb(68, 68, 68));
        private readonly Pen _dropShadowPen;
        private readonly Pen _borderPen = new Pen(Color.DarkGray, 1f);
        private readonly Pen _innerShadow = new Pen(Color.FromArgb(235, 235, 235), 2);
        private bool _isMouseOver, _isMouseDown;

        public GoogleButton()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint 
                | ControlStyles.AllPaintingInWmPaint, true);

            Color darker = ChangeColorBrightness(this.BackColor, -30);
            _dropShadowPen = new Pen(darker, 1);

            this.Font = new Font("Tahoma", 9f, FontStyle.Bold);
            UpdateGraphics();
        }

        private void UpdateGraphics()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
               _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

               Rectangle rect = new Rectangle(Point.Empty, this.Size);
               _LGB = new LinearGradientBrush(rect, Color.FromArgb(240, 240, 240), Color.FromArgb(235, 235, 235), 90f);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateGraphics();
            _path = RoundedRectangle.Create(0, 0, this.Width - 1, this.Height - 3, 2);
        }

        static public Color ChangeColorBrightness(Color color, int factor)
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
            _bufGraphics.Graphics.Clear(this.BackColor);
            // Paint here
            _bufGraphics.Graphics.FillPath(_LGB, _path);
            _bufGraphics.Graphics.DrawPath(_borderPen, _path);

            if (_isMouseOver)
            {
                // Draw shadow
                _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                _bufGraphics.Graphics.DrawLine(_dropShadowPen, new Point(2, this.Height - 2), 
                    new Point(this.Width - 4, this.Height - 2));

                _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            }

            // Draw inner shadow
            if (_isMouseDown)
            {
                _bufGraphics.Graphics.DrawLine(_innerShadow, new Point(2, 2), new Point(this.Width - 2, 2));
                Point p2 = new Point(this.Width - 3, this.Height - 4);
                _bufGraphics.Graphics.DrawLine(_innerShadow, new Point(this.Width - 3, 2), p2);
            }

            // Draw string
            SizeF stringSize = _bufGraphics.Graphics.MeasureString(this.Text, this.Font);
            PointF pos = new PointF(this.Width / 2f - stringSize.Width / 2, this.Height / 2 - stringSize.Height / 2);
            _bufGraphics.Graphics.DrawString(this.Text, this.Font, _textBrush, pos);
            _bufGraphics.Render(e.Graphics);
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
            _textBrush = new SolidBrush(Color.Black);
            _isMouseOver = true;
            _borderPen.Color = Color.DarkGray;

            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isMouseDown = true;
            _isMouseOver = false;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isMouseDown = false;
            _isMouseOver = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _textBrush = new SolidBrush(Color.FromArgb(68, 68, 68));
            _isMouseOver = false;
            _borderPen.Color = Color.FromArgb(200, 200, 200);
            this.Invalidate();
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                _dropShadowPen.Color = ChangeColorBrightness(value, -10);
                base.BackColor = value;
            }
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
