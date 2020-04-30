using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace AboControls.ExtendedControls
{
    /// <summary>
    /// Specifies what element to draw in the center of the control
    /// </summary>
    public enum CenterElement
    {
        /// <summary>
        /// Draw nothing in the center
        /// </summary>
        None,
        /// <summary>
        /// Draw the value of the Text property in the center
        /// </summary>
        Text,
        /// <summary>
        /// Draw the value of the Percent property in the center
        /// </summary>
        Percent
    }

    /// <summary>
    /// Specifies what style to use for a SuperProgressBar
    /// </summary>
    public enum ProgressStyle
    {
        /// <summary>
        /// The control will animate from left to right or top to bottom
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The control will animate from right to left or bottom to top
        /// </summary>
        Reversed,
        /// <summary>
        /// The control will animate from the outer edges inward
        /// </summary>
        Inwards,
        /// <summary>
        /// The control will animate from the center to the edge of the control
        /// </summary>
        Outwards
    }

    /// <summary>
    /// Represents a progress indicator in the form of a bar to display both calculated
    /// and undetermined progress
    /// </summary>
    public class AboProgressBar : Control
    {
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private readonly Timer _tmrMarquee = new Timer();
        private LinearGradientBrush _foreLgb, _backLgb;
        private BufferedGraphics _bufGraphics;
        private Brush _foreBrush;
        private int _marqueePos;

        public AboProgressBar()
        {
            _tmrMarquee.Tick += _tmrMarquee_Tick;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);

            // Set defaults
            GradientIntensity = 20;
            MarqueeWidth = 80;
            _tmrMarquee.Interval = 3;
            _foreBrush = new SolidBrush(base.ForeColor);
            base.MinimumSize = new Size(10, 10);
            base.Size = new Size(200, 23);
            base.BackColor = Color.DimGray;
        }

        #region Virtual and Overridden
        [Description("Occurs when the progress property has changed and the control has invalidated")]
        public event EventHandler ProgressChanged;
        /// <summary>
        /// Raises the ProgressChanged event
        /// </summary>
        protected virtual void OnProgressChanged()
        {
            if (ProgressChanged != null)
                ProgressChanged(this, EventArgs.Empty);
        }

        [Description("Occurs when progress reaches 100%")]
        public event EventHandler ProgressCompleted;
        /// <summary>
        /// Raises the ProgressCompleted event
        /// </summary>
        protected virtual void OnProgressCompleted()
        {
            if (ProgressCompleted != null)
                ProgressCompleted(this, EventArgs.Empty);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            UpdateLgBrushes();
            this.Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_autoSetOrientation) AutoSetIsVertical();
            // Update back buffer
            _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
            UpdateLgBrushes();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Draw grey backdrop
            _bufGraphics.Graphics.FillRectangle(_backLgb, this.ClientRectangle);

            if (_useMarquee) DrawMarquee();
            else if (_vertical) DrawVerticalProgress();
            else DrawHorizProgress();

            // Draw border
            if (_showBorder)
            {
                Rectangle rect = this.ClientRectangle;
                _bufGraphics.Graphics.DrawRectangle(_borderPen, rect.X,
                    rect.Y, rect.Width - 1, rect.Height - 1);
            }

            DrawCenterElement();
            _bufGraphics.Render(e.Graphics);
        }
        #endregion

        private void AutoSetIsVertical()
        {
            Vertical = (this.Width < this.Height);
        }

        private void _tmrMarquee_Tick(object sender, EventArgs e)
        {
            if (this.Visible && this.Enabled)
            {
                if ((_vertical && _marqueePos < this.Height) ||
                    (!_vertical && _marqueePos < this.Width))
                {
                    _marqueePos += 2;
                }
                else _marqueePos = 0;
                this.Invalidate();
            }
        }

        private void UpdateLgBrushes()
        {
            if (this.Width <= 0 || this.Height <= 0) return;

            int angle = (_vertical) ? 0 : 90;
            Color darkColor;
            Color darkColor2;

            if (this.Enabled)
            {
                darkColor = ChangeColorBrightness(this.BackColor, _gradientIntensity);
                darkColor2 = ChangeColorBrightness(_progressColor, _gradientIntensity);
            }
            else
            {
                darkColor = ChangeColorBrightness(Desaturate(this.BackColor), _gradientIntensity);
                darkColor2 = ChangeColorBrightness(Desaturate(_progressColor), _gradientIntensity);
            }

            Rectangle rect = (!_vertical) ? new Rectangle(0, 0, this.Width, this.Height / 2) :
                                 new Rectangle(0, 0, this.Width / 2, this.Height);

            if (this.Enabled)
            {
                _backLgb = new LinearGradientBrush(rect, this.BackColor, darkColor, angle, true);
                _foreLgb = new LinearGradientBrush(rect, _progressColor, darkColor2, angle, true);
            }
            else
            {
                _backLgb = new LinearGradientBrush(rect, Desaturate(this.BackColor), darkColor, angle, true);
                _foreLgb = new LinearGradientBrush(rect, Desaturate(_progressColor), darkColor2, angle, true);
            }

            _backLgb.WrapMode = WrapMode.TileFlipX;
            _foreLgb.WrapMode = WrapMode.TileFlipX;
        }

        #region Drawing
        private static Color ChangeColorBrightness(Color color, int factor)
        {
            int R = (color.R + factor > 255) ? 255 : color.R + factor;
            int G = (color.G + factor > 255) ? 255 : color.G + factor;
            int B = (color.B + factor > 255) ? 255 : color.B + factor;
            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;
            return Color.FromArgb(R, G, B);
        }

        private static Color Desaturate(Color color)
        {
            int b = (int)(255 * color.GetBrightness());
            return Color.FromArgb(b, b, b);
        }

        private void DrawMarquee()
        {
            if (_vertical)
            {
                int smallSect = this.Height - _marqueePos;
                int largeSect = MarqueeWidth - smallSect;

                if (largeSect > 0)
                    _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, 0, this.Width, largeSect);

                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, _marqueePos, this.Width, MarqueeWidth);
            }
            else
            {
                int smallSect = this.Width - _marqueePos;
                int largeSect = MarqueeWidth - smallSect;

                if (largeSect > 0)
                    _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, 0, largeSect, this.Height);

                _bufGraphics.Graphics.FillRectangle(_foreLgb, _marqueePos, 0, MarqueeWidth, this.Height);
            }
        }

        private void DrawHorizProgress()
        {
            if (Style == ProgressStyle.Normal)
            {
                float width = this.Width * (_progress / 100f);
                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, 0, width, this.Height);
            }
            else if (Style == ProgressStyle.Reversed)
            {
                float pixelPercent = this.Width * (_progress / 100f);
                _bufGraphics.Graphics.FillRectangle(_foreLgb, this.Width - pixelPercent, 0, pixelPercent, this.Height);
            }
            else if (Style == ProgressStyle.Inwards)
            {
                float pixelPercent = this.Width * (_progress / 100f) / 2f;
                _bufGraphics.Graphics.FillRectangle(_foreLgb, this.Width - pixelPercent, 0, pixelPercent, this.Height);
                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, 0, pixelPercent, this.Height);
            }
            else if (Style == ProgressStyle.Outwards)
            {
                float pixelPercent = this.Width * (_progress / 100f);
                float x = this.Width / 2f - pixelPercent / 2f;
                _bufGraphics.Graphics.FillRectangle(_foreLgb, x, 0, pixelPercent, this.Height);
            }
        }

        private void DrawCenterElement()
        {
            if (_centerElement == CenterElement.None) return;
            string text;

            if (_centerElement == CenterElement.Percent)
                text = ((int)(_progress + 0.5f)).ToString(CultureInfo.InvariantCulture) + '%';
            else
                text = this.Text;

            float strWidth = _bufGraphics.Graphics.MeasureString(text, this.Font).Width;
            float strHeight = _bufGraphics.Graphics.MeasureString(text, this.Font).Height;
            float xPos = this.Width / 2f - strWidth / 2f;
            float yPos = this.Height / 2f - strHeight / 2f;
            PointF p1 = new PointF(xPos, yPos);
            _bufGraphics.Graphics.DrawString(text, this.Font, _foreBrush, p1);
        }

        private void DrawVerticalProgress()
        {
            float pixelPercent = this.Height * (_progress / 100f);

            if (Style == ProgressStyle.Normal)
            {
                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, 0, this.Width, pixelPercent);
            }
            else if (Style == ProgressStyle.Reversed)
            {
                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, this.Height - pixelPercent, this.Width, pixelPercent);
            }
            else if (Style == ProgressStyle.Inwards)
            {
                float temp = pixelPercent / 2f;
                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, this.Height - temp, this.Width, temp);
                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, 0, this.Width, temp);
            }
            else if (Style == ProgressStyle.Outwards)
            {
                float y = this.Height / 2f - pixelPercent / 2f;
                _bufGraphics.Graphics.FillRectangle(_foreLgb, 0, y, this.Width, pixelPercent);
            }
        }
        #endregion

        #region Properties
        private float _progress;
        [Description("The value ranging from 0-100 which represents progress")]
        [DefaultValue(0)]
        [Category("Data")]
        public float Progress
        {
            get { return _progress; }
            set
            {
                if (value >= 100)
                {
                    _progress = 100;
                    OnProgressCompleted();
                }
                else if (value < 0) _progress = 0;
                else _progress = value;

                this.Invalidate();
                OnProgressChanged();
            }
        }

        [DefaultValue(typeof(Color), "DimGray")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                UpdateLgBrushes();
                this.Invalidate();
            }
        }

        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                _foreBrush = new SolidBrush(this.ForeColor);
                this.Invalidate();
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                this.Invalidate();
            }
        }

        private Pen _borderPen = new Pen(Color.Black, 1f);
        [Category("Appearance")]
        [Description("The color of the border")]
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return _borderPen.Color; }
            set
            {
                _borderPen = new Pen(value, 1f);
                this.Invalidate();
            }
        }

        private Color _progressColor = Color.YellowGreen;
        [Category("Appearance")]
        [Description("The color of the block of progress")]
        [DefaultValue(typeof(Color), "YellowGreen")]
        public Color ProgressColor
        {
            get { return _progressColor; }
            set
            {
                _progressColor = value;
                UpdateLgBrushes();
                this.Invalidate();
            }
        }

        private ProgressStyle _progressStyle;
        [DisplayName("Progress Style"), DefaultValue(ProgressStyle.Normal)]
        [Description("The progress animation style"), Category("Appearance")]
        public ProgressStyle Style
        {
            get { return _progressStyle; }
            set
            {
                _progressStyle = value;
                this.Invalidate();
            }
        }

        private CenterElement _centerElement;
        [Description("The element to draw at the center")]
        [Category("Appearance")]
        [DisplayName("Center Element")]
        [DefaultValue(CenterElement.None)]
        public CenterElement CenterText
        {
            get { return _centerElement; }
            set
            {
                _centerElement = value;
                this.Invalidate();
            }
        }

        private bool _vertical;
        [Description("Determines the orientation of the progress bar")]
        [Category("Layout")]
        [DefaultValue(false)]
        public bool Vertical
        {
            get { return _vertical; }
            set
            {
                bool changed = _vertical != value;

                if (changed)
                {
                    _vertical = value;
                    UpdateLgBrushes();
                    this.Invalidate();
                }
            }
        }

        private bool _useMarquee;
        [Description("Determines whether to use the marquee in opposed to direction")]
        [Category("Appearance")]
        [DisplayName("Use Marquee")]
        [DefaultValue(false)]
        public bool UseMarquee
        {
            get { return _useMarquee; }
            set
            {
                _useMarquee = value;
                _tmrMarquee.Enabled = value;
                this.Invalidate();
            }
        }

        [Description("The width of the animated marquee")]
        [Category("Appearance")]
        [DefaultValue(80)]
        public int MarqueeWidth { get; set; }

        private int _gradientIntensity;
        [Description("The intensity of the outer color in relation to the inner")]
        [Category("Appearance")]
        [DefaultValue(20)]
        public int GradientIntensity
        {
            get { return _gradientIntensity; }
            set
            {
                _gradientIntensity = value;
                UpdateLgBrushes();
                this.Invalidate();
            }
        }

        private bool _showBorder;
        [Description("Whether to show the 1 pixel border or not")]
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool ShowBorder
        {
            get { return _showBorder; }
            set
            {
                _showBorder = value;
                this.Invalidate();
            }
        }

        private bool _autoSetOrientation = true;
        [Description("When true, automatically sets the Vertical property according to the controls size")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AutoSetOrientation
        {
            get { return _autoSetOrientation; }
            set 
            { 
                _autoSetOrientation = value;
                if (_autoSetOrientation) AutoSetIsVertical();
                this.Invalidate();
            }
        }
        #endregion
    }
}

