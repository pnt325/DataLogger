using System;
using System.Windows.Forms;
using AxWMPLib;
using System.Drawing;
using WMPLib;
using System.ComponentModel;

namespace AboControls.UserControls
{
    /// <summary>
    /// The target platform for the project that uses this control, must be x86, 
    /// or the assemblies will fail to load
    /// </summary>
    class MediaPlayer : Control
    {
        // Do not use as flags
        public enum UserInterfaceMode { None, Invisible, Mini, Full };
        public enum MediaMode { AutoRewind, Loop, ShowFrame, Shuffle };

        private AxWindowsMediaPlayer _mediaPlayer = new AxWindowsMediaPlayer();
        private Timer _timerUpdate = new Timer();
        private Pen _pen = new Pen(Color.Blue, 0f);
        private bool _halfBorder;
        private double _lastPlayTime;
        public event EventHandler PlayHeadMoved;

        public MediaPlayer()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            // We cannot add the mediaPlayer to a parent outside of begin and end init
            _mediaPlayer.BeginInit();
            this.Controls.Add(_mediaPlayer);
            _mediaPlayer.EndInit();
            _mediaPlayer.PlayStateChange += _mediaPlayer_PlayStateChange;
            _mediaPlayer.Visible = true;
            _mediaPlayer.Size = this.Size = new Size(300, 200);
            _mediaPlayer.Location = Point.Empty;
            _mediaPlayer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            _timerUpdate.Interval = 200;
            _timerUpdate.Tick += _timerUpdate_Tick;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_pen.Width > 0)
            {
                if (_halfBorder)
                {
                    e.Graphics.DrawRectangle(_pen, _pen.Width / 2, _pen.Width / 2, 
                        this.Width - _pen.Width * 2, this.Height - _pen.Width * 2);
                }
                else
                {
                    e.Graphics.DrawRectangle(_pen, _pen.Width / 2, _pen.Width / 2, 
                        this.Width - _pen.Width, this.Height - _pen.Width);
                }
            }
        }

        protected virtual void OnPlayHeadMoved()
        {
            if (PlayHeadMoved != null)
            {
                PlayHeadMoved(this, EventArgs.Empty);
            }
        }

        protected virtual void _mediaPlayer_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            WMPPlayState currentPlayState = (WMPPlayState)e.newState;

            if (currentPlayState == WMPPlayState.wmppsPlaying)
            {
                _timerUpdate.Start();
            }
            else
            {
                _timerUpdate.Stop();
            }
        }

        private void _timerUpdate_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer.Disposing || !_mediaPlayer.Created)
            {
                _timerUpdate.Stop();
                return;
            }

            // compare lastSave time to current to detect change
            if (_lastPlayTime != _mediaPlayer.Ctlcontrols.currentPosition)
            {
                OnPlayHeadMoved();
                // Capture last play time pos
                _lastPlayTime = _mediaPlayer.Ctlcontrols.currentPosition;
            }
        }

        /// <summary>
        /// Plays the currently loaded media
        /// </summary>
        public void Play()
        {
            _mediaPlayer.Ctlcontrols.play();
        }

        /// <summary>
        /// Stops the currently loaded media
        /// </summary>
        public void Stop()
        {
            _mediaPlayer.Ctlcontrols.stop();
        }

        public void Pause()
        {
            _mediaPlayer.Ctlcontrols.pause();
        }

        public void FastForward()
        {
            _mediaPlayer.Ctlcontrols.fastForward();
        }

        public void Rewind()
        {
            _mediaPlayer.Ctlcontrols.fastReverse();
        }

        public void NextItem()
        {
            _mediaPlayer.Ctlcontrols.next();
        }

        public void PreviousItem()
        {
            _mediaPlayer.Ctlcontrols.previous();
        }

        /// <summary>
        /// Enables or disables specific modes pertaining to media (mostly playback). (Enum bit flagging not implemented)
        /// AutoRewind: Mode indicating whether the tracks are rewound to the beginning after playing to the end. Default state is true.
        /// Loop: Mode indicating whether the sequence of tracks repeats itself. Default state is false.
        /// ShowFrame: Mode indicating whether the nearest video key frame is displayed at the current position 
        /// when not playing. Default state is false. Has no effect on audio tracks.
        /// Shuffle: Mode indicating whether the tracks are played in random order. Default state is false.
        /// </summary>
        public void SetMediaMode(MediaMode mediaMode, bool setState)
        {
            _mediaPlayer.settings.setMode(mediaMode.ToString(), setState);
        }

        public bool GetMediaModeState(MediaMode mediaMode)
        {
            return _mediaPlayer.settings.getMode(mediaMode.ToString());
        }

        /// <summary>
        /// Sets the user interface mode.
        /// None will keep the ui from loading.
        /// Mini will display a compact version of the ui.
        /// Invisible will hide the ui.
        /// Full will display all elements of the ui (default).
        /// </summary>
        public void SetUIMode(UserInterfaceMode mode)
        {
            _mediaPlayer.uiMode = mode.ToString().ToLower();
        }

        /// <summary>
        /// Loads all of the users music into a playlist and sets it as the current. If 
        /// no such playlist can be found or created, then return false.
        /// </summary>
        public bool LoadAllMusicPlaylist()
        {
            // Get an interface to the first playlist from the library. 
            IWMPPlaylistArray playlists = _mediaPlayer.playlistCollection.getByName("All Music");

            if (playlists.count > 0)
            {
                // Make the retrieved playlist the current playlist.
                _mediaPlayer.currentPlaylist = playlists.Item(0);
                return true;
            }

             return false;
        }

        /// <summary>
        /// Opens a file dialog to pick and load files into the media player, check the
        /// MediaLocation after successful pick
        /// </summary>
        /// <returns>True if a file has been picked, false if not</returns>
        public bool PickMediaFile()
        {
            using (OpenFileDialog dlgOpenFileDialog = new OpenFileDialog())
            {
                // The control is just an interface and should support the vast amount
                // of file types the actual app does.
                dlgOpenFileDialog.Filter = "All Files|*.*";

                if (dlgOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _mediaPlayer.URL = dlgOpenFileDialog.FileName;
                    return true;
                }
            }

            return false;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the location of the media. This can be a URL or a path
        /// to a local file
        /// </summary>
        [Category("Media")]
        [Description("The URL or local path of a media file to be played")]
        // For re-setting in property browser
        [DefaultValue("")]
        public string MediaLocation
        {
            get { return _mediaPlayer.URL; }
            set { _mediaPlayer.URL = value; }
        }

        /// <summary>
        /// Gets or sets the location of the media. This can be a URL or a path
        /// to a local file
        /// </summary>
        [Browsable(false)]
        [Category("Media")]
        [Description("The URL or local path of a media file to be played")]
        public double MediaDuration
        {
            get
            {
                if (_mediaPlayer.currentMedia == null)
                    return 0;

                return _mediaPlayer.currentMedia.duration;
            }
        }

        /// <summary>
        /// Gets or sets the play head update speed in milliseconds, the default update
        /// speed for the internal PlayHead is about 1/4th of a second. The lower the number
        /// the smoother visuals will appear to the user
        /// </summary>
        [Category("Media")]
        [DefaultValue(200)]
        [Description("The interval time in milliseconds between playhead update events")]
        public int PlayHeadUpdateInterval
        {
            get { return _timerUpdate.Interval; }
            set { _timerUpdate.Interval = value; }
        }

        [Category("Media")]
        [Description("The current play time elapsed in seconds")]
        [DefaultValue(0)]
        public double ElapsedPlayTime
        {
            get { return _mediaPlayer.Ctlcontrols.currentPosition; }
            set { _mediaPlayer.Ctlcontrols.currentPosition = value; }
        }

        /// <summary>
        /// Gets or sets the play progress (or the play head position) in a percentage from 0-100.
        /// If no media is loaded, the play progress defaults to 0 when retrieved.
        /// </summary>
        [Category("Media")]
        [Description("The current play progress of the loaded media. The percentage ranges from 0-100")]
        public double PlayProgress
        {
            get
            {
                if (_mediaPlayer.currentMedia != null)
                {
                    return _mediaPlayer.Ctlcontrols.currentPosition / _mediaPlayer.currentMedia.duration * 100;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (_mediaPlayer.currentMedia != null)
                {
                    double pos = value / 100 * _mediaPlayer.currentMedia.duration;

                    if (pos < 0)
                    {
                        _mediaPlayer.Ctlcontrols.currentPosition = 0;
                    }
                    else if (pos > 100)
                    {
                        _mediaPlayer.Ctlcontrols.currentPosition = 100;
                    }
                    else
                    {
                        _mediaPlayer.Ctlcontrols.currentPosition = pos;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that determines the playback screen state. Media
        /// must be loaded to enter fullscreen or an exception will raise. We also, 
        /// cannot enter full-screen after media has been stopped or a catastrophic failure will occur
        /// </summary>
        [Category("Media")]
        [DefaultValue(false)]
        [Description("Determines the playback screen state. Media must be loaded to enter full-screen or an exception will raise.")]
        public bool FullScreen
        {
            get { return _mediaPlayer.fullScreen; }
            set
            {
                if (value && _mediaPlayer.URL.Length.Equals(0) || _mediaPlayer.playState == WMPPlayState.wmppsStopped)
                {
                    return;
                }

                _mediaPlayer.fullScreen = value;
            }
        }

        /// <summary>
        /// Gets the enum that denotes the current media playback state.
        /// </summary>
        [Browsable(false)]
        public WMPPlayState PlayState
        {
            get { return _mediaPlayer.playState; }
        }

        /// <summary>
        /// Gets or sets whether or not the media will automatically start on load
        /// </summary>
        [Description("Determines whether or not the media will automatically start on load")]
        [DefaultValue(true)]
        [Category("Media")]
        public bool AutoStart
        {
            get { return _mediaPlayer.settings.autoStart; }
            set { _mediaPlayer.settings.autoStart = value; }
        }


        /// <summary>
        /// Gets or sets the play speed of the current media, typical speed range
        /// between -10 and 10. Anywhere beyond will not take effect.
        /// </summary>
        [Description(@"Gets or sets the play speed of the current media, typical speed range
        between -10 and 10. Anywhere beyond will not take effect.")]
        [DefaultValue(50)]
        [Category("Media")]
        public double PlaySpeed
        {
            get { return _mediaPlayer.settings.rate; }
            set { _mediaPlayer.settings.rate = value; }
        }

        /// <summary>
        /// Mutes or unmutes playback within the control
        /// </summary>
        [Description("Mutes or unmutes playback within the player")]
        [DefaultValue(false)]
        [Category("Media")]
        public bool Mute
        {
            get { return _mediaPlayer.settings.mute; }
            set { _mediaPlayer.settings.mute = value; }
        }

        /// <summary>
        /// Gets or sets the volume control of the media player settings. No need to check bounds
        /// to my surprise, it is already done internally
        /// </summary>
        [Description("Controls the volume of playback within the player")]
        [DefaultValue(50)]
        [Category("Media")]
        public int Volume
        {
            get { return _mediaPlayer.settings.volume; }
            set { _mediaPlayer.settings.volume = value; }
        }

        /// <summary>
        /// Gets or sets the pen for the border of the control, the child will automatically be
        /// positioned and sized according to the border. I had to switch from docking to anchoring for this
        /// </summary>
        [Category("Appearance")]
        [Browsable(false)]
        public Pen BorderPen
        {
            get { return _pen; }
            set
            {
                _pen = value;
                _mediaPlayer.Top = (int)(_pen.Width + 0.5);
                _mediaPlayer.Left = (int)(_pen.Width + 0.5);
                _mediaPlayer.Width = this.Width - (int)(_pen.Width + 0.5) * 2;
                _mediaPlayer.Height = this.Height - (int)(_pen.Width + 0.5) * 2;
                this.Invalidate(false);
            }
        }

        /// <summary>
        /// Gets or sets the value that determines if only half of the border will appear. 
        /// you can use this to apply a limited styed border to the control. The to-left of
        /// the control will have the border painted.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool HalfBorder
        {
            get { return _halfBorder; }
            set 
            { 
                _halfBorder = value;
                this.Invalidate(false);
            }
        }
        #endregion

        public new void Dispose()
        {
            _mediaPlayer.Dispose();
        }
    }
}
