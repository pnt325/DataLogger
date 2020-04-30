using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    /// <summary>
    /// Represents a box to display incrementable numbers
    /// </summary>
    [DefaultEvent("ValueChanged")]
    class AboNumberBox : Control
    {
        /// <summary>
        /// Represents one of the hoverable parts of the control or none of them
        /// </summary>
        private enum HoveredPart { None, LeftArrow, RightArrow, Center }
        private BufferedGraphics _bufGraphics;
        private RectangleF _numberBoxRect;
        private readonly GraphicsPath _leftArrowPath = new GraphicsPath();
        private readonly GraphicsPath _rightArrowPath = new GraphicsPath();
        private readonly Pen _rangePen = new Pen(Color.FromArgb(220, 220, 220), 2f);
        private int _lastX, _moveDist;
        private bool _scrolling;

        public AboNumberBox()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);

            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            Size = new Size(80, 20);
            base.Font = new Font(base.Font.FontFamily, 10f);
            // For faster incrementing

            _rangePen.DashStyle = DashStyle.Dash;
        }

        #region Overrides
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Select();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.Invalidate();
        }

        [Description("Occurs when the value of the Value property changes")]
        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        [Description("Occurs when the value of the Value property changes by user input")]
        public event EventHandler Scroll;
        protected virtual void OnScroll()
        {
            if (Scroll != null)
                Scroll(this, EventArgs.Empty);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CurrentHovered = HoveredPart.None;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (CurrentHovered == HoveredPart.Center)
            {
                _lastX = e.X;
                _scrolling = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cursor = Cursors.Default;

            if (_scrolling)
            {
                _scrolling = false;
                this.Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left && _scrolling)
            {
                if (Cursor != Cursors.NoMoveHoriz)
                    Cursor = Cursors.NoMoveHoriz;

                const int SENSITIVITY = 40;

                if (_moveDist > SENSITIVITY)
                {
                    ScrollDown();
                    _moveDist = 0;
                }
                else if (_moveDist < -SENSITIVITY)
                {
                    ScrollUp();
                    _moveDist = 0;
                }
                else
                {
                    _moveDist += _lastX - e.X;
                    _lastX = e.X;
                }

                this.Invalidate();
            }
            else
            {
                if (_numberBoxRect.Contains(e.Location))
                    CurrentHovered = HoveredPart.Center;
                else if (_leftArrowPath.IsVisible(e.Location))
                    CurrentHovered = HoveredPart.LeftArrow;
                else if (_rightArrowPath.IsVisible(e.Location))
                    CurrentHovered = HoveredPart.RightArrow;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                if (_rightArrowPath.IsVisible(e.Location))
                {
                    ScrollUp();
                }
                else if (_leftArrowPath.IsVisible(e.Location))
                {
                    ScrollDown();
                }
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
            if (_scrolling) DrawRange();
            _bufGraphics.Graphics.FillPath(LeftArrowBrush, _leftArrowPath);
            _bufGraphics.Graphics.FillPath(RightArrowBrush, _rightArrowPath);
            var arrowPen = this.Enabled ? Pens.Black : new Pen(Color.FromArgb(100, 100, 100));
            _bufGraphics.Graphics.DrawPath(arrowPen, _leftArrowPath);
            _bufGraphics.Graphics.DrawPath(arrowPen, _rightArrowPath);
            DrawValue();
            _bufGraphics.Render(e.Graphics);
        }
        #endregion

        /// <summary>
        /// Decrements Value with bounds checks
        /// </summary>
        private void ScrollDown()
        {
            if (_value - _increment < _minimum)
                Value = _minimum;
            else
                Value -= _increment;

            OnScroll();
        }

        /// <summary>
        /// Increments Value with bounds checks
        /// </summary>
        private void ScrollUp()
        {
            if (_value + _increment > _maximum)
                Value = _maximum;
            else
                Value += _increment;

            OnScroll();
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                BufferedGraphicsContext bufContext = BufferedGraphicsManager.Current;
                bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                UpdateDrawingBoundaries();
                _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        /// <summary>
        /// Gets a smaller copy of the controls font, if ValueString is too wide
        /// </summary>
        private Font GetResultFont()
        {
            Font tempFont = (Font)Font.Clone();
            SizeF textSize = _bufGraphics.Graphics.MeasureString(ValueString, tempFont);
            float padding = this.Width * 0.05f;

            while (tempFont.Size > 3 && textSize.Width > _numberBoxRect.Width - padding)
            {
                tempFont = new Font(tempFont.FontFamily, tempFont.Size - 1, tempFont.Style);
                textSize = _bufGraphics.Graphics.MeasureString(ValueString, tempFont);
            }

            if (Focused)
            {
                tempFont = new Font(tempFont.FontFamily, tempFont.Size, FontStyle.Underline);
            }

            return tempFont;
        }

        private void DrawValue()
        {
            Font font = GetResultFont();
            SizeF textSize = _bufGraphics.Graphics.MeasureString(ValueString, font);
            float x = _numberBoxRect.X + _numberBoxRect.Width / 2 - textSize.Width / 2;
            float y = _numberBoxRect.Y + _numberBoxRect.Height / 2 - textSize.Height / 2;
            Brush brush = (this.Enabled) ? new SolidBrush(this.ForeColor) : Brushes.DimGray;
            _bufGraphics.Graphics.DrawString(ValueString, font, brush, x, y);
        }

        /// <summary>
        /// Draws the lines that indicate a percentage from the max value to the min value
        /// </summary>
        private void DrawRange()
        {
            if (_maximum == _minimum) return;
            decimal percent = (_value - _minimum) / (_maximum - _minimum);
            if (percent == 0) return;
            float width = _numberBoxRect.Width * (float)percent;

            PointF p1 = new PointF(_numberBoxRect.X, _numberBoxRect.Y);
            PointF p2 = new PointF(_numberBoxRect.X + width, _numberBoxRect.Y);
            _bufGraphics.Graphics.DrawLine(_rangePen, p1, p2);

            float y = _numberBoxRect.Y + _numberBoxRect.Height - _rangePen.Width / 2f;
            p1 = new PointF(_numberBoxRect.X, y);
            p2 = new PointF(_numberBoxRect.X + width, y);
            _bufGraphics.Graphics.DrawLine(_rangePen, p1, p2);
        }

        /// <summary>
        /// Updates the number rect and the arrow paths
        /// </summary>
        private void UpdateDrawingBoundaries()
        {
            _numberBoxRect = this.ClientRectangle;
            _numberBoxRect.Inflate(-this.Height, 0);

            // Update left arrow
            var poly = new[]
                {
                    new PointF(0, this.Height / 2f),
                    new PointF(_numberBoxRect.X, 0),
                    new PointF(_numberBoxRect.X, this.Height - 1)
                };

            _leftArrowPath.Reset();
            _leftArrowPath.AddPolygon(poly);

            float x = _numberBoxRect.X + _numberBoxRect.Width;
            // Update right arrow
            poly = new[]
                {
                    new PointF(x , 0),
                    new PointF(this.Width, this.Height / 2f),
                    new PointF(x, this.Height - 1)
                };

            _rightArrowPath.Reset();
            _rightArrowPath.AddPolygon(poly);
        }

        #region Properties
        private HoveredPart _currentHovered;
        private HoveredPart CurrentHovered
        {
            get { return _currentHovered; }
            set
            {
                bool changed = _currentHovered != value;

                if (changed)
                {
                    _currentHovered = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the SolidBrush for the left arrows fill
        /// </summary>
        private SolidBrush LeftArrowBrush
        {
            get
            {
                if (this.Enabled == false)
                    return new SolidBrush(Color.FromArgb(150, 150, 150));

                if (_currentHovered == HoveredPart.LeftArrow)
                    return new SolidBrush(Color.FromArgb(190, 190, 190));

                return new SolidBrush(Color.FromArgb(120, 120, 120));
            }
        }

        /// <summary>
        /// Gets the SolidBrush for the right arrows fill
        /// </summary>
        private SolidBrush RightArrowBrush
        {
            get
            {
                if (this.Enabled == false)
                    return new SolidBrush(Color.FromArgb(150, 150, 150));

                if (_currentHovered == HoveredPart.RightArrow)
                    return new SolidBrush(Color.FromArgb(190, 190, 190));

                return new SolidBrush(Color.FromArgb(120, 120, 120));
            }
        }

        private bool _showPercent = true;
        [Category("Appearance")]
        [Description(@"Whether to show the ""%"" symbol after the number")]
        [DefaultValue(true)]
        public bool ShowPercent
        {
            get { return _showPercent; }
            set
            {
                _showPercent = value;
                this.Invalidate();
            }
        }

        private decimal _value = 1;
        [Category("Appearance")]
        [DefaultValue(1)]
        public decimal Value
        {
            get { return _value; }
            set
            {
                if (value > _maximum || value < _minimum)
                {
                    const string FORMAT = @"Value of {0} is not valid for 'Value'. 'Value' should be between 'Minimum' and 'Maximum'";
                    string msg = String.Format(FORMAT, value);
                    throw new ArgumentOutOfRangeException("value", msg);
                }

                bool changed = value != _value;

                if (changed)
                {
                    if (value > _maximum)
                        _value = _maximum;
                    else if (value < _minimum)
                        _value = _minimum;
                    else
                        _value = value;

                    OnValueChanged();
                    this.Invalidate();
                }
            }
        }

        private decimal _increment = 1;
        [Category("Data")]
        [Description("The amount to increment or decrement when the arrows buttons are clicked")]
        [DefaultValue(1)]
        public decimal Increment
        {
            get { return _increment; }
            set
            {
                _increment = value;
                this.Invalidate();
            }
        }

        private decimal _minimum;
        [Category("Data")]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public decimal Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;

                if (_value < _minimum)
                    _value = _minimum;

                if (_minimum > _maximum)
                    _maximum = _minimum;

                this.Invalidate();
            }
        }

        private decimal _maximum = 100;
        [Category("Data")]
        [DefaultValue(100)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public decimal Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;

                if (_value > _maximum)
                    _value = _maximum;

                if (_maximum < _minimum)
                    _minimum = _maximum;

                this.Invalidate();
            }
        }

        private int _decimalPlaces;
        [Category("Data")]
        [DefaultValue(0)]
        public int DecimalPlaces
        {
            get { return _decimalPlaces; }
            set
            {
                _decimalPlaces = value;

                if (_decimalPlaces > 0)
                Value = decimal.Round(_value, _decimalPlaces);
            }
        }

        private int _scrollSensitivity = 40;
        [Description("Determines how many pixels to mouse drag before incrementing/decrementing value")]
        [Category("Behavior")]
        [DefaultValue(40)]
        public int ScrollSensitivity
        {
            get { return _scrollSensitivity; }
            set { _scrollSensitivity = value; }
        }


        /// <summary>
        /// Gets the value as a string
        /// </summary>
        [Browsable(false)]
        public string ValueString
        {
            get
            {
                string str = _value.ToString(CultureInfo.InvariantCulture);
                if (_showPercent) str += '%';
                return str;
            }
        }
        #endregion
    }
}
