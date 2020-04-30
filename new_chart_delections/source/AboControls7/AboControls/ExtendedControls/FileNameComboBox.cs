using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

namespace AboControls.ExtendedControls
{
    class FileNameComboBox : ComboBox
    {
        private string[] _fileNames = new string[] {};
        private string _lastFileName;
        private bool _retainSelectedFile = true;

        public FileNameComboBox()
        {
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Gets the fileName selected with the path included
        /// </summary>
        [Browsable(false)]
        public string SelectedFullName
        {
            get { return _fileNames[this.SelectedIndex]; }
        }

        [Browsable(false)]
        public string[] FileNames
        {
            get { return _fileNames; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    return;
                }

                // Save selected fileName
                if (this.SelectedIndex != -1)
                {
                    _lastFileName = _fileNames[this.SelectedIndex];
                }

                _fileNames = value;

                this.SuspendLayout();
                this.Items.Clear();

                foreach (string fileName in _fileNames)
                {
                    this.Items.Add(Path.GetFileNameWithoutExtension(fileName));
                }

                this.ResumeLayout();

                if (_retainSelectedFile)
                {
                    // Revert fileName
                    for (int i = 0; i < _fileNames.Length; i++)
                    {
                        if (_fileNames[i] == _lastFileName)
                        {
                            this.SelectedIndex = i;
                        }
                    }
                }
            }
        }

        [Description("When true, the last known fileName will be reselected when new fileNames loaded")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool RetainSelectedFileName
        {
            get { return _retainSelectedFile; }
            set { _retainSelectedFile = value; }
        }
    }
}
