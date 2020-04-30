using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AboControls.ExtendedControls
{
    class FilenameTextbox : TextBox
    {
        private bool _replaceSpaces;
        private bool _pendingFilter;
        private bool _filterEnabled = true;

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (!_filterEnabled) return;

            if (e.KeyChar.Equals(' ') || e.KeyChar.Equals('_'))
            {
                // Do not allow spaces at the start or end of the text
                if (this.SelectionStart.Equals(0))
                {
                    e.Handled = true;
                }
                // Do not allow spaces if there is a space around already the selection index
                else if ((this.SelectionStart - 1 > 0 && this.Text[this.SelectionStart - 1].Equals(' '))
                    || this.SelectionStart > this.TextLength && this.Text[this.SelectionStart].Equals(' '))
                {
                    e.Handled = true;
                }
                // Do not allow spaces if there is a space around already the selection index
                else if ((this.SelectionStart - 1 > 0 && this.Text[this.SelectionStart - 1].Equals('_'))
                    || this.SelectionStart > this.TextLength && this.Text[this.SelectionStart].Equals('_'))
                {
                    e.Handled = true;
                }
            }

            // Handle reserved characters
            else if (e.KeyChar.Equals('>') ||
                e.KeyChar.Equals('<') ||
                e.KeyChar.Equals(':') ||
                e.KeyChar.Equals('"') ||
                e.KeyChar.Equals('/') ||
                e.KeyChar.Equals('\\') ||
                e.KeyChar.Equals('|') ||
                e.KeyChar.Equals('?') ||
                e.KeyChar.Equals('*'))
            {
                e.Handled = true;
            }

            if (_replaceSpaces && e.KeyChar.Equals(' '))
            {
                e.KeyChar = '_';
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode.Equals(Keys.V) && e.Control)
            {
                _pendingFilter = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (_pendingFilter)
            {
                FilterText();
            }
        }

        private void FilterText()
        {
            if (!_filterEnabled) return;

            // Trim spacing
            this.Text = this.Text.Trim(' ', '_');
            // Filter reserved characters ><:"/\|?*
            this.Text = Regex.Replace(this.Text, @"[\>\<:""/\\\|\?\*]", string.Empty);

            if (_replaceSpaces)
            {
                this.Text = this.Text.Replace(' ', '_');
                // Replace more than one successive underscores with one underscore
                this.Text = Regex.Replace(this.Text, @"__+", " ");
            }
            else
            {
                // Replace more than one successive spaces with one space
                // hello    sdfdfd
                this.Text = Regex.Replace(this.Text, @"\s\s+", " ");
            }

            _pendingFilter = false;
        }

        [Category("Behavior")]
        [Description("When set to true, replaces space characters with underscores")]
        [DefaultValue(false)]
        public bool ReplaceSpacesWithUnderscores
        {
            get { return _replaceSpaces; }
            set { _replaceSpaces = value; }
        }

        [Category("Behavior")]
        [Description("When set to true, restricts the characters the user can enter to file safe ones")]
        [DefaultValue(true)]
        public bool FilterEnabled
        {
            get { return _filterEnabled; }
            set 
            { 
                _filterEnabled = value;

                if (_filterEnabled)
                {
                    FilterText();
                }
            }
        }
    }

}
