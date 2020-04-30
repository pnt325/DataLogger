using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    public class StatusIndicator : Control
    {
        private int _numOfBars = 6, _spacing = 3, _status = 100;
        private Graphics _graphicsBuffer;
        private Bitmap _drawingSurface;
        private Color _lowStatusColor = Color.Red;
        private Color _mediumLowStatusColor = Color.Yellow;
        private Color _mediumHighStatusColor = Color.YellowGreen;
        private Color _highStatusColor = Color.Green;
        private Color _currentStatusColor = Color.Green;
        private LinearGradientBrush _lgb;

        public StatusIndicator()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | 
              ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        private void UpdateGraphics()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _drawingSurface = new Bitmap(this.Width, this.Height);
                _graphicsBuffer = Graphics.FromImage(_drawingSurface);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _graphicsBuffer.Clear(this.BackColor);
            //Casting to double to retain absolute precision
            float widthSpace = (_spacing * (float)_numOfBars) + _spacing;
            float barWidth = (this.Width - widthSpace) / _numOfBars;
            float baseHeight = this.Height - _spacing * 2;
            //Only display a percentage of the status bars according to the status value
            float numOfBarsToDisplay = _numOfBars * (_status / 100.0f) + 0.5f;

            _lgb = new LinearGradientBrush(Point.Empty, new Point(0, this.Height),
                _currentStatusColor, AdjustBrightness(_currentStatusColor, .5F));

            for (int i = 0; i < numOfBarsToDisplay; i++)
            {
                float barHeight = baseHeight / _numOfBars * (i + 1);
                float barPosX = _spacing + ((_spacing + barWidth) * i);
                float barPosY = baseHeight - barHeight + _spacing;

                _graphicsBuffer.FillRectangle(_lgb, barPosX, barPosY, barWidth, barHeight);
            }

            e.Graphics.DrawImageUnscaled(_drawingSurface, Point.Empty);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateGraphics();
            this.Invalidate();
        }

        private static Color AdjustBrightness(Color inputColor, float factor)
        {
            float R = ((inputColor.R * factor) > 255) ? 255 : (inputColor.R * factor);
            float G = ((inputColor.G * factor) > 255) ? 255 : (inputColor.G * factor);
            float B = ((inputColor.B * factor) > 255) ? 255 : (inputColor.B * factor);
            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;
            return Color.FromArgb(inputColor.A, (int)(R + .5F), (int)(G + .5F), (int)(B + .5F));
        }

        private void SetStatusColor()
        {
            if (_status <= 25)
                _currentStatusColor = _lowStatusColor;
            else if (_status > 25 && _status <= 50)
                _currentStatusColor = _mediumLowStatusColor;
            else if (_status > 50 && _status <= 75)
                _currentStatusColor = _mediumHighStatusColor;
            else
                _currentStatusColor = _highStatusColor;
        }

        #region Properties 
        [Category("Status Indicator Specific")]
        [Description("The color of the bars when the status value is 0-25")]
        [DefaultValue(typeof(Color), "Red")]
        public Color LowColor
        {
            get { return _lowStatusColor; }
            set 
            {
                _lowStatusColor = value;
                SetStatusColor();
                this.Invalidate();
            }
        }

        [Category("Status Indicator Specific")]
        [Description("The color of the bars when the status value is 25-50")]
        [DefaultValue(typeof(Color), "Yellow")]
        public Color MediumLowColor
        {
            get { return _mediumLowStatusColor; }
            set
            { 
                _mediumLowStatusColor = value;
                SetStatusColor();
                this.Invalidate();
            }
        }

        [Category("Status Indicator Specific")]
        [Description("The color of the bars when the status value is 50-75")]
        [DefaultValue(typeof(Color), "YellowGreen")]
        public Color MediumHighColor
        {
            get { return _mediumHighStatusColor; }
            set 
            { 
                _mediumHighStatusColor = value;
                SetStatusColor();
                this.Invalidate();
            }
        }

        [Category("Status Indicator Specific")]
        [Description("The color of the bars when the status value is 75-100")]
        [DefaultValue(typeof(Color), "Green")]
        public Color HighColor
        {
            get { return _highStatusColor; }
            set 
            { 
                _highStatusColor = value;
                SetStatusColor();
                this.Invalidate();
            }
        }

        [Category("Status Indicator Specific")]
        [Description("The number of bars to display status")]
        [DefaultValue(6)]
        public int NumberOfBars
        {
            get { return _numOfBars; }
            set 
            {
                _numOfBars = (value >= 3) ? value : 3;
                this.Invalidate();
            }
        }

        [Category("Status Indicator Specific")]
        [Description("The space between the bars and the padding of there container")]
        [DefaultValue(3)]
        public int Spacing
        {
            get { return _spacing; }
            set 
            {
                _spacing = (value > 0) ? value : 0;
                this.Invalidate();
            }
        }

        [Category("Status Indicator Specific")]
        [Description("The current status which fully influences the status bars")]
        [DefaultValue(100)]
        public int Status
        {
            get { return _status; }
            set 
            {
                if (value < 0) _status = 0;
                else if (value > 100) _status = 100;
                else _status = value;

                SetStatusColor();
                this.Invalidate();
            }
        }
        #endregion
    }
}
