using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    /// <summary>
    /// Determines how the progress is displayed as numbers
    /// </summary>
    enum NumericDisplayStyle 
    {
        /// <summary>
        /// The numbers will always be hidden
        /// </summary>
        Hidden, 
        /// <summary>
        /// The numbers will always be visible
        /// </summary>
        Shown, 
        /// <summary>
        /// The numbers will only be shown when the mouse is over the control
        /// </summary>
        MouseOver 
    };

    /// <summary>
    /// Specifies what borders will be drawn
    /// </summary>
    [Flags]
    enum BorderSides
    {
        /// <summary>
        /// No borders will be drawn
        /// </summary>
        None = 0,
        /// <summary>
        /// The inner border (around the center) will be drawn
        /// </summary>
        Inner = 1,
        /// <summary>
        /// The outer border (around the edge of the progress) will be drawn
        /// </summary>
        Outer = 2,
        /// <summary>
        /// Both borders will be drawn
        /// </summary>
        Both = Inner | Outer
    };

    [DefaultEvent("ProgressChanged")]
    class ProgressIndicator : Control
    {
        private readonly SolidBrush _sectionBrush = new SolidBrush(Color.Transparent);
        private readonly SolidBrush _centerBrush = new SolidBrush(Color.Transparent);
        private readonly SolidBrush _progressBrush = new SolidBrush(Color.YellowGreen);
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private float _value, _centerScale = .70f;
        private bool _showPercentSymbol, _hasMouse;
        private int _sectionCount = 6;
        private int _sectionWidth = 10;
        private readonly Pen _borderPen = new Pen(Color.Black, 4);
        private BorderSides _borderSides = BorderSides.Both;
        private NumericDisplayStyle _numericDisplayStyle = NumericDisplayStyle.Hidden;

        [Description("Occurs when the value of the Progress property has changed")]
        public event EventHandler ProgressChanged;

        public ProgressIndicator()
        {
            this.Size = new Size(60, 60);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.SupportsTransparentBackColor, true);

            UpdateGraphicsBuffer();
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                _bufGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                _bufGraphics.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateGraphicsBuffer();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hasMouse = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hasMouse = false;
        }

        #region Painting
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawBackground();
            DrawProgress();
            if (_sectionCount > 0) DrawSectioners();
            DrawCenter();
            if (_borderPen.Width != 0f) DrawBorder();
            DrawNumericPercentage();
            if (this.BackgroundImage != null) DrawBackgroundImage();
            _bufGraphics.Render(e.Graphics);
        }

        private void DrawBackgroundImage()
        {
            if (this.BackgroundImageLayout == ImageLayout.Center)
            {
                float x = this.Width / 2f - this.BackgroundImage.Width / 2f;
                float y = this.Height / 2f - this.BackgroundImage.Height / 2f;
                _bufGraphics.Graphics.DrawImage(this.BackgroundImage, new PointF(x, y));
            }
            else if (this.BackgroundImageLayout == ImageLayout.Stretch)
            {
                _bufGraphics.Graphics.DrawImage(this.BackgroundImage, this.ClientRectangle);
            }
        }

        private void DrawSectioners()
        {
            int sectionSize = 360 / _sectionCount;
            SolidBrush brush = (_sectionBrush.Color == Color.Transparent) ? new SolidBrush(this.BackColor) : _sectionBrush;

            for (int i = 0; i < _sectionCount; i++)
            {
                int angle = i * sectionSize - _sectionWidth / 2;
                _bufGraphics.Graphics.FillPie(brush, this.ClientRectangle, angle, _sectionWidth);
            }
        }

        private void DrawBorder()
        {
            if (_borderSides.HasFlag(BorderSides.Outer))
            {
                // Draw around edge
                float xy = _borderPen.Width / 2f;
                float width = this.Width - _borderPen.Width;
                float height = this.Height - _borderPen.Width;
                _bufGraphics.Graphics.DrawEllipse(_borderPen, xy, xy, width, height);
            }

            if (_borderSides.HasFlag(BorderSides.Inner))
            {
                // Draw around center
                float centerWidth = this.Width * _centerScale;
                float centerHeight = this.Height * _centerScale;
                float x = this.Width / 2f - centerWidth / 2f;
                float y = this.Height / 2f - centerHeight / 2f;
                _bufGraphics.Graphics.DrawEllipse(_borderPen, x, y, centerWidth, centerHeight);
            }
        }

        /// <summary>
        /// If the controls backColor is set to transparent, then draw the background 
        /// with the control's parent's backColor
        /// </summary>
        private void DrawBackground()
        {
            if (this.BackColor == Color.Transparent && this.Parent != null)
                _bufGraphics.Graphics.Clear(this.Parent.BackColor);
            else
                _bufGraphics.Graphics.Clear(this.BackColor);
        }

        private void DrawProgress()
        {
            // Draw progress
            float angle = 360 * _value;
            _bufGraphics.Graphics.FillPie(_progressBrush, this.ClientRectangle, 0, angle);
        }

        private void DrawCenter()
        {
            float width = this.Width * _centerScale;
            float height = this.Height * _centerScale;
            float x = this.Width / 2f - width / 2f;
            float y = this.Height / 2f - height / 2f;
            Color color = _centerBrush.Color.Equals(Color.Transparent) ? this.Parent.BackColor : _centerBrush.Color;
            _bufGraphics.Graphics.FillEllipse(new SolidBrush(color), x, y, width, height);
        }

        private void DrawNumericPercentage()
        {
            if (_numericDisplayStyle == NumericDisplayStyle.Shown ||
                _numericDisplayStyle == NumericDisplayStyle.MouseOver && _hasMouse)
            {
                int progress = (int)(this.Value * 100 + 0.5);
                string text = progress.ToString(CultureInfo.InvariantCulture);
                if (_showPercentSymbol) text += '%';
                SizeF textSize = _bufGraphics.Graphics.MeasureString(text, this.Font);
                SolidBrush brush = new SolidBrush(this.ForeColor);
                float x = this.Width / 2f - textSize.Width / 2f;
                float y = this.Height / 2f - textSize.Height / 2f;
                _bufGraphics.Graphics.DrawString(text, this.Font, brush, new PointF(x, y));
            }
        }
        #endregion

        #region Properties
        [DefaultValue(0)]
        [Description("The value from 0-1 indicating progress")]
        [Category("Appearance")]
        public float Value
        {
            get { return _value; }
            set
            {
                bool changed = _value != value;
                if (value < 0) _value = 0;
                else if (value > 1) _value = 1;
                else _value = value;

                if (changed && ProgressChanged != null)
                {
                    ProgressChanged(this, EventArgs.Empty);
                }

                this.Invalidate();
            }
        }

        [DefaultValue(.6f)]
        [Category("Appearance")]
        [Description("The scale of the center in relation to the size of the control")]
        public float CenterScale
        {
            get { return _centerScale; }
            set
            {
                if (value > 1) _centerScale = 1;
                else _centerScale = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "YellowGreen")]
        [Category("Appearance")]
        [Description("The color of the area that indicates progress")]
        public Color ProgressColor
        {
            get { return _progressBrush.Color; }
            set
            {
                _progressBrush.Color = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        [Category("Appearance")]
        [Description("The color of the center area of the control")]
        public Color CenterColor
        {
            get { return _centerBrush.Color; }
            set
            {
                _centerBrush.Color = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Black")]
        [Category("Appearance")]
        [Description("The color of the border")]
        public Color BorderColor
        {
            get { return _borderPen.Color; }
            set
            {
                _borderPen.Color = value;
                this.Invalidate();
            }
        }

        [DefaultValue(0)]
        [Category("Appearance")]
        [Description("The width of the border")]
        public float BorderThickness
        {
            get { return _borderPen.Width; }
            set
            {
                _borderPen.Width = value;
                this.Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Determines whether to show the percent symbol next to the percent number")]
        public bool ShowPercentSymbol
        {
            get { return _showPercentSymbol; }
            set
            {
                _showPercentSymbol = value;
                this.Invalidate();
            }
        }

        [DefaultValue(BorderSides.Both)]
        [Category("Appearance")]
        [Description("Determines what borders will be drawn")]
        public BorderSides BorderSides
        {
            get { return _borderSides; }
            set
            {
                _borderSides = value;
                this.Invalidate();
            }
        }

        [DefaultValue(6)]
        [Category("Appearance")]
        [Description("Specifies how many sections there will be")]
        public int SectionCount
        {
            get { return _sectionCount; }
            set
            {
                _sectionCount = value;
                this.Invalidate();
            }
        }

        [DefaultValue(20)]
        [Category("Appearance")]
        [Description("Specifies how big the section seperators will be")]
        public int SectionSweep
        {
            get { return _sectionWidth; }
            set
            {
                _sectionWidth = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "transparent")]
        [Category("Appearance")]
        [Description("Specifies the color the sections")]
        public Color SectionColor
        {
            get { return _sectionBrush.Color; }
            set
            {
                _sectionBrush.Color = value;
                this.Invalidate();
            }
        }

        [DefaultValue(NumericDisplayStyle.Hidden)]
        [Category("Appearance")]
        [Description("Specifies how to show the numeric representation of progress")]
        public NumericDisplayStyle NumericDisplayStyle
        {
            get { return _numericDisplayStyle; }
            set
            {
                _numericDisplayStyle = value;
                this.Invalidate();
            }
        }
        #endregion
    }
}
