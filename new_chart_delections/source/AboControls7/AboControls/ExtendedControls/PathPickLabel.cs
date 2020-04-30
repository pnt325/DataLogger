using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace AboControls.ExtendedControls
{
    class PathPickLabel : Label
    {
        private string _path;
        private Color _mouseLeaveColor = Color.FromKnownColor(KnownColor.Control);

        public PathPickLabel()
        {
            MouseEnterColor = Color.FromKnownColor(KnownColor.ControlLight);
            ShowFullPath = true;
            this.AutoEllipsis = true;
            base.BorderStyle = BorderStyle.FixedSingle;
            base.TextAlign = ContentAlignment.MiddleCenter;
            base.Cursor = Cursors.Hand;
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            base.AutoSize = false;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.BackColor = MouseEnterColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = _mouseLeaveColor;
        }

         [Category("Appearance")]
        public Color MouseLeaveColor
        {
            get { return _mouseLeaveColor; }
            set 
            {
                _mouseLeaveColor = value;
                this.BackColor = value;
            }
        }

        [Category("Appearance")]
        public Color MouseEnterColor { get; set; }

        [Category("Data")]
        [Description("Determines whether or not to show the full path or the filename with the extension. Only for filepaths")]
        public bool ShowFullPath { get; set; }

        [Category("Data")]
        [Description("The full path of the directory or filename")]
        public string FullPath
        {
            get { return _path; }
            set 
            {
                _path = value;
                this.Text = (ShowFullPath) ? _path : Path.GetFileName(_path);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        public override string Text
        {
            get { return base.Text; }
            set { base.Text = String.IsNullOrEmpty(_path) ? "..." : value; }
        }
    }
}
