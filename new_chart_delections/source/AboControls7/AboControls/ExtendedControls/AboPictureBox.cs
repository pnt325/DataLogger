using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    public enum DisplayMode { Normal, Fill, Center, Stretch, Zoomable }

    class AboPictureBox : Control
    {
        private DisplayMode _mode = DisplayMode.Center;
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private RectangleF _imageRect;
        private Point _lastPos;
        private Image _image;
        private Size _lastSize;
        private int _zoomSpeedMultiplier = 10;
        private float _diff;

        public AboPictureBox()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateGraphicsBuffer();
        }

        private void UpdateGraphicsBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
                _bufGraphics.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (_imageRect.Width > _imageRect.Height)
            {
                if (_lastSize.Width < this.ClientSize.Width && 
                    _mode == DisplayMode.Zoomable && _imageRect.Width < this.Width)
                {
                        SetImageWidth(this.Width);
                }
            }
            else
            {
                if (_lastSize.Height < this.ClientSize.Height &&
                    _mode == DisplayMode.Zoomable && _imageRect.Height < this.Height)
                {
                     SetImageHeight(this.Height);
                }
            }

            UpdateGraphicsBuffer();
            _lastSize = this.Size;
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _bufGraphics.Graphics.Clear(this.BackColor);

            if (this.Image != null)
            {
                switch (_mode)
                {
                    case DisplayMode.Normal: DrawImageNormal(); break;
                    case DisplayMode.Center: DrawImageCentered(); break;
                    case DisplayMode.Fill: DrawImageFilled(); break;
                    case DisplayMode.Stretch: DrawImageStretched(); break;
                    case DisplayMode.Zoomable: DrawImageWithRect(); break;
                }
            }

            _bufGraphics.Render(e.Graphics);
        }

        private void DrawImageWithRect()
        {
                if (_imageRect.Width <= 0) _imageRect.Width = _image.Width;
                if (_imageRect.Height <= 0) _imageRect.Height = _image.Height;
                _bufGraphics.Graphics.DrawImage(_image, _imageRect);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RightClickCMS.Show(this.PointToScreen(e.Location));
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button.Equals(MouseButtons.Left))
            {
                if (this.Image != null && _mode == DisplayMode.Zoomable && this.Cursor == Cursors.Default)
                    this.Cursor = Cursors.Hand;

                float x = _imageRect.Left + e.X - _lastPos.X;
                float y = _imageRect.Top + e.Y - _lastPos.Y;
                _imageRect.Location = new PointF(x, y);
                _lastPos = e.Location;
                this.Invalidate();
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                _diff = (e.Y - _lastPos.Y) * _zoomSpeedMultiplier;

                float height = _imageRect.Height + _diff;
                float ratio = height / _imageRect.Height;
                // We need to alter the width according to the change in height
                float width = _imageRect.Width * ratio;
                // Set x and y so the image sizes around its center
                float x = _imageRect.X + (_imageRect.Width - width) / 2f;
                float y = _imageRect.Y + (_imageRect.Height - height) / 2f;

                // makes for cleaner zooming (anchor top or left under certain circumstances)
                if (x > 0 || _imageRect.Width < this.Width) x = 0;
                if (y > 0 || _imageRect.Height < this.Height) y = 0;

                // If not shrinking and image is above 3x3 pixels
                if (!(_diff < 0 && (width < 3 || height < 3)))
                {
                    _imageRect = new RectangleF(x, y, width, height);
                }

                _lastPos = e.Location;
                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _lastPos = e.Location;

            // If there is an image and in zoom mode and button is left
            if (this.Image != null && _mode == DisplayMode.Zoomable)
            {
                if  (e.Button == MouseButtons.Left)
                    this.Cursor = Cursors.Hand;
                else if  (e.Button == MouseButtons.Right) 
                    this.Cursor = Cursors.NoMoveVert;
            }
        }

        private void SetImageWidth(int width)
        {
            double ratio = (double)width / _imageRect.Width;
            _imageRect.Width = width;
            _imageRect.Height = (float)(_imageRect.Height * ratio);
        }

        private void SetImageHeight(int height)
        {
            double ratio = (double)height / _imageRect.Height;
            _imageRect.Height = height;
            _imageRect.Width = (float)(_imageRect.Width * ratio);
        }

        private void AlignImageRectangle()
        {
            bool smallWidth = false;
            bool smallHeight = false;

            // Test to see if the image is smaller than the control and the width is the biggest dimension
            if (_imageRect.Width > _imageRect.Height && _imageRect.Width < this.ClientSize.Width)
            {
                smallWidth = true;
                _imageRect.Location = new PointF(0, _imageRect.Y);
                SetImageWidth(this.ClientSize.Width);
            }
            else if (_imageRect.X > 0) // We do not want to drag to far right
            {
                _imageRect.Location = new PointF(0, _imageRect.Location.Y);
            }
            else if (_imageRect.X + _imageRect.Width < this.ClientSize.Width) // We do not want to drag to far left
            {
                if (_imageRect.Width >= this.ClientSize.Width)
                    _imageRect.Location = new PointF(this.ClientSize.Width - _imageRect.Width, _imageRect.Location.Y);
                else
                    _imageRect.Location = new PointF(0, _imageRect.Location.Y);
            }

            // Test to see if the image is smaller than the control and the height is the biggest dimension
            if (_imageRect.Width < _imageRect.Height && _imageRect.Height < this.ClientSize.Height)
            {
                smallHeight = true;
                _imageRect.Location = new PointF(_imageRect.X, 0);
                SetImageHeight(this.ClientSize.Height);
            }
            else if (_imageRect.Y > 0) // We do not want to drag to far down
            {
                _imageRect.Location = new PointF(_imageRect.Location.X, 0);
            }
            else if (_imageRect.Y + _imageRect.Height < this.ClientSize.Height) // We do not want to drag to far up
            {
                _imageRect.Location = new PointF(_imageRect.X, this.ClientSize.Height - _imageRect.Height);
            }

            if (smallHeight && smallWidth)
            {
                DrawImageCentered();
                this.Invalidate();
                return;
            }

            // Keep us from dragging the image to far up when the image is smaller than the control
            if (_imageRect.Height < this.ClientSize.Height)
            {
                _imageRect.Location = new PointF(_imageRect.X, 0);
            }

            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Cursor = Cursors.Default;
            AlignImageRectangle();
        }

        private void DrawImageFilled()
        {
            float width = this.Width; // The new image width (for drawing only)
            float height = this.Height; // The new image height
            float x = 0;
            float y = 0;
            // Get image to control dimension ratio
            float ratio1 = this.Image.Width / width;
            float ratio2 = this.Image.Height / height;

            // Whatever ratio is greatest is the ratio to be applied
            if (ratio1 < ratio2)
            {
                // Apply ratio to the height of the image (we dont need to touch the width)
                height = this.Image.Height / ratio1;
                // Center the image on the y, it will be left 0 on the x
                y = this.Height / 2f - height / 2f;
            }
            else if (ratio2 < ratio1) // do the exact same thing, just for the other dimension
            {
                width = this.Image.Width / ratio2;
                x = this.Width / 2f - width / 2f;
            }

            _bufGraphics.Graphics.DrawImage(this.Image, x, y, width, height);
        }

        private void DrawImageCentered()
        {
            float width = this.ClientSize.Width; // The new image width (for drawing only)
            float height = this.ClientSize.Height; // The new image height
            float x = 0;
            float y = 0;
            // Get image to control dimension ratio
            float ratio1 = this.Image.Width / (float)this.ClientSize.Width;
            float ratio2 = this.Image.Height / (float)this.ClientSize.Height;

            // Whatever ratio is greatest is the ratio to be applied
            if (ratio1 > ratio2)
            {
                // Apply ratio to the height of the image (we dont need to touch the width)
                height = this.Image.Height / ratio1;
                // Center the image on the y, it will be left 0 on the x
                y = this.ClientSize.Height / 2f - height / 2f;
            }
            else if (ratio2 > ratio1) // do the exact same thing, just for the other dimension
            {
                width = this.Image.Width / ratio2;
                x = this.ClientSize.Width / 2f - width / 2f;
            }

            _bufGraphics.Graphics.DrawImage(this.Image, x, y, width, height);
        }

        /// <summary>
        /// Same as draw image normal except the image is resized to be completely visible
        /// </summary>
        private void DrawImageTopLeft()
        {
            float width = this.Width; // The new image width (for drawing only)
            float height = this.Height; // The new image height
            // Get image to control dimension ratio
            float ratio1 = this.Image.Width / width;
            float ratio2 = this.Image.Height / height;

            // Whatever ratio is greatest is the ratio to be applied
            if (ratio1 > ratio2)
            {
                // Apply ratio to the height of the image (we dont need to touch the width)
                height = this.Image.Height / ratio1;
                // Center the image on the y, it will be left 0 on the x
            }
            else if (ratio2 > ratio1) // do the exact same thing, just for the other dimension
            {
                width = this.Image.Width / ratio2;
            }

            _imageRect = new RectangleF(0, 0, width, height);
        }

        private void DrawImageNormal()
        {
            _bufGraphics.Graphics.DrawImage(_image, _imageRect);
        }

        private void DrawImageStretched()
        {
            _bufGraphics.Graphics.DrawImage(this.Image, this.ClientRectangle);
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                _imageRect =  new RectangleF(Point.Empty, _image.Size);

                if (this.Image != null && _mode == DisplayMode.Zoomable)
                {
                    DrawImageTopLeft();
                }

                this.Invalidate();
            }
        }

        [Category("Layout")]
        [DefaultValue(DisplayMode.Center)]
        [Description("Determines how the image will be displayed")]
        public DisplayMode SizeMode
        {
            get { return _mode; }
            set 
            {
                // if changing from zoomable then set cursor to default
                if  (_mode == DisplayMode.Zoomable && value != DisplayMode.Zoomable)
                {
                    this.Cursor = Cursors.Default;

                    if (this.Image != null)
                    DrawImageTopLeft();
                }

                _mode = value;
                this.Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(10)]
        [Description("Determines how fast to zoom in zoom mode. Its recommended to keep this value between 0 and 15")]
        public int ZoomSpeedMultiplier
        {
            get { return _zoomSpeedMultiplier; }
            set { _zoomSpeedMultiplier = value; }
        }

        [Category("Behavior")]
        [DefaultValue(null)]
        [Description("The context to show when you right-click the control (or double right-click in zoom mode)")]
        public ContextMenuStrip RightClickCMS { get; set; }
    }
}
