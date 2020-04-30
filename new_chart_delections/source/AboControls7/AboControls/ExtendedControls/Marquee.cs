using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    public enum ScrollSpeed { Slow, Medium, Fast };

    public class Marquee : Control
    {
        private readonly Timer _tmrAnimate = new Timer();
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private PointF _textPos;
        private Brush _brush;
        private ScrollSpeed _scrollSpeed = ScrollSpeed.Fast;

        public Marquee()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |  ControlStyles.UserPaint | 
                ControlStyles.OptimizedDoubleBuffer, true);

            SetDefaults();
            UpdateBackBuffer();
            CenterMarquee();

            _tmrAnimate.Interval = 1;
            _tmrAnimate.Tick += _tmrAnimate_Tick;
            _tmrAnimate.Start();
        }

        private void SetDefaults()
        {
            base.BackColor = Color.Black;
            base.Font = new Font(this.Font.FontFamily, 50f);
            base.ForeColor = Color.DeepPink;
            Size = new Size(500, 100);
            _brush = new SolidBrush(this.ForeColor);
        }

        private void UpdateBackBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        private void CenterMarquee()
        {
            float marqueeHeight = GetStringSize().Height;
            float yPos = (this.Height / 2f) - (marqueeHeight / 2f);
            _textPos = new PointF(_textPos.X, yPos);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBackBuffer();
            CenterMarquee();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _bufGraphics.Graphics.Clear(this.BackColor);

            if (_textPos.X >= this.Width)
            {
                float xPos = -GetStringSize().Width;
                _textPos = new PointF(xPos, _textPos.Y);
            }

            _textPos = new PointF(_textPos.X + 2, _textPos.Y);
            _bufGraphics.Graphics.DrawString(this.Text, this.Font, _brush, _textPos);
            _bufGraphics.Render(e.Graphics);
        }

        private void _tmrAnimate_Tick(object sender, EventArgs e)
        {
            this.Invalidate(false);
        }

        private SizeF GetStringSize()
        {
            if (_bufGraphics == null)
                return SizeF.Empty;

            return _bufGraphics.Graphics.MeasureString(this.Text, this.Font);
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                _brush = new SolidBrush(value);
            }
        }

        [Category("Behavior")]
        [DefaultValue(ScrollSpeed.Fast)]
        public ScrollSpeed ScrollingSpeed
        {
            get { return _scrollSpeed; }
            set 
            {
                _scrollSpeed = value;

                switch (_scrollSpeed)
                {
                    case ScrollSpeed.Fast: _tmrAnimate.Interval = 1; break;
                    case ScrollSpeed.Medium: _tmrAnimate.Interval = 30; break;
                    case ScrollSpeed.Slow: _tmrAnimate.Interval = 40; break;
                }
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                CenterMarquee();
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set 
            { 
                base.Text = value;
                CenterMarquee();
            }
        }
    }
}
