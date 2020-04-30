using System;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing.Text;

namespace AboControls.ExtendedControls
{
    /// <summary>
    /// Represents a location for the AM and PM indicator
    /// </summary>
    public enum MeridiemPosition { TopLeft, TopRight, BottomLeft, BottomRight, BottomMiddle };
    /// <summary>
    /// Represents a meridiem to display
    /// </summary>
    public enum Meridiem
    {
        /// <summary>
        /// Ante meridiem
        /// </summary>
        Am,
        /// <summary>
        /// Post meridiem
        /// </summary>
        Pm
    };

    /// <summary>
    /// Represents a large digital clock control
    /// </summary>
    class DigitalClock : Control
    {
        // These enums are used internally

        /// <summary>
        /// Represents the various bar locations (which are the seperate shapes of the numbers)
        /// </summary>
        private enum BarPosition { Top, TopLeft, TopRight, Middle, BottomLeft, BottomRight, Bottom };
        /// <summary>
        /// Provided for a position offset for the digits
        /// </summary>
        private enum DigitPosition { First = 0, Second = 215, Third = 505, Forth = 722 };

        #region Global Declarations
        private MeridiemPosition _meridiemPos = MeridiemPosition.BottomRight;
        private readonly SolidBrush _foreBrush = new SolidBrush(Color.DeepPink);
        private Color _lastBackColor = Color.Black;
        private readonly Pen _outlinePen = new Pen(Color.White, 0f);
        private readonly Timer _tmrUpdate = new Timer();
        private readonly SoundPlayer _soundPlayer = new SoundPlayer();
        private int _lastMinute, _alarmMinute, _alarmHour = 12;
        private float _dimAmount = 0.04f;
        private float _fadeGradientIntensity = .3f;
        private readonly double _aspectConstraint;
        private bool _optimizeForTransparency, _showMeridiem, _isArmyTime, 
            _alarmEnabled, _alarmHasPlayed, _fullOutline;

        private bool  _lockAspectRatio = true;
        private bool _dimmedBarsEnabled = true;
        private readonly Size _startSize = new Size(1006, 560);

        private BufferedGraphics _bufGraphics;

        [Description("Occurs when the minute value of the clock has changed")]
        public event EventHandler MinuteChanged;
        [Description("Occurs when the alarm has started")]
        public event EventHandler AlarmSounded;
        #endregion

        #region Methods
        public DigitalClock()
        {
            // Set to default values
            base.BackColor = Color.Black;
            this.ForeColor = Color.DeepPink;
            base.MinimumSize = new Size(20, 10);
            _aspectConstraint = _startSize.Height / (double)_startSize.Width;
            // The polys were defined and hardcoded with this start size
            // to properly scale the polys the form size must start with start size
           // this.Size = _startSize;
            this.Size = new Size(500, 500);
            // The meridiem uses this Font property
            base.Font = new Font("Tahoma", 65F, FontStyle.Bold);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

            _tmrUpdate.Tick += tmrUpdate_Tick;
            _tmrUpdate.Start();
        }

        private void UpdateBackBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                BufferedGraphicsContext bufContext = BufferedGraphicsManager.Current;
                bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);

