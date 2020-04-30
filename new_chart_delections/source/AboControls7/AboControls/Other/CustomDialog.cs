using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AboControls.Other
{
    public partial class CustomDialog : Form
    {
        private LinearGradientBrush _LGB;

        public CustomDialog(string title, string message)
        {
            InitializeComponent();

            base.Text = title;
            lblErrorMsg.Text = message;

            AdaptFormToMessage();
            SetupGradientBrush();
        }

        private void SetupGradientBrush()
        {
            Rectangle rect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height - pnlButtonDock.Height);
            _LGB = new LinearGradientBrush(rect, Color.White, Color.FromKnownColor(KnownColor.Control), 90);
        }

        private void AdaptFormToMessage()
        {
            int width = lblErrorMsg.Location.X * 2 + lblErrorMsg.Width;
            int height = lblErrorMsg.Location.Y * 2 + lblErrorMsg.Height + pnlButtonDock.Height;
            this.ClientSize = new Size(width, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int height = this.ClientSize.Height - pnlButtonDock.Height;
            e.Graphics.FillRectangle(_LGB, 0, 0, this.ClientSize.Width, height);
        }
    }
}
