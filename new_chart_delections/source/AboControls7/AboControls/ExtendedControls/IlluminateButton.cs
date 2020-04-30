using System;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    /// <summary>
    /// Represents a button configured for displaying an image. The image will illuminate
    /// when the mouse hovers over this control
    /// </summary>
    class IlluminateButton : Button
    {
        // To keep track of unmodified images
        private Image _leaveImage;

        public IlluminateButton()
        {
            base.Cursor = Cursors.Hand;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //Disable addtional graphics
            FlatAppearance.BorderSize = 0;
            base.BackColor = Color.Transparent;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            HoverBrightness = 1;
            HoverContrast = 1.2f;
            HoverGamma = 1.2f;
            DepressBrightness = 1;
            DepressConstrast = 1;
            DepressGamma = 1;
        }

        /// <summary>
        /// Adjusts the brightness, constrast, and gamma of an image
        /// </summary>
        private static Image AdjustImage(Image image, float brightness, float contrast, float gamma)
        {
            if (gamma <= 0)
            {
                throw new ArgumentOutOfRangeException("gamma", "Value must be greater than 0");
            }

            float adjustedBrightness = brightness - 1.0f;
            // create matrix that will brighten and contrast the image
            float[][] ptsArray =
            {
                new[] {contrast, 0, 0, 0, 0}, // scale red
                new[] {0, contrast, 0, 0, 0}, // scale green
                new[] {0, 0, contrast, 0, 0}, // scale blue
                new[] {0, 0, 0, 1.0f, 0},     // don't scale alpha
                new[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}
            };

            Bitmap adjustedImage = new Bitmap(image);

            using (var imageAttributes = new ImageAttributes())
            {
                imageAttributes.ClearColorMatrix();
                imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), 
                    ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
       
                using (Graphics graphics = Graphics.FromImage(adjustedImage))
                {
                    Rectangle rect = new Rectangle(Point.Empty, adjustedImage.Size);
                    graphics.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }

            return adjustedImage;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (_hideFocusBorder && this.Parent != null)
            {
                ControlPaint.DrawBorder(pevent.Graphics, this.ClientRectangle,
                    this.Parent.BackColor, ButtonBorderStyle.Solid);
            }
        }

        #region Mouse Events
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (this.Image != null)
            {
                _leaveImage = this.Image;
                this.Image = AdjustImage(this.Image, HoverBrightness, HoverContrast, HoverGamma);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (this.Image != null)
            this.Image = _leaveImage;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (this.Image != null)
            {
                this.Image = AdjustImage(this.Image, DepressBrightness, DepressConstrast, DepressGamma);
            }
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            if (this.Image != null)
            this.Image = _leaveImage;
        }
        #endregion

        #region Properties
        public override bool AutoSize
        {
            get
            {
                // So it dosent get all small
                if (Image == null) return false;
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = string.Empty; }
        }

        /// <summary>
        /// Hide selection rectangle
        /// </summary>
        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        private bool _hideFocusBorder = true;
        /// <summary>
        /// This differs from the selection rectangle
        /// </summary>
        [DefaultValue(true)]
        [Description("If true, the black focus rectangle that appears when the button is the forms accept button, will be hidden")]
        [Category("Appearance")]
        public bool HideFocusBorder
        {
            get { return _hideFocusBorder; }
            set { _hideFocusBorder = value; }
        }

        [Description("The brightness of the image when the mouse hovers over this button")]
        [Category("Appearance")]
        [DefaultValue(1)]
        public float HoverBrightness { get; set; }

        [Description("The constrast of the image when the mouse hovers over this button")]
        [Category("Appearance")]
        [DefaultValue(1)]
        public float HoverContrast { get; set; }

        [Description("The gamma of the image when the mouse hovers over this button")]
        [Category("Appearance")]
        [DefaultValue(1.2f)]
        public float HoverGamma { get; set; }

        [Description("The brightness of the image when the mouse depresses this button")]
        [Category("Appearance")]
        [DefaultValue(1)]
        public float DepressBrightness { get; set; }

        [Description("The constrast of the image when the mouse depresses this button")]
        [Category("Appearance")]
        [DefaultValue(1)]
        public float DepressConstrast { get; set; }

        [Description("The gamma of the image when the mouse depresses this button")]
        [Category("Appearance")]
        [DefaultValue(1.2f)]
        public float DepressGamma { get; set; }
        #endregion
    }
}