                if (OptimizeForTransparency)
                    DecreaseGraphicsQuality();
                else
                    IncreaseGraphicsQuality();
            }
        }

        private void IncreaseGraphicsQuality()
        {
            _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _bufGraphics.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _bufGraphics.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        private void DecreaseGraphicsQuality()
        {
            _bufGraphics.Graphics.SmoothingMode = SmoothingMode.Default;
            _bufGraphics.Graphics.CompositingQuality = CompositingQuality.Default;
            _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
            _bufGraphics.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
        }

        private static int GetOnesPlace(int number)
        {
            return number - (number / 10 * 10);
        }

        private static int GetTensPlace(int number)
        {
            return number / 10;
        }

        /// <summary>
        /// Converts military time to 12-Hour time
        /// </summary>
        private static int ToNormalTime(int number)
        {
            if (DateTime.Now.Hour > 12)
                return DateTime.Now.Hour - 12;

            return (DateTime.Now.Hour == 0) ? 12 : number;
        }

        /// <summary>
        /// This is required to scale the graphics with the drawing surface
        /// </summary>
        private void UpdateScale()
        {
            float xDifference = (float)Size.Width / _startSize.Width;
            float yDifference = (float)Size.Height / _startSize.Height;
            _bufGraphics.Graphics.ScaleTransform(xDifference, yDifference);
        }
        #endregion

        #region Event Handlers
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            //Check to see if the minute has changed
            //Only render if time has changed
            if (_lastMinute == DateTime.Now.Minute) return;
            this.Invalidate();
            _lastMinute = (byte)DateTime.Now.Minute;
            this.Text = DateTime.Now.ToShortTimeString();
            OnMinuteChanged();

            if (_alarmEnabled)
            {
                //Is alarm hour the sames as the Date Time now local hour?
                bool isHourSame = (_alarmHour == ToNormalTime(DateTime.Now.Hour));
                bool isMinuteSame = (_alarmMinute == DateTime.Now.Minute);
                string currentMeridiem = DateTime.Now.ToString("tt");
                string setMeridiem = AlarmMeridiem.ToString();
                bool isMeridiemSame = (currentMeridiem == setMeridiem);

                if (isHourSame && isMinuteSame && isMeridiemSame)
                {
                    if (!_alarmHasPlayed && _soundPlayer.SoundLocation.ToLowerInvariant() != "none")
                    {
                            _soundPlayer.Play();
                            OnAlarmSounded();
                            // If alarm time, play alarm then indicate that it has played
                            _alarmHasPlayed = true;
                    }
                }
                else
                {
                    // If not alarm time indicate the alarm has not played so it can play again
                    _alarmHasPlayed = false;
                }
            }
        }

        private void RenderNumbers()
        {
            int hours = DateTime.Now.Hour;
            if (!_isArmyTime) hours = ToNormalTime(hours);

            if (!_fullOutline && _outlinePen.Width > 0)
            {
                RenderOutline(GetOnesPlace(DateTime.Now.Minute), DigitPosition.Forth);
                RenderOutline(GetTensPlace(DateTime.Now.Minute), DigitPosition.Third);
                RenderOutline(GetOnesPlace(hours), DigitPosition.Second);
            }

            //Render these digits no matter what
            RenderNumber(GetOnesPlace(DateTime.Now.Minute), DigitPosition.Forth);
            RenderNumber(GetTensPlace(DateTime.Now.Minute), DigitPosition.Third);
            RenderNumber(GetOnesPlace(hours), DigitPosition.Second);

            if (_fullOutline && _outlinePen.Width > 0)
            {
                RenderOutline(GetOnesPlace(DateTime.Now.Minute), DigitPosition.Forth);
                RenderOutline(GetTensPlace(DateTime.Now.Minute), DigitPosition.Third);
                RenderOutline(GetOnesPlace(hours), DigitPosition.Second);
            }

            if (GetTensPlace(hours) != 0) //but this one should not render when 0
            {
                if (_outlinePen.Width > 0 && !_fullOutline)
                    RenderOutline(GetTensPlace(hours), DigitPosition.First);

                RenderNumber(GetTensPlace(hours), DigitPosition.First);

                if (_outlinePen.Width > 0 && _fullOutline)
                    RenderOutline(GetTensPlace(hours), DigitPosition.First);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _bufGraphics.Graphics.Clear(this.BackColor);
            RenderColon();

            if (_dimmedBarsEnabled)
                RenderDimDigits(_dimAmount);

            RenderNumbers();

            if (_showMeridiem)
                RenderMeridiem(_meridiemPos, this.Font);

            if (_fadeGradientIntensity > 0)
                RenderFadeGradient();

            _bufGraphics.Render(e.Graphics);
        }

        private void RenderFadeGradient()
        {
            float gradientHeight = _startSize.Height * _fadeGradientIntensity;

            // Draw top gradient
            RectangleF rect = new RectangleF(0, 0, _startSize.Width, gradientHeight);
            var gradient = new LinearGradientBrush(rect, Color.Black, Color.Transparent, 90);
            gradient.WrapMode = WrapMode.TileFlipXY;
            // Double to make more intense
            _bufGraphics.Graphics.FillRectangle(gradient, rect);
            _bufGraphics.Graphics.FillRectangle(gradient, rect);
            // Draw bottom gradient
            float y = _startSize.Height * (1 - _fadeGradientIntensity);
            rect = new RectangleF(0, y, _startSize.Width, gradientHeight);
            gradient = new LinearGradientBrush(rect, Color.Transparent, Color.Black, 90);
             gradient.WrapMode = WrapMode.TileFlipXY;
            _bufGraphics.Graphics.FillRectangle(gradient, rect);
            _bufGraphics.Graphics.FillRectangle(gradient, rect);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            // Lock aspect ratio
            if (_lockAspectRatio)
                this.Height = (int)(this.Width * _aspectConstraint + 0.5);

            UpdateBackBuffer();
            UpdateScale();
            base.OnSizeChanged(e);
        }


        protected virtual void OnMinuteChanged()
        {
            if (MinuteChanged != null)
                MinuteChanged(this, EventArgs.Empty);
        }

        protected virtual void OnAlarmSounded()
        {
            if (AlarmSounded != null)
                AlarmSounded(this, EventArgs.Empty);
        }
        #endregion

        #region Drawing
        private static Color AdjustBrightness(Color inputColor, float factor)
        {
            float r = ((inputColor.R * factor) > 255) ? 255 : (inputColor.R * factor);
            float g = ((inputColor.G * factor) > 255) ? 255 : (inputColor.G * factor);
            float b = ((inputColor.B * factor) > 255) ? 255 : (inputColor.B * factor);
            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;
            return Color.FromArgb(inputColor.A, (int)(r + .5F), (int)(g + .5F), (int)(b + .5F));
        }

        private static Point[] GetShape(BarPosition barPosition, DigitPosition digitPos)
        {
            //Points retrieved when the form was 1006, 560.
            switch (barPosition)
            {
                case BarPosition.Top:
                    Point[] shape1 = { new Point(44 + (int)digitPos, 111), new Point(213 + (int)digitPos, 111),
                                       new Point(168 + (int)digitPos, 155), new Point(86 + (int)digitPos, 155) };
                    return shape1;

                case BarPosition.TopLeft:
                    Point[] shape2 = { new Point(36 + (int)digitPos, 233), new Point(36 + (int)digitPos, 120), 
                                     new Point(79 + (int)digitPos, 164), new Point(79 + (int)digitPos, 223),
                                     new Point(52 + (int)digitPos, 249) };
                    return shape2;

                case BarPosition.TopRight:
                    Point[] shape3 = { new Point(221 + (int)digitPos, 119), new Point(178 + (int)digitPos, 163), 
                                     new Point(178 + (int)digitPos, 220), new Point(205 + (int)digitPos, 248),
                                     new Point(221 + (int)digitPos, 232)};
                    return shape3;

                case BarPosition.Middle:
                    Point[] shape4 = { new Point(83 + (int)digitPos, 236), new Point(175 + (int)digitPos, 236),
                                     new Point(196 + (int)digitPos, 258), new Point(175 + (int)digitPos, 279),
                                     new Point(83 + (int)digitPos, 279), new Point(60 + (int)digitPos, 258)};
                    return shape4;

                case BarPosition.BottomLeft:
                    Point[] shape5 = { new Point(53 + (int)digitPos, 266), new Point(36 + (int)digitPos, 285),
                                     new Point(36 + (int)digitPos, 396), new Point(80 + (int)digitPos, 350),
                                     new Point(80 + (int)digitPos, 292) };
                    return shape5;

                case BarPosition.BottomRight:
                    Point[] shape6 = { new Point(205 + (int)digitPos, 266), new Point(221 + (int)digitPos, 281), 
                                     new Point(221 + (int)digitPos, 396), new Point(177 + 
                                       (int)digitPos, 350), new Point(177 + (int)digitPos, 294) };
                    return shape6;

                case BarPosition.Bottom:
                    Point[] shape7 = { new Point(88 + (int)digitPos, 359), new Point(169 + (int)digitPos, 359),
                                     new Point(212 + (int)digitPos, 404), new Point(45 + (int)digitPos, 404) };
                    return shape7;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Renders a colon (:) between the numbers
        /// </summary>
        private void RenderColon()
        {
            if (_outlinePen.Width > 0 && !_fullOutline)
            {
                _bufGraphics.Graphics.DrawRectangle(_outlinePen, 467, 319, 45, 45);
                _bufGraphics.Graphics.DrawRectangle(_outlinePen, 467, 160, 45, 45);
            }

            _bufGraphics.Graphics.FillRectangle(_foreBrush, 467, 319, 45, 45);
            _bufGraphics.Graphics.FillRectangle(_foreBrush, 467, 160, 45, 45);

            if (_outlinePen.Width > 0 && _fullOutline)
            {
                _bufGraphics.Graphics.DrawRectangle(_outlinePen, 467, 319, 45, 45);
                _bufGraphics.Graphics.DrawRectangle(_outlinePen, 467, 160, 45, 45);
            }
        }

        /// <summary>
        /// Renders a specified number at the specified position
        /// </summary>
        private void RenderNumber(int number, DigitPosition digitPos)
        {
            switch (number)
            {
                case 0:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 1:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    break;

                case 2:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 3:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 4:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    break;

                case 5:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 6:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 7:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    break;

                case 8:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 9:
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.FillPolygon(_foreBrush, GetShape(BarPosition.Bottom, digitPos));
                    break;
            }
        }

        private void RenderOutline(int number, DigitPosition digitPos)
        {
            switch (number)
            {
                case 0:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 1:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    break;

                case 2:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 3:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 4:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    break;

                case 5:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 6:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 7:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    break;

                case 8:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Bottom, digitPos));
                    break;

                case 9:
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Top, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopLeft, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.TopRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Middle, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.BottomRight, digitPos));
                    _bufGraphics.Graphics.DrawPolygon(_outlinePen, GetShape(BarPosition.Bottom, digitPos));
                    break;
            }
        }

        /// <summary>
        /// Renders all bars with a dimmed effect
        /// </summary>
        private void RenderDimDigits(float dimAmount)
        {
            Brush dimmedBrush = new SolidBrush(AdjustBrightness(this.ForeColor, dimAmount));

            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Top, DigitPosition.Second));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopLeft, DigitPosition.Second));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopRight, DigitPosition.Second));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Middle, DigitPosition.Second));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomLeft, DigitPosition.Second));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomRight, DigitPosition.Second));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Bottom, DigitPosition.Second));

            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Top, DigitPosition.Third));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopLeft, DigitPosition.Third));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopRight, DigitPosition.Third));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Middle, DigitPosition.Third));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomLeft, DigitPosition.Third));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomRight, DigitPosition.Third));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Bottom, DigitPosition.Third));

            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Top, DigitPosition.First));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopLeft, DigitPosition.First));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopRight, DigitPosition.First));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Middle, DigitPosition.First));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomLeft, DigitPosition.First));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomRight, DigitPosition.First));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Bottom, DigitPosition.First));

            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Top, DigitPosition.Forth));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopLeft, DigitPosition.Forth));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.TopRight, DigitPosition.Forth));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Middle, DigitPosition.Forth));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomLeft, DigitPosition.Forth));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.BottomRight, DigitPosition.Forth));
            _bufGraphics.Graphics.FillPolygon(dimmedBrush, GetShape(BarPosition.Bottom, DigitPosition.Forth));

            dimmedBrush.Dispose();
        }

        private void RenderMeridiem(MeridiemPosition enumMeridiemPos, Font font)
        {
            //The graphics will still be placed with the original size
            //Because they are properly scaled
            Size original = new Size(1006, 560);
            SizeF stringSize = _bufGraphics.Graphics.MeasureString("AM", font);
            int x, y;
            Point pos = new Point();

            switch (enumMeridiemPos)
            {
                case MeridiemPosition.TopLeft:
                    pos = new Point(5, 5);
                    break;

                case MeridiemPosition.TopRight:

                    x = original.Width - (int)(original.Width * .01F) - (int)stringSize.Width;
                    pos = new Point(x, (int)(original.Height * .006F));
                    break;

                case MeridiemPosition.BottomLeft:
                    y = original.Height - (int)(original.Height * .02F) - (int)stringSize.Height;
                    pos = new Point((int)(original.Width * .01F), y);
                    break;

                case MeridiemPosition.BottomRight:
                    x = original.Width - (int)(original.Width * .01F) - (int)stringSize.Width;
                    y = original.Height - (int)(original.Height * .02F) - (int)stringSize.Height;
                    pos = new Point(x, y);
                    break;

                case MeridiemPosition.BottomMiddle:
                    x = original.Width / 2 - (int)stringSize.Width / 2;
                    y = original.Height - (int)(original.Height * .02F) - (int)stringSize.Height;
                    pos = new Point(x, y);
                    break;
            }

            _bufGraphics.Graphics.DrawString(DateTime.Now.ToString("tt "), font, _foreBrush, pos);
        }
        #endregion

        #region Properties
        [Category("Appearance")]
        [Description("Enables all of the bars to be dimmed behind the illuminated")]
        [DisplayName("Dimmed Bars Enabled")]
        [DefaultValue(true)]
        public bool DimmedBarsEnabled
        {
            get { return _dimmedBarsEnabled; }
            set
            {
                _dimmedBarsEnabled = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the bool that indicates army time or 12 hour time
        /// </summary>     
        [Category("Appearance")]
        [Description("When enabled, the timer will display time in army time")]
        [DisplayName("Army Time")]
        [DefaultValue(false)]
        public bool UseArmyTime
        {
            get { return _isArmyTime; }
            set
            {
                _isArmyTime = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// If set to true, this will configure the graphics object so it can properly draw
        ///  in a transparent enviroment. Also additional tweaks like making the background the 
        ///  commonly used transparency key along with disabling dim bars because it dosen't look right.
        ///  The program control will attempt to restore the previous color when optimizing for solid color
        /// </summary>
        [Category("Appearance")]
        [Description("If set to true, this will configure the graphics object so it can properly draw" +
            " in a transparent enviroment. Also additional tweaks like making the background the " +
            "commonly used transparency key along with disabling dim bars because it dosen't look right.")]
        [DisplayName("Optimize For Transparency")]
        [DefaultValue(false)]
        public bool OptimizeForTransparency
        {
            get { return _optimizeForTransparency; }
            set
            {
                _optimizeForTransparency = value;

                if (_optimizeForTransparency)
                {
                    this.BackColor = Color.Magenta;
                    this.DimmedBarsEnabled = false;
                }
                else
                {
                    this.BackColor = _lastBackColor;
                    this.DimmedBarsEnabled = true;
                }

                OnSizeChanged(EventArgs.Empty);
            }
        }

        [DefaultValue(typeof(Font), "Tahoma, 65pt, style=Bold")]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        /// <summary>
        /// Gets or sets the bool that shows and hides the meridiems
        /// </summary>
        [Category("Appearance")]
        [Description("This value indicated whether or not the meridiems will be shown")]
        [DisplayName("Show Meridiem")]
        [DefaultValue(false)]
        public bool ShowMeridiem
        {
            get { return _showMeridiem; }
            set
            {
                _showMeridiem = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The amount to dim the bars when not illuminated")]
        [DisplayName("Dim Amount")]
        [DefaultValue(0.04F)]
        public float DimAmount
        {
            get { return _dimAmount; }
            set
            {
                if (value < 0) _dimAmount = 0;
                else if (value > 1) _dimAmount = 1;
                else _dimAmount = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The preset position of the meridiem")]
        [DisplayName("Meridiem Position")]
        [DefaultValue(MeridiemPosition.BottomRight)]
        public MeridiemPosition MeridiemPos
        {
            get { return _meridiemPos; }
            set
            {
                _meridiemPos = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "DeepPink")]
        public override Color ForeColor
        {
            get { return _foreBrush.Color; }
            set
            {
                base.ForeColor = value;
                _foreBrush.Color = value;
                this.Invalidate();
            }
        }

        [Description("AM or PM"), DefaultValue(Meridiem.Am), Category("Clock Alarm")]
        public Meridiem AlarmMeridiem { get; set; }

        [Description("The 12 hour time value in which the alarm will sound")]
        [DefaultValue(12)]
        [Category("Clock Alarm")]
        public int AlarmHour
        {
            get { return _alarmHour; }
            set { _alarmHour = (value > 12) ? 12 : value; }
        }

        [Description("The minute of the specified hour in which the alarm will sound")]
        [DefaultValue(0)]
        [Category("Clock Alarm")]
        public int AlarmMinute
        {
            get { return _alarmMinute; }
            set { _alarmMinute = (value > 59) ? 59 : value; }
        }

        [DefaultValue(false)]
        [Category("Clock Alarm")]
        public bool AlarmEnabled
        {
            get { return _alarmEnabled; }
            set { _alarmEnabled = value; }
        }

        [DefaultValue(typeof(Color), "Black")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (!this.BackColor.Equals(Color.Magenta))
                {
                    _lastBackColor = this.BackColor;
                }

                base.BackColor = value;
                this.Invalidate();
            }
        }

        [Category("Clock Alarm")]
        [DefaultValue("None")]
        [Description("The location of the wav to play when the alarm goes off")]
        public string AlarmSoundLocation
        {
            get { return _soundPlayer.SoundLocation; }
            set { _soundPlayer.SoundLocation = string.IsNullOrEmpty(value) ? "None" : value; }
        }

        /// <summary>
        /// Do not call this directly after setting colors as brushes for colors may not
        /// set because control is being resized. When the control is being resized the
        /// forecolor brush cannot be set.
        /// </summary>
        [Description("When true, locks the digital clocks original ratio")]
        [Category("Layout")]
        [DefaultValue(true)]
        public bool LockAspectRatio
        {
            get { return _lockAspectRatio; }
            set
            {
                _lockAspectRatio = value;
                // Update when we are locking
                OnSizeChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color OutlineColor
        {
            get { return _outlinePen.Color; }
            set
            {
                _outlinePen.Color = value;
                this.Invalidate();
            }
        }

        [Description("The intensity of the vertical fade to black gradient (0 - 0.6f). Set to 0 to disable")]
        [Category("Appearance")]
        [DefaultValue(0.3f)]
        public float FadeGradientIntensity
        {
            get { return _fadeGradientIntensity; }
            set
            {
                if (value < 0)
                    _fadeGradientIntensity = 0;
                else if (value > 0.6)
                    _fadeGradientIntensity = 0.6f;
                else
                    _fadeGradientIntensity = value;

                this.Invalidate();
            }
        }

        [Description("The thickness of the outline (set to 0 for no outline)")]
        [Category("Appearance")]
        [DefaultValue(0)]
        public float OutlineThickness
        {
            get { return _outlinePen.Width; }
            set
            {
                _outlinePen.Width = value;
                this.Invalidate();
            }
        }

        [Description("When true, the full stroke of the outline is drawn centered around the polys")]
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool FullOutline
        {
            get { return _fullOutline; }
            set
            {
                _fullOutline = value;
                this.Invalidate();
            }
        }
        #endregion
    }
}
