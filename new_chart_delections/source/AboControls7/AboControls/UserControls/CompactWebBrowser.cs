using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AboControls.UserControls
{
    public partial class CompactWebBrowser : UserControl
    {
        private bool _useOwnHomePage;
        private string _homepage = "http://www.google.com/";

        public CompactWebBrowser()
        {
            InitializeComponent();
        }

        private void btnNavigate_Click(object sender, EventArgs e)
        {
            NavigateToPage();
        }

        private void txtAdress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                NavigateToPage();
            }
        }

        private void NavigateToPage()
        {
            if (txtAdress.Text.Length > 0)
            {
                if (webBrowser.Url != null)
                {
                    //Do not navigate to a page twice
                    //That is what the refresh button id for
                    if (webBrowser.Url.AbsoluteUri.Equals(txtAdress.Text))
                    {
                        return;
                    }
                }
                webBrowser.Navigate(txtAdress.Text);
            }
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            webBrowser.GoBack();
        }

        private void btnGoForward_Click(object sender, EventArgs e)
        {
            webBrowser.GoForward();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            webBrowser.Stop();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            GoHome();
        }

        private void GoHome()
        {
            if (_useOwnHomePage)
                webBrowser.Navigate(_homepage);
            else
                webBrowser.GoHome();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //Display the full url in address bar after navigating
            txtAdress.Text = webBrowser.Url.AbsoluteUri;
        }

        private void CompactWebBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Home)
            {
                GoHome();
            }
            else if (e.KeyCode == Keys.Back)
            {
                webBrowser.GoBack();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawRectangle(Pens.Black, 1, 1, this.Width - 2, this.Height - 2);
            e.Graphics.DrawRectangle(Pens.Black, webBrowser.Location.X - 1,
                webBrowser.Location.Y - 1, webBrowser.Width + 2, webBrowser.Height + 2);
        }

        /// <summary>
        /// Gets or sets the value that denotes the homepage type to be used.
        /// When enabled, The user may specify its own home page in place of the
        /// internet settings; bound homepage
        /// </summary>
        [Description("When enabled, the webrowser will navigate to a user defined homepage")]
        [Category("Behavior")]
        [DisplayName("Use Own Home Page")]
        [DefaultValue(false)]
        public bool UseOwnHomePage
        {
            get { return _useOwnHomePage; }
            set { _useOwnHomePage = value; }
        }

        [Description("This is the homepage to be navigated to when UseOwnHomePage is enabled")]
        [Category("Behavior")]
        [DefaultValue("http://www.google.ca/")]
        public string HomePage
        {
            get { return _homepage; }
            set { _homepage = value; }
        }
    }
}
