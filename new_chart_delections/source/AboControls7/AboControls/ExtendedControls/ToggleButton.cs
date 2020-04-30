using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    public enum InitialText { UnCheckedText, CheckedText, CustomText };

    public class ToggleButton : CheckBox
    {
        private InitialText _initialText;

        public ToggleButton()
        {
            this.Appearance = Appearance.Button;
            base.AutoSize = false;
            base.TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            this.Text = (this.Checked) ? CheckedText : UnCheckedText;
        }

        [Category("State")]
        [DefaultValue(InitialText.UnCheckedText)]
        [Description("The text to show before the button has been used")]
        [DisplayName("Initial Text")]
        public InitialText InitText
        {
            get { return _initialText; }
            set
            {
                _initialText = value;

                if (_initialText == InitialText.CheckedText)
                {
                    this.Text = CheckedText;
                }
                else if (_initialText == InitialText.UnCheckedText)
                {
                    this.Text = UnCheckedText;
                }
            }
        }

        [Category("State"), Description("The text to show when the button is checked"), DisplayName("Checked Text")]
        public string CheckedText { get; set; }

        [Category("State"), Description("The text to show when the button is un-checked"), DisplayName("Un-checked Text")]
        public string UnCheckedText { get; set; }

        [Category("State")]
        [Description("A custom caption to be the initial text of" +
            " the button instead of the unchecked and checked state captions")]
        [DisplayName("Custom Caption")]
        public string CustomCaption
        {
            get { return this.Text; }
            set { this.Text = value; }
        }
    }
}
