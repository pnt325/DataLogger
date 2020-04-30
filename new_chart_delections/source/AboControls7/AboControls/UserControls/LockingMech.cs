using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AboControls.Properties;
using System.Drawing.Drawing2D;

namespace AboControls.UserControls
{
    public partial class LockingMech : UserControl
    {
        private Point _lastClick;
        private const int _MARGIN = 3;
        private bool _locked = true;
        private double _lockerWidthPercent = 0.3;
        public readonly Bitmap GrayButtonBitmap = Resources.GraySlideButton;
        public readonly Bitmap RedButtonBitmap = Resources.RedSlideButton;

        [Description("Occurs when the IsLocked Property has changed")]
        public event EventHandler LockedChanged;

        public LockingMech()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns the locker to its original position and 
        /// sets the IsLocked property back to true.
        /// </summary>
        public void LockMech()
        {
            while (btnLocker.Location.X > 3)
            {
                btnLocker.Left -= 1;
            }

            this.IsLocked = true;
        }

        /// <summary>
        /// Moves the locker to its locked state position and
        /// changes the IsLocked property to false.
        /// </summary>
        public void UnlockMech()
        {
            int unlockedXPos = this.Width - btnLocker.Size.Width - _MARGIN;

            while (btnLocker.Location.X < unlockedXPos)
            {
                btnLocker.Left += 1;
            }

            _locked = false;
        }

