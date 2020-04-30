using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AboControls.ExtendedControls
{
    class RolloverButton : Button
    {
        public RolloverButton()
        {
            base.Cursor = Cursors.Hand;
            base.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //Disable addtional graphics
            this.FlatAppearance.BorderSize = 0;
            base.BackColor = Color.Transparent;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            // Comment out this code if not using
            SetSampleImages();
        }

        /// <summary>
        /// Sets sample images for mouse enter and mouseleave
        /// </summary>
        private void SetSampleImages()
        {
            var mouseEnterBitmap = new Bitmap(this.Width, this.Height);
            var mouseLeaveBitmap = new Bitmap(this.Width, this.Height);
            var enterRect = new Rectangle(Point.Empty, mouseEnterBitmap.Size);

            // Draw mouse enter image
            using (Graphics graphicsEnter = Graphics.FromImage(mouseEnterBitmap))
            {
                using (var lgbEnter = new LinearGradientBrush(enterRect, Color.GreenYellow, Color.YellowGreen, 90f))
                {
                    graphicsEnter.FillRectangle(lgbEnter, 0, 0, this.Width, this.Height);
                }
            }

            MouseEnterImage = mouseEnterBitmap;

            // Draw mouse leave image
            using (Graphics graphicsLeave = Graphics.FromImage(mouseLeaveBitmap))
            {
                using (var lgbLeave = new LinearGradientBrush(enterRect, Color.Gray, Color.LightGray, 90f))
                {
                    graphicsLeave.FillRectangle(lgbLeave, 0, 0, this.Width, this.Height);
                }
            }

            MouseLeaveImage = mouseLeaveBitmap;
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

            if (MouseEnterImage != null)
                this.Image = MouseEnterImage;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (_mouseLeaveImage != null)
                this.Image = _mouseLeaveImage;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (MouseDownImage != null)
                this.Image = MouseDownImage;
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            if (MouseEnterImage != null)
                this.Image = MouseEnterImage;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Hide selection rectangle
        /// </summary>
        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        private Image _mouseLeaveImage;
        [Description("The image to display when the cursor enters the control"), Category("Effects")]
        public Image MouseEnterImage { get; set; }

        /// <summary>
        /// Gets or sets the mouse leave image
        /// </summary>
        [Description("The image to display when the cursor leaves the control")]
        [Category("Effects")]
        public Image MouseLeaveImage
        {
            get { return _mouseLeaveImage; }
            set 
            { 
                _mouseLeaveImage = value;

                if (_autoSetImage)
                {
                    this.Image = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the mouse down image
        /// </summary>
        [DefaultValue(null), Category("Effects"), Description("The image that is displayed when the mouse is down on the control")]
        public Image MouseDownImage { get; set; }

        private bool _autoSetImage = true;
        /// <summary>
        /// Gets or sets the value that denotes whether or not the Image property
        /// will adapt to the "MouseLeaveImage" property
        /// </summary>
        [DefaultValue(true)]
        [Description("If true, the \"Image\" property will be assigned the image of the \"MouseLeaveImage\" propery on modify")]
        [Category("Behavior")]
        public bool AutoSetImage
        {
            get { return _autoSetImage; }
            set { _autoSetImage = value; }
        }

        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set {  base.Text = string.Empty; }
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
        #endregion
    }
}
