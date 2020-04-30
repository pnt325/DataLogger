using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace AboControls.ExtendedControls
{
    class StarRatingStrip : Control
    {
        public enum StarType { Normal, Fat, Detailed };
        private StarType _starType = StarType.Fat;
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private Pen _starStroke = new Pen(Color.Gold, 3f);
        private Pen _starDullStroke = new Pen(Color.Gray, 3f);
        private SolidBrush _starBrush = new SolidBrush(Color.Yellow);
        private SolidBrush _starDullBrush = new SolidBrush(Color.Silver);
        private int _starCount = 5;
        private int _starSpacing = 1;
        private int _starWidth = 25;
        private float _mouseOverIndex = -1;
        private float _rating;
        private bool _allowHalfStarRating, _settingRating;

        [Description("Occurs when a different number of stars are illuminated (does not include mouseleave un-ilum)")]
        public event EventHandler StarsPanned;
        [Description("Occurs when the star rating of the strip has changed (Typically by a click operation)")]
        public event EventHandler RatingChanged;

        public StarRatingStrip()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            SetPenBrushDefaults();
            this.Size = new Size(200, 100);
            UpdateGraphicsBuffer();
        }

        /// <summary>
        /// Rounds precise numbers to a number no more precise than .5
        /// </summary>
        private static float RoundToNearestHalf(float f)
        {
            return (float)Math.Round(f / 5.0, 1) * 5f;
        }

        private void DrawDullStars()
        {
            float height = this.Height - _starStroke.Width;
            float lastX = _starStroke.Width / 2f; // Start off at stroke size and increment
            float width = (this.Width - TotalSpacing - TotalStrokeWidth) / (float)_starCount;

            // Draw stars
            for (int i = 0; i < _starCount; i++)
            {
                RectangleF rect = new RectangleF(lastX, _starStroke.Width / 2f, width, height);
                PointF[] polygon = GetStarPolygon(rect);
                _bufGraphics.Graphics.FillPolygon(_starDullBrush, polygon);
                _bufGraphics.Graphics.DrawPolygon(_starDullStroke, polygon);
                lastX += _starWidth + _starSpacing + _starStroke.Width;
                _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                _bufGraphics.Graphics.FillPolygon(_starDullBrush, polygon);
                _bufGraphics.Graphics.DrawPolygon(_starDullStroke, polygon);
                _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            }
        }

        private void DrawIllumStars()
        {
            float height = this.Height - _starStroke.Width;
            float lastX = _starStroke.Width / 2f; // Start off at stroke size and increment
            float width = (this.Width - TotalSpacing - TotalStrokeWidth) / _starCount;

            if (_allowHalfStarRating)
            {
                // Draw stars
                for (int i = 0; i < _starCount; i++)
                {
                    RectangleF rect = new RectangleF(lastX, _starStroke.Width / 2f, width, height);

                    if (i < _mouseOverIndex - 0.5f)
                    {
                        PointF[] polygon = GetStarPolygon(rect);
                        _bufGraphics.Graphics.FillPolygon(_starBrush, polygon);
                        _bufGraphics.Graphics.DrawPolygon(_starStroke, polygon);
                    }
                    else if (i == _mouseOverIndex - 0.5f)
                    {
                        PointF[] polygon = GetSemiStarPolygon(rect);
                        _bufGraphics.Graphics.FillPolygon(_starBrush, polygon);
                        _bufGraphics.Graphics.DrawPolygon(_starStroke, polygon);
                    }
                    else
                    {
                        break;
                    }

                    lastX += _starWidth + _starSpacing + _starStroke.Width;
                }
            }
            else
            {
                // Draw stars
                for (int i = 0; i < _starCount; i++)
                {
                    RectangleF rect = new RectangleF(lastX, _starStroke.Width / 2f, width, height);
                    PointF[] polygon = GetStarPolygon(rect);

                    if (i <= _mouseOverIndex)
                    {
                        _bufGraphics.Graphics.FillPolygon(_starBrush, polygon);
                        _bufGraphics.Graphics.DrawPolygon(_starStroke, polygon);
                    }
                    else
                    {
                        break;
                    }

                    lastX += _starWidth + _starSpacing + _starStroke.Width;
                }
            }
        }

        private PointF[] GetStarPolygon(RectangleF rect)
        {
            switch (_starType)
            {
                case StarType.Normal: return GetNormalStar(rect);
                case StarType.Fat: return GetFatStar(rect);
                case StarType.Detailed: return GetDetailedStar(rect);
                default: return null;
            }
        }

        private PointF[] GetSemiStarPolygon(RectangleF rect)
        {
            switch (_starType)
            {
                case StarType.Normal: return GetNormalSemiStar(rect);
                case StarType.Fat: return GetFatSemiStar(rect);
                case StarType.Detailed: return GetDetailedSemiStar(rect);
                default: return null;
            }
        }

        private float GetHoveredStarIndex(Point pos)
        {
            if (_allowHalfStarRating)
            {
                float widthSection = this.Width / (float)_starCount / 2f;

                for (float i = 0f; i < _starCount; i += 0.5f)
                {
                    float starX = i * widthSection * 2f;

                    // If cursor is within the x region of the iterated star
                    if (pos.X >= starX && pos.X <= starX + widthSection)
                    {
                        return i + 0.5f;
                    }
                }

                return -1;
            }
            else
            {
                int widthSection = (int)(this.Width / (double)_starCount + 0.5);

                for (int i = 0; i < _starCount; i++)
                {
                    float starX = i * widthSection;

                    // If cursor is within the x region of the iterated star
                    if (pos.X >= starX && pos.X <= starX + widthSection)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_rating > 0) return;
            float index = GetHoveredStarIndex(e.Location);

            if (index != _mouseOverIndex)
            {
                _mouseOverIndex = index;
                OnStarsPanned();
                this.Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_rating == 0f)
            {
                _settingRating = true;
                Rating = (_allowHalfStarRating) ? _mouseOverIndex : _mouseOverIndex + 1f;
                _settingRating = false;
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_rating > 0) return;
            _mouseOverIndex = -1; // No stars will be highlighted
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateSize();
            UpdateGraphicsBuffer();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _bufGraphics.Graphics.Clear(this.BackColor);
            DrawDullStars();
            DrawIllumStars();
            _bufGraphics.Render(e.Graphics);
        }

        private void UpdateSize()
        {
            int height = (int)(_starWidth + _starStroke.Width + 0.5);
            int width = (int)(TotalStarWidth + TotalSpacing + TotalStrokeWidth + 0.5);
            this.Size = new Size(width, height);
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        private void SetPenBrushDefaults()
        {
            _starStroke.LineJoin = LineJoin.Round;
            _starStroke.Alignment = PenAlignment.Outset;
            _starDullStroke.LineJoin = LineJoin.Round;
            _starDullStroke.Alignment = PenAlignment.Outset;
        }

        protected virtual void OnStarsPanned()
        {
            if (StarsPanned != null)
            {
                StarsPanned(this, EventArgs.Empty);
            }
        }

        protected virtual void OnRatingChanged()
        {
            if (RatingChanged != null)
            {
                RatingChanged(this, EventArgs.Empty);
            }
        }

        #region Properties
        [Browsable(false)]
        public SolidBrush StarBrush
        {
            get { return _starBrush; }
            set { _starBrush = value; }
        }

        [Browsable(false)]
        public SolidBrush StarDullBrush
        {
            get { return _starDullBrush; }
            set { _starDullBrush = value; }
        }

        [Browsable(false)]
        public Pen StarStroke
        {
            get { return _starStroke; }
            set { _starStroke = value; }
        }

        [Browsable(false)]
        public Pen StarDullStroke
        {
            get { return _starDullStroke; }
            set { _starDullStroke = value; }
        }

        [Browsable(false)]
        public float MouseOverStarIndex
        {
            get { return _mouseOverIndex; }
        }

        /// <summary>
        /// Gets all of the spacing between the stars
        /// </summary>
        private int TotalSpacing
        {
            get { return (_starCount - 1) * _starSpacing; }
        }

        /// <summary>
        /// Gets the sum of the width of the stroke for each star
        /// </summary>
        private float TotalStrokeWidth
        {
            get { return _starCount * _starStroke.Width; }
        }

        /// <summary>
        /// Gets the sum of all star widths
        /// </summary>
        private int TotalStarWidth
        {
            get { return _starCount * _starWidth; }
        }

        [Description("The color to use for the star when they are illuminated")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color StarColor
        {
            get { return _starBrush.Color; }
            set
            {
                _starBrush.Color = value;
                this.Invalidate();
            }
        }

        [Description("The color to use for the star borders when they are illuminated")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Gold")]
        public Color StarBorderColor
        {
            get { return _starStroke.Color; }
            set
            {
                _starStroke.Color = value;
                this.Invalidate();
            }
        }

        [Description("The color to use for the stars when they are not illuminated")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Silver")]
        public Color StarDullColor
        {
            get { return _starDullBrush.Color; }
            set
            {
                _starDullBrush.Color = value;
                this.Invalidate();
            }
        }

        [Description("The color to use for the star borders when they are not illuminated")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color StarDullBorderColor
        {
            get { return _starDullStroke.Color; }
            set
            {
                _starDullStroke.Color = value;
                this.Invalidate();
            }
        }

        [Description("The amount of space between each star")]
        [Category("Layout")]
        [DefaultValue(1)]
        public int StarSpacing
        {
            get { return _starSpacing; }
            set
            {
                _starSpacing = _starSpacing < 0 ? 0 : value;
                UpdateSize();
                this.Invalidate();
            }
        }

        [Description("The width and height of the star in pixels (not including the border)")]
        [Category("Layout")]
        [DefaultValue(25)]
        public int StarWidth
        {
            get { return _starWidth; }
            set
            {
                _starWidth = _starWidth < 1 ? 1 : value;
                UpdateSize();
                this.Invalidate();
            }
        }

        [Description("The number of stars selected (Note: 0 is considered un-rated")]
        [DefaultValue(0f)]
        public float Rating
        {
            get { return _rating; }
            set
            {
                if (value > _starCount) value = _starCount; // bounds check
                else if (value < 0) value = 0;
                else
                {
                    // Rounding to whole number or near .5 appropriately
                    if (_allowHalfStarRating) value = RoundToNearestHalf(value);
                    else value = (int)(value + 0.5f);
                }

                bool changed = value != _rating;
                _rating = value;

                if (changed)
                {
                    if (!_settingRating)
                    {
                        _mouseOverIndex = _rating;
                        if (!_allowHalfStarRating) _mouseOverIndex -= 1f;
                    }

                    OnRatingChanged();
                    this.Invalidate();
                }
            }
        }

        [Description("The number of stars to display")]
        [Category("Appearance")]
        [DefaultValue(5)]
        public int StarCount
        {
            get { return _starCount; }
            set
            {
                bool changed = _starCount != value;
                _starCount = value;

                if (changed)
                {
                    UpdateSize();
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the preset appearance of the star
        /// </summary>
        [Description("The star style to use")]
        [Category("Appearance")]
        [DefaultValue(StarType.Fat)]
        public StarType TypeOfStar
        {
            get { return _starType; }
            set
            {
                _starType = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the width of the border around the star (including the dull version)
        /// </summary>
        [Description("The width of the star border")]
        [Category("Appearance")]
        [DefaultValue(3f)]
        public float StarBorderWidth
        {
            get { return _starStroke.Width; }
            set
            {
                _starStroke.Width = value;
                _starDullStroke.Width = value;
                UpdateSize();
                this.Invalidate();
            }
        }

        [Description("Determines whether the user can rate with a half a star of specificity")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool AllowHalfStarRating
        {
            get { return _allowHalfStarRating; }
            set 
            {
                bool disabled = (!value && _allowHalfStarRating);
                _allowHalfStarRating = value;

                if (disabled) // Only set rating if half star was enabled and now disabled
                {
                    Rating = (int)(Rating + 0.5);
                }
            }
        }
        #endregion

        #region Polygon Definitions
        /// <summary>
        /// Gets a typical thin star polygon as a point[]
        /// </summary>
        private static PointF[] GetNormalStar(RectangleF rect)
        {
            return new []
            {
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0f), 
                new PointF(rect.X + rect.Width * 0.38f, rect.Y + rect.Height * 0.38f), 
                new PointF(rect.X + rect.Width * 0f, rect.Y + rect.Height * 0.38f), 
                new PointF(rect.X + rect.Width * 0.31f, rect.Y + rect.Height * 0.61f), 
                new PointF(rect.X + rect.Width * 0.19f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.77f), 
                new PointF(rect.X + rect.Width * 0.8f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.69f, rect.Y + rect.Height * 0.61f), 
                new PointF(rect.X + rect.Width * 1f, rect.Y + rect.Height * 0.38f), 
                new PointF(rect.X + rect.Width * 0.61f, rect.Y + rect.Height * 0.38f)
             };
        }

        /// <summary>
        /// Gets half of a typical thin star polygon as a point[]
        /// </summary>
        private static PointF[] GetNormalSemiStar(RectangleF rect)
        {
            return new []
            {
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0f), 
                new PointF(rect.X + rect.Width * 0.38f, rect.Y + rect.Height * 0.38f), 
                new PointF(rect.X + rect.Width * 0f, rect.Y + rect.Height * 0.38f), 
                new PointF(rect.X + rect.Width * 0.31f, rect.Y + rect.Height * 0.61f), 
                new PointF(rect.X + rect.Width * 0.19f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.77f), 
             };
        }

        /// <summary>
        /// Gets a fat star polygon as a point[]
        /// </summary>
        private static PointF[] GetFatStar(RectangleF rect)
        {
            return new []
            {
                new PointF(rect.X + rect.Width * 0.31f, rect.Y + rect.Height * 0.33f), 
                new PointF(rect.X + rect.Width * 0f, rect.Y + rect.Height * 0.37f), 
                new PointF(rect.X + rect.Width * 0.25f, rect.Y + rect.Height * 0.62f), 
                new PointF(rect.X + rect.Width * 0.19f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.81f), 
                new PointF(rect.X + rect.Width * 0.81f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.75f, rect.Y + rect.Height * 0.62f), 
                new PointF(rect.X + rect.Width * 1f, rect.Y + rect.Height * 0.37f), 
                new PointF(rect.X + rect.Width * 0.69f, rect.Y + rect.Height * 0.33f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0f)
            };
        }

        /// <summary>
        /// Gets half of a fat star polygon as a point[]
        /// </summary>
        private static PointF[] GetFatSemiStar(RectangleF rect)
        {
            return new []
            {
                new PointF(rect.X + rect.Width * 0.31f, rect.Y + rect.Height * 0.33f), 
                new PointF(rect.X + rect.Width * 0f, rect.Y + rect.Height * 0.37f), 
                new PointF(rect.X + rect.Width * 0.25f, rect.Y + rect.Height * 0.62f), 
                new PointF(rect.X + rect.Width * 0.19f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.81f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0f)
            };
        }

        /// <summary>
        /// Gets a detailed star polygon as a point[]
        /// </summary>
        private static PointF[] GetDetailedStar(RectangleF rect)
        {
            return new []
            {
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0f), 
                new PointF(rect.X + rect.Width * 0.6f, rect.Y + rect.Height * 0.27f), 
                new PointF(rect.X + rect.Width * 0.83f, rect.Y + rect.Height * 0.17f), 
                new PointF(rect.X + rect.Width * 0.73f, rect.Y + rect.Height * 0.4f), 
                new PointF(rect.X + rect.Width * 1f, rect.Y + rect.Height * 0.5f), 
                new PointF(rect.X + rect.Width * 0.73f, rect.Y + rect.Height * 0.6f), 
                new PointF(rect.X + rect.Width * 0.83f, rect.Y + rect.Height * 0.83f), 
                new PointF(rect.X + rect.Width * 0.6f, rect.Y + rect.Height * 0.73f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.4f, rect.Y + rect.Height * 0.73f), 
                new PointF(rect.X + rect.Width * 0.17f, rect.Y + rect.Height * 0.83f), 
                new PointF(rect.X + rect.Width * 0.27f, rect.Y + rect.Height * 0.6f), 
                new PointF(rect.X + rect.Width * 0f, rect.Y + rect.Height * 0.5f), 
                new PointF(rect.X + rect.Width * 0.27f, rect.Y + rect.Height * 0.4f), 
                new PointF(rect.X + rect.Width * 0.17f, rect.Y + rect.Height * 0.17f), 
                new PointF(rect.X + rect.Width * 0.4f, rect.Y + rect.Height * 0.27f)
            };
        }

        /// <summary>
        /// Gets half of the detailed star polygon as a point[]
        /// </summary>
        private static PointF[] GetDetailedSemiStar(RectangleF rect)
        {
            return new []
            {
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0f), 
                new PointF(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 1f), 
                new PointF(rect.X + rect.Width * 0.4f, rect.Y + rect.Height * 0.73f), 
                new PointF(rect.X + rect.Width * 0.17f, rect.Y + rect.Height * 0.83f), 
                new PointF(rect.X + rect.Width * 0.27f, rect.Y + rect.Height * 0.6f), 
                new PointF(rect.X + rect.Width * 0f, rect.Y + rect.Height * 0.5f), 
                new PointF(rect.X + rect.Width * 0.27f, rect.Y + rect.Height * 0.4f), 
                new PointF(rect.X + rect.Width * 0.17f, rect.Y + rect.Height * 0.17f), 
                new PointF(rect.X + rect.Width * 0.4f, rect.Y + rect.Height * 0.27f)
            };
        }
        #endregion
    }
}
