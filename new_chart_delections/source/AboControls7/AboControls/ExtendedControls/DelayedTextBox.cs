using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace AboControls.ExtendedControls
{
    class DelayedTextBox : TextBox
    {
        private readonly Timer _timer = new Timer();
        [Description("Occurs after a certain period has elapsed after the user has finished typing")]
        public event EventHandler TextChangedDelayed;

        public DelayedTextBox()
        {
            _timer.Interval = 400;
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            if (TextChangedDelayed != null)
                TextChangedDelayed(this, EventArgs.Empty);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            _timer.Stop();
            _timer.Start();
        }

        /// <summary>
        /// The amount of time that needs to elapse after the last text change, before the TextChangedDelayed 
        /// event is to be raised
        /// </summary>
        [DefaultValue(400)]
        [Category("Behavior")]
        public int Delay
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }
    }
}