        private void btnLocker_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _lastClick = e.Location;
            }
        }

        private void btnLocker_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the position the button must reach before it gets locked
                int maxRightMostPos = this.Width - btnLocker.Size.Width - _MARGIN;

                if (btnLocker.Location.X < maxRightMostPos)
                {
                    btnLocker.Left += e.X - _lastClick.X;
                }
                else
                {
                    // Make sure the button does not end up out of bounds
                    btnLocker.Location = new Point(maxRightMostPos, btnLocker.Location.Y);
                    // Control is now in locked state
                    IsLocked = false;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateSliderWidth();
        }

        /// <summary>
        /// When the control is resized, update the buttons size
        /// in relation to its parent container, by applying a 
        /// percentage to it.
        /// </summary>
        private void UpdateSliderWidth()
        {
            btnLocker.Width = (int)(this.Width * _lockerWidthPercent + 0.5);
        }

        private void btnLocker_MouseUp(object sender, MouseEventArgs e)
        {
            if (_locked)
            {
                while (btnLocker.Location.X > 3)
                {
                    btnLocker.Left -= 1;
                }
            }
        }

        protected virtual void OnLockedChanged()
        {
            if (LockedChanged != null)
            {
                LockedChanged(this, EventArgs.Empty);
            }
        }

        #region Properties
        /// <summary>
        /// Sets the image used for the locker button's background
        /// </summary>
        [Category("Appearance")]
        [Description("The image used for the locker button's background")]
        public Bitmap ButtonImage
        {
            set { btnLocker.BackgroundImage = value; }
        }

        /// <summary>
        /// Gets or sets how wide the button is in relation to its parent
        /// </summary>
        [Category("Layout")]
        [Description("How wide the button is in relation to its parent")]
        [DisplayName("Button width percent")]
        [DefaultValue(0.3)]
        public double ButtonWidthPercent
        {
            get { return _lockerWidthPercent; }
            set
            {
                if (value > 0.9)
                {
                    _lockerWidthPercent = 0.9;
                }
                else if (value < 0.1)
                {
                    _lockerWidthPercent = 0.1;
                }
                else
                {
                    _lockerWidthPercent = value;
                }

                UpdateSliderWidth();
            }
        }

        /// <summary>
        /// Gets or sets the lock state of the control.
        /// The button will be to the left if control is locked
        /// and it will be to the right if it is unlocked
        /// </summary>
        [Category("Behaviour")]
        [Description("The button will be to the left if control is locked "
        + "and it will be to the right if it is unlocked")]
        [DefaultValue(true)]
        public bool IsLocked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                OnLockedChanged();
            }
        }
        #endregion
    }

    [DesignTimeVisible(false)]
    public class LockButton : Control
    {
        private bool _slideRightToLock = true;
        private int _cornerRadius = 10;
        private int _arrowScale = 80;
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufferContext = BufferedGraphicsManager.Current;
        private readonly SolidBrush _arrowBrush = (SolidBrush)Brushes.Gray;

        public LockButton()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            _bufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            // Use handle here
            _bufGraphics = _bufferContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
            _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _bufGraphics.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            this.Region = new Region(this.ClientRectangle);

            using (GraphicsPath GP = RoundedRectangle.Create(this.ClientRectangle, _cornerRadius))
            {
                this.Region = new Region(GP);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            _bufGraphics.Graphics.Clear(Color.Silver);
            PointF[] poly = GetArrowPoly();
            _bufGraphics.Graphics.FillPolygon(_arrowBrush, poly);
            _bufGraphics.Render(pevent.Graphics);
        }

        private PointF[] GetArrowPoly()
        {
            Rectangle rect = this.ClientRectangle;

            double multiplier = (_arrowScale) / 100.0 * -1;
            int width = (int)(this.ClientSize.Width * multiplier + 0.5);
            int height = (int)(this.ClientSize.Height * multiplier + 0.5);
            rect.Inflate(width, height);

            PointF[] vertices;

            if (!_slideRightToLock)
            {
                vertices = new []
            {
            new PointF(rect.X, rect.Y + rect.Height * .3f),
            new PointF(rect.X + rect.Width * .6f, rect.Y + rect.Height * .3f),
            new PointF(rect.X + rect.Width * .6f, rect.Y),
            new PointF(rect.X + rect.Width, rect.Y + rect.Height *.5f),
            new PointF(rect.X + rect.Width * .6f, rect.Y + rect.Height),
            new PointF(rect.X + rect.Width *.6f, rect.Y + rect.Height * .7f),
            new PointF(rect.X, rect.Y + rect.Height * .7f)
            };
            }
            else
            {
                vertices = new []
            {
            new PointF(rect.X + rect.Width, rect.Y + rect.Height * .25f),
            new PointF(rect.X + rect.Width * .4f, rect.Y + rect.Height * .25f),
            new PointF(rect.X + rect.Width * .4f, rect.Y),
            new PointF(rect.X, rect.Y + rect.Height *.5f),
            new PointF(rect.X + rect.Width * .4f, rect.Y + rect.Height),
            new PointF(rect.X + rect.Width *.4f, rect.Y + rect.Height * .75f),
            new PointF(rect.X+ rect.Width, rect.Y + rect.Height * .75f)
            };
            }

            return vertices;
        }

        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                if (_cornerRadius < 0)
                {
                    _cornerRadius = 0;
                }
                else
                {
                    _cornerRadius = value;
                }
            }
        }

        public int SizeModifer
        {
            get { return _arrowScale; }
            set { _arrowScale = value; }
        }

        public bool SlideRightToLock
        {
            get { return _slideRightToLock; }
            set
            {
                _slideRightToLock = value;
                this.Invalidate(false);
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

            public static GraphicsPath Create(Rectangle rect, int radius, RectangleCorners c)
            { return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, c); }

            public static GraphicsPath Create(int x, int y, int width, int height, int radius)
            { return Create(x, y, width, height, radius, RectangleCorners.All); }

            public static GraphicsPath Create(Rectangle rect, int radius)
            { return Create(rect.X, rect.Y, rect.Width, rect.Height, radius); }

            public static GraphicsPath Create(int x, int y, int width, int height)
            { return Create(x, y, width, height, 5); }

            public static GraphicsPath Create(Rectangle rect)
            { return Create(rect.X, rect.Y, rect.Width, rect.Height); }
        }
    }
}
