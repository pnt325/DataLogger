using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    [DefaultEvent("ColorPicked")]
    public class ColorPickerControl : Control
    {
        private readonly Panel _selectedColorBox = new Panel();
        private readonly Panel _hoverColorBox = new Panel();
        private Bitmap _canvas;
        private Graphics _graphicsBuffer;
        private LinearGradientBrush _spectrumGradient, _blackBottomGradient, _whiteTopGradient;
        private float _boxSizeRatio = 0.15f; // In percent
        private float _paddingPercentage = 0.05f;
        public event EventHandler ColorPicked;

        public ColorPickerControl()
        {
            base.Cursor = Cursors.Hand;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw |
                          ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);

            this.Size = new Size(200, 100);
            UpdateLinearGradientBrushes();
            UpdateGraphicsBuffer();
            SetupInnerBoxes();
        }

        private void SetupInnerBoxes()
        {
            ResizeChildControls();
        }

        protected virtual void OnColorPicked()
        {
            if (ColorPicked != null) 
                ColorPicked(this, EventArgs.Empty);
        }

        private void UpdateLinearGradientBrushes()
        {
            // Update spectrum gradient
            _spectrumGradient = new LinearGradientBrush (Point.Empty, new Point(this.Width, 0), Color.White, Color.White);
            ColorBlend blend = new ColorBlend();
            blend.Positions = new[] { 0, 1 / 7f, 2 / 7f, 3 / 7f, 4 / 7f, 5 / 7f, 6 / 7f, 1 };
            blend.Colors = new[] { Color.Gray, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
            _spectrumGradient.InterpolationColors = blend;
            // Update greyscale gradient
            RectangleF rect = new RectangleF(0, this.Height * 0.7f, this.Width, this.Height * 0.3F);
            _blackBottomGradient = new LinearGradientBrush(rect, Color.Transparent, Color.Black, 90f);
            rect = new RectangleF(Point.Empty, new SizeF(this.Width, this.Height * 0.3F));
            _whiteTopGradient = new LinearGradientBrush(rect, Color.White, Color.Transparent, 90f);
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0)
            {
                _canvas = new Bitmap(this.Width, this.Height);
                _graphicsBuffer = Graphics.FromImage(_canvas);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ResizeChildControls();
            UpdateLinearGradientBrushes();
            UpdateGraphicsBuffer();
        }

        /// <summary>
        /// Resize the child controls, since the controls are anchored bottom right,
        /// we must also relocate the controls
        /// </summary>
        private void ResizeChildControls()
        {
            // Both controls will be the same size
            int width = (int)(this.Width * _boxSizeRatio + 0.5f);
            int height = (int)(this.Height * _boxSizeRatio + 0.5f);
            _selectedColorBox.Size = new Size(width, height);
            _hoverColorBox.Size = new Size(width, height);

            int padding = (int)(this.Height * _paddingPercentage);

            // Change Location of first box
            int x = this.Width - _selectedColorBox.Width - _hoverColorBox.Width - padding * 2;
            int y = this.Height - _hoverColorBox.Height - padding;
            _selectedColorBox.Location = new Point(x, y);

            // Now second box
            x = this.Width - _selectedColorBox.Width - padding;
            y = this.Height - _selectedColorBox.Height - padding;
            _hoverColorBox.Location = new Point(x, y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //base.OnMouseMove(e);

            //if (this.ClientRectangle.Contains(e.Location))
            //{
            //    _hoverColorBox.BackColor = _canvas.GetPixel(e.X, e.Y);

            //    if (!_hoverColorBox.Visible)
            //        _hoverColorBox.Show();
            //}
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ColorPicked.Invoke(_canvas.GetPixel(e.X, e.Y), EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            _graphicsBuffer.FillRectangle(_spectrumGradient, this.ClientRectangle);
            _graphicsBuffer.FillRectangle(_blackBottomGradient, 0, this.Height * 0.7f + 1, this.Width, this.Height * 0.3f);
            _graphicsBuffer.FillRectangle(_whiteTopGradient, 0, 0, this.Width, this.Height * 0.3f);
            e.Graphics.DrawImageUnscaled(_canvas, Point.Empty);
        }

        [Description("The current selected color of the color picker control")]
        public Color SelectedColor
        {
            get { return _selectedColorBox.BackColor; }
            set
            {
                _selectedColorBox.BackColor = value;
                _selectedColorBox.Visible = true;
            }
        }

        [DefaultValue(0.15f)]
        [Description("The size of the color boxes in relation to the parent control")]
        [Category("Layout")]
        public float ColorBoxSizeRatio
        {
            get { return _boxSizeRatio; }
            set
            {
                _boxSizeRatio = value;
                ResizeChildControls();
            }
        }

        [DefaultValue(0.05f)]
        [Description("The size of the color boxes in relation to the parent control")]
        [Category("Layout")]
        public float ColorBoxPaddingRatio
        {
            get { return _paddingPercentage; }
            set
            {
                _paddingPercentage = value;
                ResizeChildControls();
            }
        }

        private bool _fullColorSpectrum = true;
        [DefaultValue(true)]
        [Description(@"Determines whether or not to use a full color spectrum for color picking.
                If set to false, a RGB spectrum will be used")]
        [Category("Appearance")]
        public bool FullColorSpectrum
        {
            get { return _fullColorSpectrum; }
            set
            {
                _fullColorSpectrum = value;
                UpdateLinearGradientBrushes();
                this.Invalidate(false);
            }
        }
    }
}
