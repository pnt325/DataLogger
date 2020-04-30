using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    public enum InterchangeMode { Linear, Random, Reverse };

    public class InterchangablePictureBox : PictureBox
    {
        private int _imageIndex, _lastIndex;
        private readonly Timer _tmrInterchanger = new Timer();
        [Description("Occurs when the value of the ImageIndex property has changed")]
        public event EventHandler ImageIndexChanged;

        public InterchangablePictureBox()
        {
            _tmrInterchanger.Tick += tmrInterchanger_Tick;
            _tmrInterchanger.Interval = 1000;
        }

        private void tmrInterchanger_Tick(object sender, EventArgs e)
        {
            if (Images.Images.Count == 0) return;

            if (ChangeMode == InterchangeMode.Linear)
            {
                bool isAtEnd = (_imageIndex >= Images.Images.Count - 1);
                ImageIndex = (isAtEnd) ? 0 : _imageIndex + 1;
            }
            else if (ChangeMode == InterchangeMode.Random)
            {
                Random randomNum = new Random();
                ImageIndex = randomNum.Next(0, Images.Images.Count);

                //Make sure the same image isnt displayed twice in a row
                while (_imageIndex == _lastIndex)
                {
                    ImageIndex = randomNum.Next(0, Images.Images.Count);
                }

                _lastIndex = _imageIndex;
            }
            else if (ChangeMode == InterchangeMode.Reverse)
            {
                bool isAtStart = (_imageIndex == 0);
                ImageIndex = (isAtStart) ? Images.Images.Count - 1 : _imageIndex - 1;
            }
        }

        public void StartInterchange()
        {
            _tmrInterchanger.Start();
        }

        public void StopInterchange()
        {
            _tmrInterchanger.Stop();
        }

        public void GotoStartImage()
        {
            ImageIndex = 0;
        }

        public void GotoEndImage()
        {
            if (Images.Images.Count > 0)
            ImageIndex = Images.Images.Count - 1;
        }

        #region Properties
        [DefaultValue(1000)]
        [Description("The delay in time between the changing of images")]
        [Category("Behavior")]
        public int InterchangeInterval
        {
            get { return _tmrInterchanger.Interval; }
            set { _tmrInterchanger.Interval = value; }
        }

        [Description("Sets the current image list index")]
        [DefaultValue(0)]
        public int ImageIndex
        {
            set
            {
                _imageIndex = value;
                this.Image = Images.Images[_imageIndex];

                if (ImageIndexChanged != null) 
                    ImageIndexChanged(this, EventArgs.Empty);
            }
        }

        [Description("The list of images to cycle through")]
        [Browsable(false)]
        public ImageList Images { get; set; }

        [Description("The way the control cycles through the images")]
        [Category("Behavior")]
        [DefaultValue(InterchangeMode.Linear)]
        public InterchangeMode ChangeMode { get; set; }
        #endregion
    }
}
