using System.Windows.Forms;

namespace AboControls.ExtendedControls
{
    class OverlayControl : Control
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Don't paint background
        }
    }
}
