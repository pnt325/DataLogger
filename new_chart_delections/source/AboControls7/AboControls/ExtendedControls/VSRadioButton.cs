using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    // I use the term "Button" to refer to the drawn image that indicates the controls checked state

    /// <summary>
    /// Represents a checkbox like control to be used in a set, to provide multiple options
    /// </summary>
    [DefaultEvent("ChangedChanged")]
    class VSRadioButton : BackbufferedControl
    {
        #region Fields
        private bool _containsCursor;
        private const int _TEXT_VERT_PAD = 5; // Value is used when determining the best auto-size
        private const int _TEXT_HORIZ_PAD = 4;
        private const int _BUTTON_LEFT_PAD = 2;
        private const int _BUTTON_DIM = 15; // The width and height of the small glyph on the left
        /// <summary>
        /// The Brush used for the outer portion of the button
        /// </summary>
        private readonly SolidBrush _outerRingBrush = new SolidBrush(Color.FromArgb(200, 200, 200));
        /// <summary>
        /// The Brush used for the outer portion of the button
        /// </summary>
        private readonly SolidBrush _outerRingHoverBrush = new SolidBrush(Color.FromArgb(138, 172, 184));
        /// <summary>
        /// The brush used for the inner portion of the button when the button is checked
        /// </summary>
        private readonly SolidBrush _checkedBrush = new SolidBrush(Color.FromArgb(0, 122, 204));
        /// <summary>
        /// The brush used for the inner portion of the button when the button is checked
        /// </summary>
        private readonly SolidBrush _uncheckedBrush = new SolidBrush(Color.Silver);
        #endregion

        [Description("Occurs when the value of the Checked property has changed")]
        public event EventHandler ChangedChanged;

        public VSRadioButton()
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UncheckOthers = true;
            base.Font = new Font("Segoe UI", 9f);
            base.BackColor = Color.Transparent;
            base.ForeColor = Color.White;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            AutoAdjustSize();
        }

        #region Overrides
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _containsCursor = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _containsCursor = false;
            this.Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Checked = true;
        }

        protected override void OnBackBufferUpdated()
        {
            BackBuffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            BackBuffer.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }

        protected override void Render()
        {
            // Draw outer ring 
            float y = this.Height / 2f - _BUTTON_DIM / 2f;
            var rect = new RectangleF(_BUTTON_LEFT_PAD, y, _BUTTON_DIM, _BUTTON_DIM);
            var ringBrush = _containsCursor ? _outerRingHoverBrush : _outerRingBrush;
            BackBuffer.Graphics.FillEllipse(ringBrush, rect);
            // Draw center
            rect.Inflate(-3, -3);
            BackBuffer.Graphics.FillEllipse(ButtonForeBrush, rect);
            // Draw text
            const float BTN_MARGIN_WIDTH = _BUTTON_LEFT_PAD + _TEXT_HORIZ_PAD + _BUTTON_DIM;
            float x = BTN_MARGIN_WIDTH;
            y = this.Height/2f - TextSize.Height/2f;
            var foreBrush = new SolidBrush(this.ForeColor);
           BackBuffer.Graphics.DrawString(this.Text, this.Font, foreBrush, x, y);
        }

        /// <summary>
        /// Occurs when the value of the Checked property has changed
        /// </summary>
        protected virtual void OnCheckedChanged()
        {
            if (ChangedChanged != null)
            {
                ChangedChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            AutoAdjustSize();
        }
        #endregion

        /// <summary>
        /// Automatically adjusts the control size to encompass the text and button
        /// </summary>
        private void AutoAdjustSize()
        {
            if (BackBuffer != null && _customAutoSize)
            {
                int width = _BUTTON_DIM + (int)TextSize.Width + _TEXT_HORIZ_PAD;
                int height = (int)(TextSize.Height + 0.5);

                if (height < _BUTTON_DIM)
                {
                    height = _BUTTON_DIM + _TEXT_VERT_PAD*2;
                }

                base.Size = new Size(width, height);
            }
        }

        /// <summary>
        /// Uncheck all other VSRadioButtons in this controls parent
        /// </summary>
        private void UncheckOthersInParent()
        {
            foreach (Control control in this.Parent.Controls)
            {
                var radio = control as VSRadioButton;

                if (radio != null && radio != this)
                {
                    radio.Checked = false;
                }
            }
        }

        #region Properties
        /// <summary>
        /// Gets the fore-brush of the button which is determined by the current checkstate
        /// </summary>
        private SolidBrush ButtonForeBrush
        {
            get { return _checked ? _checkedBrush : _uncheckedBrush; }
        }

        /// <summary>
        /// Gets the size of the controls text using the controls font
        /// </summary>
        private SizeF TextSize
        {
            get
            {
                return BackBuffer.Graphics.MeasureString(this.Text, this.Font);
               // return TextRenderer.MeasureText(this.Text, this.Font);
            }
        }

        [DefaultValue(typeof(Color), "transparent")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }


        private bool _customAutoSize = true;
        [Category("Layout")]
        [Description("If true, the control will automatically size itself to its containing")]
        [DefaultValue(true)]
        public bool CustomAutoSize
        {
            get { return _customAutoSize; }
            set
            {
                bool changed = _customAutoSize != value;

                if (changed)
                {
                    _customAutoSize = value;
                    AutoAdjustSize();
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [DefaultValue(typeof(Font), "Segoe UI, 9pt")]
        public override Font Font 
        { 
            get { return base.Font; }
            set
            {
                base.Font = value;
                AutoAdjustSize();
            }
        }

        private bool _checked;
        [Category("Input")]
        [Description("Determines whether or not this control is checked")]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                bool changed = value != _checked;

                if (changed)
                {
                    _checked = value;
                    OnCheckedChanged();

                    if (UncheckOthers && this.Parent != null && _checked)
                    {
                        UncheckOthersInParent();
                    }

                    this.Invalidate();
                }
            }
        }

        [Category("Behavior")]
        [Description("If true, unchecks all other VSRadioButtons in this controls parent")]
        [DefaultValue(true)]
        public bool UncheckOthers { get; set; }
        #endregion
    }

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
            Color backColor = this.BackColor;

            if (GetStyle(ControlStyles.SupportsTransparentBackColor) &&
                this.BackColor == Color.Transparent)
            {
                backColor = this.Parent.BackColor;
            }

            BackBuffer.Graphics.Clear(backColor);
            Render();
            BackBuffer.Render(e.Graphics);
        }

        /// <summary>
        /// Provides implementation after the BackBuffer has been update
        /// (which occurs on size and in the constructor)
        /// </summary>
        protected virtual void OnBackBufferUpdated() {}

        /// <summary>
        /// Implement drawing logic here
        /// </summary>
        protected abstract void Render();
    }
}
