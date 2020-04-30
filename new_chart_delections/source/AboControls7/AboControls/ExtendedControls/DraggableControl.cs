using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace AboControls.ExtendedControls
{
    class DraggableControl : Control
    {
        private Point _lastPos;
        private bool _movable = true;

        public DraggableControl()
        {
            this.Cursor = Cursors.Hand;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_movable)
            {
                _lastPos = e.Location;
                this.Cursor = Cursors.SizeAll;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_movable)
            {
                this.Cursor = Cursors.Hand;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_movable && e.Button == MouseButtons.Left)
            {
                this.Left += e.Location.X - _lastPos.X;
                this.Top += e.Location.Y - _lastPos.Y;
            }
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Determines whether or not the control can be moved by the user")]
        public bool Movable
        {
            get { return _movable; }
            set
            {
                _movable = value;

                if (_movable)
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }
    }
}
