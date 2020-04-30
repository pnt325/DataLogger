using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace AboControls.ExtendedControls
{
    public enum EmbossDirection { TopLeft, TopRight, BottomLeft, BottomRight };
    public enum EmbossStyle { ShadowCast, Extrude };

    public class EmbossLabel : Control
    {
        private int _dropAmount = 5;
        private int _textContrast = 4;
        // This needs to be global because the GFX will be reassigned
        // and I need like to retain the quality in the UpdateGraphicsBuffer method
        private bool _isHighQuality = true;
        private EmbossStyle _embossStyle = EmbossStyle.Extrude;
        private TextRenderingHint _textRenderingHint = TextRenderingHint.AntiAliasGridFit;
        private BufferedGraphics _bufGraphics;
        private readonly BufferedGraphicsContext _bufContext = BufferedGraphicsManager.Current;
        private SolidBrush _surfaceBrush;
        private SolidBrush _embossBrush = new SolidBrush(Color.Black);
        private Size _textSize;
        private EmbossDirection _embossDirection = EmbossDirection.BottomRight;
        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;

        public EmbossLabel()
        {
            this.Font = new Font("Tahoma", 30f);
            this.ForeColor = Color.YellowGreen;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            UpdateGraphicsBuffer();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _bufGraphics.Graphics.Clear(this.BackColor);
            Render();
            _bufGraphics.Render(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateGraphicsBuffer();
            this.Update();
        }

        private void UpdateGraphicsBuffer()
        {
            // We cannot create a bitmap if any of these values are 0
            if (this.Size.Width > 0 && this.Size.Height > 0)
            {
                _bufContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufGraphics = _bufContext.Allocate(this.CreateGraphics(), this.ClientRectangle);

                if (_isHighQuality)
                {
                    _bufGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    _bufGraphics.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                    _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                }
                else
                {
                    _bufGraphics.Graphics.SmoothingMode = SmoothingMode.Default;
                    _bufGraphics.Graphics.CompositingQuality = CompositingQuality.Default;
                    _bufGraphics.Graphics.PixelOffsetMode = PixelOffsetMode.Default;
                }
            }
        }

        /// <summary>
        /// Gets the location of the text according to the text alignment
        /// </summary>
        private Point GetAlignmentLocation()
        {
            switch (_textAlign)
            {
                case ContentAlignment.BottomCenter:
                    {
                        int x = (this.Width / 2) - (_textSize.Width / 2);
                        int y = this.Height - _textSize.Height;
                        return new Point(x, y);
                    }

                case ContentAlignment.BottomLeft:
                    {
                        return new Point(0, this.Height - _textSize.Height);
                    }

                case ContentAlignment.BottomRight:
                    {
                        return new Point(this.Width - _textSize.Width, this.Height - _textSize.Height);
                    }

                case ContentAlignment.MiddleCenter:
                    {
                        int x = (this.Width / 2) - (_textSize.Width / 2);
                        int y = (this.Height / 2) - (_textSize.Height / 2);
                        return new Point(x, y);
                    }

                case ContentAlignment.MiddleLeft:
                    {
                        int y = this.Height / 2 - (_textSize.Height / 2);
                        return new Point(0, y);
                    }

                case ContentAlignment.MiddleRight:
                    {
                        int y = (this.Height / 2) - (_textSize.Height / 2);
                        int x = this.Width - _textSize.Width;
                        return new Point(x, y);
                    }

                case ContentAlignment.TopCenter:
                    {
                        int x = (this.Width / 2) - (_textSize.Width / 2);
                        return new Point(x, 0);
                    }

                case ContentAlignment.TopRight:
                    {
                        int x = this.Width - _textSize.Width;
                        return new Point(x, 0);
                    }

                default: return Point.Empty;
            }
        }

        private void Render()
        {
            // Update textsize here for simplicity
            _textSize = _bufGraphics.Graphics.MeasureString(this.Text, this.Font).ToSize();
            Point shadowLayerPos, topLayerPos;
            // Disable size restrictions so we can set the actual size
            this.MinimumSize = this.MaximumSize = new Size(0, 0);

            if (this.AutoSize)
            {
                // No text alignment needed with autosize
                shadowLayerPos = topLayerPos = Point.Empty;
                // Only adjust size when autosizing
                this.Size = new Size(_textSize.Width, _textSize.Height);
                // Lock size
                this.MinimumSize = this.MaximumSize = this.Size;
            }
            else
            {
                shadowLayerPos = GetAlignmentLocation();
                topLayerPos = shadowLayerPos;
            }

            OffsetShadowLayer(ref shadowLayerPos);

            if (_embossStyle.Equals(EmbossStyle.Extrude))
            {
                // Repaint the shadow along a diagonal path to create a 3D effect
                while (!shadowLayerPos.X.Equals(topLayerPos.X) && !shadowLayerPos.Y.Equals(topLayerPos.Y))
                {
                    if (shadowLayerPos.X < topLayerPos.X)
                    {
                        shadowLayerPos.X++;
                    }
                    else if (shadowLayerPos.X > topLayerPos.X)
                    {
                        shadowLayerPos.X--;
                    }

                    if (shadowLayerPos.Y < topLayerPos.Y)
                    {
                        shadowLayerPos.Y++;
                    }
                    else if (shadowLayerPos.Y > topLayerPos.Y)
                    {
                        shadowLayerPos.Y--;
                    }

                    _bufGraphics.Graphics.DrawString(this.Text, this.Font, _embossBrush, shadowLayerPos);
                }
            }
            else if (_embossStyle.Equals(EmbossStyle.ShadowCast))
            {
                _bufGraphics.Graphics.DrawString(this.Text, this.Font, _embossBrush, shadowLayerPos);
            }

            _bufGraphics.Graphics.DrawString(this.Text, this.Font, _surfaceBrush, topLayerPos);
        }

        /// <summary>
        /// Offsets the shadow layer, the shadow will move in relation to
        /// the top layer, not the other way around
        /// </summary>
        private void OffsetShadowLayer(ref Point layerPos)
        {
            switch (_embossDirection)
            {
                case EmbossDirection.TopLeft:
                    layerPos = new Point(layerPos.X - _dropAmount,
                        layerPos.Y - _dropAmount);
                    break;

                case EmbossDirection.TopRight:
                    layerPos = new Point(layerPos.X + _dropAmount,
                        layerPos.Y - _dropAmount);
                    break;

                case EmbossDirection.BottomLeft:
                    layerPos = new Point(layerPos.X - _dropAmount,
                        layerPos.Y + _dropAmount);
                    break;

                case EmbossDirection.BottomRight:
                    layerPos = new Point(layerPos.X + _dropAmount,
                        layerPos.Y + _dropAmount);
                    break;
            }
        }

        #region Properties
        [Category("Appearance")]
        [Description("Determines whether or not the graphics are high quality")]
        [DefaultValue(true)]
        public bool HighQuality
        {
            get { return _isHighQuality; }
            set
            {
                _isHighQuality = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The distance in pixels the end of the emboss will be")]
        [DefaultValue(5)]
        public int EmbossAmount
        {
            get { return _dropAmount; }
            set
            {
                _dropAmount = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        [DisplayName("Shadow Position")]
        [Description("The position of the shadow")]
        [DefaultValue((EmbossDirection)3)]
        public EmbossDirection ShadowPos
        {
            get { return _embossDirection; }
            set
            {
                _embossDirection = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "YellowGreen")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                _surfaceBrush = new SolidBrush(value);
                this.Invalidate();
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                this.Invalidate();
                this.Update(); // Needed to update bouds
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                this.Invalidate();
                this.Update();
            }
        }

        [Category("Layout")]
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set
            {
                _textAlign = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                this.Refresh();
            }
        }

        [Category("Appearance")]
        [DefaultValue(TextRenderingHint.AntiAliasGridFit)]
        public TextRenderingHint TextRenderingHint
        {
            get { return _textRenderingHint; }
            set
            {
                _textRenderingHint = value;
                UpdateGraphicsBuffer();
                this.Invalidate();
            }
        }

        [Description("The gamma correction value used for rendering antialiased " +
        "and ClearType text.The values must be between 0 and 12. The default value is 4.")]
        [Category("Appearance")]
        [DefaultValue(4)]
        public int TextContrast
        {
            get { return _textContrast; }
            set
            {
                _textContrast = value;
                UpdateGraphicsBuffer();
                this.Invalidate();
            }
        }

        [Description("The type of extrusion to use")]
        [Category("Appearance")]
        [DefaultValue(EmbossStyle.Extrude)]
        public EmbossStyle EmbossStyle
        {
            get { return _embossStyle; }
            set
            {
                _embossStyle = value;
                this.Invalidate();
            }
        }

        [Editor(typeof(ColorDialog), typeof(ColorDialog))]
        [Description("The color of the embossment")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        public Color EmbossColor
        {
            get
            {
                using (Pen pen = new Pen(_embossBrush))
                {
                    return pen.Color;
                }
            }
            set
            {
                _embossBrush = new SolidBrush(value);
                this.Invalidate();
            }
        }
        #endregion
    }
}
