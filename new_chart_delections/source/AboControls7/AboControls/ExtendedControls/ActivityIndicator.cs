using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace AboControls.ExtendedControls
{
    class ActivityIndicator : Control
    {
        private int _circleDiam = 15;
        private int _circleIndex;
        private int _circleCount = 6;
        private readonly SolidBrush _circleBrush = new SolidBrush(Color.Gray);
        private readonly SolidBrush _moveCircleBrush = new SolidBrush(Color.YellowGreen);
        private PointF[] _circlePoints;
        private Size _lastSize;
        private BufferedGraphics _graphicsBuffer;
        private readonly BufferedGraphicsContext _bufferContext = BufferedGraphicsManager.Current;
        private readonly Timer _tmrAnimate = new Timer();
        private UnitVector _unitVector = new UnitVector();

        public ActivityIndicator()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | 
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            this.Size = new Size(70, 70);
            SetCirclePoints();

            _tmrAnimate.Interval = 300;
            _tmrAnimate.Tick +=_tmrAnimate_Tick;
            _tmrAnimate.Start();
        }

        private void _tmrAnimate_Tick(object sender, EventArgs e)
        {
            if (_circleIndex.Equals(0))
            {
                _circleIndex = _circlePoints.Length - 1;
            }
            else
            {
                _circleIndex--;
            }

            this.Invalidate(false);
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _bufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _graphicsBuffer = _bufferContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                _graphicsBuffer.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        /// <summary>
        /// Sets the circle point array by iterating around a circle and saving points
        /// </summary>
        private void SetCirclePoints()
        {
            var pointStack = new Stack<PointF>();
            PointF centerPoint = new PointF(this.Width / 2f, this.Height / 2f);

            for (float i = 0; i < 360f; i += 360f / _circleCount)
            {
                _unitVector.SetValues(centerPoint, this.Width / 2 - _circleDiam, i);
                PointF newPoint = _unitVector.EndPoint;
                newPoint = new PointF(newPoint.X - _circleDiam / 2f, newPoint.Y - _circleDiam / 2f);
                pointStack.Push(newPoint);
            }

            _circlePoints = pointStack.ToArray();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _graphicsBuffer.Graphics.Clear(this.BackColor);

            for (int i = 0; i < _circlePoints.Length; i++) 
            {
                if (_circleIndex == i)
                {
                    _graphicsBuffer.Graphics.FillEllipse(_moveCircleBrush, _circlePoints[i].X,
                        _circlePoints[i].Y, _circleDiam, _circleDiam);
                }
                else
                {
                    _graphicsBuffer.Graphics.FillEllipse(_circleBrush, _circlePoints[i].X,
                        _circlePoints[i].Y, _circleDiam, _circleDiam);
                }
            }

            _graphicsBuffer.Render(e.Graphics);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            LockAspectRatio();
            UpdateGraphicsBuffer();
            SetCirclePoints();
            _lastSize = this.Size;
        }

        private void LockAspectRatio()
        {
            if (_lastSize.Height != this.Height)
            {
                this.Width = this.Height;
            }
            else if (_lastSize.Width != this.Width)
            {
                this.Height = this.Width;
            }
        }

        [Description("The animation speed in a milliseconds interval")]
        [Category("Behavior")]
        public int AnimateInterval
        {
            get { return _tmrAnimate.Interval; }
            set { _tmrAnimate.Interval = value; }
        }

        [Category("Layout")]
        public int CircleDiameter
        {
            get { return _circleDiam; }
            set 
            {
                _circleDiam = value;
                SetCirclePoints();
            }
        }

        [Category("Layout")]
        public int CircleCount
        {
            get { return _circleCount; }
            set 
            {
                if (value < 3) _circleCount = 3;
                else _circleCount = value;
                SetCirclePoints();

            }
        }

        [Category("Appearance")]
        public Color MovingCircleColor
        {
            get { return _moveCircleBrush.Color; }
            set { _moveCircleBrush.Color = value; }
        }

        [Category("Appearance")]
        public Color StaticCircleColor
        {
            get { return _circleBrush.Color; }
            set { _circleBrush.Color = value; }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _tmrAnimate.Enabled = this.Enabled;
        }
    }

    struct UnitVector
    {
        private double _rise, _run;
        private PointF _startPoint;

        public void SetValues(PointF startPoint, int length, double angleInDegrees)
        {
            _startPoint = startPoint;

            // Convert degrees to angle
            double radian = Math.PI * angleInDegrees / 180.0;
            if (radian > Math.PI * 2) radian = Math.PI * 2;
            if (radian < 0) radian = 0;

            // Set rise over run
            _rise = _run = length;
            _rise = Math.Sin(radian) * _rise;
            _run = Math.Cos(radian) * _run;
        }

        /// <summary>
        /// Gets the point at the end of the unit vector. It will offset from the start point
        /// by the length of the vector at the specified angle
        /// </summary>
        public PointF EndPoint
        {
            get
            {
                float xPos = (float)(_startPoint.Y + _rise);
                float yPos = (float)(_startPoint.X + _run);
                // x and y pos will be swapped because we are working with rise/run
                return new PointF(yPos, xPos);
            }
        }
    }
}
