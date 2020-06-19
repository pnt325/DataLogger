using DataLogger.Layout;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataLogger.Core
{
    public class Grid
    {
        // Constant
        #region PROPERTY
        public const int OFFSET_TOP = 24;       // pixel
        public const int OFFSET_BOTTOM = 22;    // pixel
        public const int X_DEFAULT = 4;         // Default grid number
        public const int Y_DEFAULT = 4;

        public static int X { get;  set; }       // Grid size
        public static int Y { get;  set; }
        public static int StepX { get; set; }              // Step size
        public static int StepY { get; set; }

        private static Point[] point_x;
        private static Point[] point_y;
        private static Pen pen;
        private static Brush brush;
        private static Rectangle rect;
        private static Size clientSize;

        private static bool mouseDown;
        private static Point rectPoint;         // to draw mouse select rectangle
        private static Size rectSize;           // to draw mouse select rectangle
        private static Point selectEndPoint;    // end point with grid(X,Y)
        private static Point selectStartPoint;
        #endregion

        #region EVENT
        public static event EventHandler SizeChanged;
        public delegate void AreaSelectedEvent(Point start, Point end);
        public static event AreaSelectedEvent AreaSelected;
        public static event EventHandler AreaEndSelect;
        #endregion

        #region ATTRIBUTE
        public static void Init(Form frm)
        {
            // public variable init
            X = X_DEFAULT;
            Y = Y_DEFAULT;

            // private variable init
            mouseDown = false;
            rectPoint = new Point();  // to draw mouse select rectangle
            rectSize = new Size();     // to draw mouse select rectangle
            selectEndPoint = new Point();   // end point with grid(X,Y)
            selectStartPoint = new Point();

            point_x = new Point[2];
            point_y = new Point[2];
            pen = new Pen(Color.Black, 1.0f);
            brush = new SolidBrush(Color.FromArgb(128, Color.Silver));
            rect = new Rectangle();
            clientSize = new Size();

            // Form event
            frm.Paint += Frm_Paint;
            frm.MouseDown += Frm_MouseDown;
            frm.MouseUp += Frm_MouseUp;
            frm.MouseMove += Frm_MouseMove;
            frm.ClientSizeChanged += Frm_ClientSizeChanged;

            Update(frm.ClientSize);
        }

        public static bool SetSize(int x, int y)
        {
            // Check with component management
            foreach (ComponentItem item in Component.Items)
            {
                if (x < item.EndPoint.X || y < item.EndPoint.Y)
                {
                    return false;
                }
            }

            // check condition to set grid value
            if (x == X && y == Y || (x < X_DEFAULT) || (y < Y_DEFAULT))
            {
                return false;
            }

            X = x;
            Y = y;

            Update(clientSize);
            return true;
        }

        /// <summary>
        /// use grid reference point to calculate form pixel point of form
        /// </summary>
        /// <param name="startPoint"></param>
        /// <returns></returns>
        public static Point GetPoint(Point startPoint)
        {
            Point startLocation = new Point();

            startLocation.X = startPoint.X * StepX;
            startLocation.Y = startPoint.Y * StepY + OFFSET_TOP;

            if (startPoint.X == 0)
            {
                startLocation.X -= 1;
            }

            return startLocation;
        }

        /// <summary>
        /// Use grid size to calculate size by pixel
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Size GetSize(Point startPoint, Point endPoint)
        {
            Size areaSize = new Size();
            if (endPoint.X == X)
            {
                areaSize.Width = clientSize.Width - startPoint.X * StepX + 1;
            }
            else
            {
                areaSize.Width = (endPoint.X - startPoint.X) * StepX + 1;
                if (startPoint.X == 0)
                {
                    areaSize.Width += 1;
                }
            }

            if (endPoint.Y == Y)
            {
                areaSize.Height = clientSize.Height - OFFSET_BOTTOM - OFFSET_TOP - startPoint.Y * StepY;
            }
            else
            {
                areaSize.Height = StepY * endPoint.Y - startPoint.Y * StepY + 1;
            }

            return areaSize;
        }
        #endregion

        #region FORM_EVENT
        private static void Frm_ClientSizeChanged(object sender, EventArgs e)
        {
            Form frm = (Form)sender;
            Update(frm.ClientSize);
        }

        private static void Frm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.ResetTransform();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            Draw(e.Graphics);
        }

        private static void Frm_MouseDown(object sender, MouseEventArgs e)
        {
            Form frm = (Form)sender;
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;

                int x = e.Location.X / StepX;
                int y = (e.Location.Y - OFFSET_TOP) / StepY;

                rectPoint.X = x * StepX;
                rectPoint.Y = y * StepY + OFFSET_TOP + 1;

                if (x + 1 == X)
                {
                    rectSize.Width = clientSize.Width - x * StepX;
                }
                else
                {
                    rectSize.Width = StepX;
                }

                if (y + 1 == Y)
                {
                    rectSize.Height = clientSize.Height - OFFSET_BOTTOM - OFFSET_TOP - y * StepY;
                }
                else
                {
                    rectSize.Height = StepY;
                }

                rectSize.Height -= 1;

                if (x != 0)
                {
                    rectSize.Width -= 1;
                    rectPoint.X += 1;
                }

                selectEndPoint.X = x + 1;
                selectEndPoint.Y = y + 1;
                selectStartPoint.X = x;
                selectStartPoint.Y = y;

                frm.Invalidate();
            }
        }

        private static void Frm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;

            int x = e.Location.X / StepX + 1;
            int y = (e.Location.Y - OFFSET_TOP) / StepY + 1;

            if (x == selectEndPoint.X && y == selectEndPoint.Y)
            {
                return;
            }

            Form frm = (Form)sender;

            if (x == X)
            {
                rectSize.Width = clientSize.Width - rectPoint.X;
            }
            else
            {
                rectSize.Width = x * StepX - rectPoint.X;
            }

            if (y == Y)
            {
                rectSize.Height = clientSize.Height - OFFSET_BOTTOM - rectPoint.Y - 1;
            }
            else
            {
                rectSize.Height = StepY * y + OFFSET_TOP - rectPoint.Y;
            }

            selectEndPoint.X = x;
            selectEndPoint.Y = y;

            // rectangle collision detect
            foreach (ComponentItem item in Component.Items)
            {
                Rectangle rec1 = new Rectangle();
                rec1.X = selectStartPoint.X;
                rec1.Y = selectStartPoint.Y;
                rec1.Width = selectEndPoint.X - selectStartPoint.X;
                rec1.Height = selectEndPoint.Y - selectStartPoint.Y;

                Rectangle rec2 = new Rectangle();
                rec2.X = item.StartPoint.X;
                rec2.Y = item.StartPoint.Y;
                rec2.Width = item.EndPoint.X - item.StartPoint.X;
                rec2.Height = item.EndPoint.Y - item.StartPoint.Y;

                float disSubX = (rec1.X + (rec1.Width / 2.0f)) - (rec2.X + (rec2.Width / 2.0f));
                if (disSubX < 0) { disSubX = (-1) * disSubX; }

                float disW = (rec1.Width + rec2.Width) / 2.0f;

                float disSubY = (rec1.Y + (rec1.Height / 2.0f)) - (rec2.Y + (rec2.Height / 2.0f));
                if (disSubY < 0) { disSubY = (-1) * disSubY; }

                float disH = (rec1.Height + rec2.Height) / 2.0f;

                if (disSubX < disW && disSubY < disH)
                {
                    Cursor.Current = Cursors.No;
                    return;
                }
                else
                {
                    // Do nothing
                }
            }

            Cursor.Current = Cursors.Default;   // reset cursor
            AreaSelected?.Invoke(selectStartPoint, selectEndPoint);
            frm.Invalidate();
        }

        private static void Frm_MouseUp(object sender, MouseEventArgs e)
        {
            Form frm = (Form)sender;
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = false;
                if (selectStartPoint.X != selectEndPoint.X && selectStartPoint.Y != selectEndPoint.Y && Cursor.Current != Cursors.No)
                {
                    using (Layout.FrmSelectComponent f = new Layout.FrmSelectComponent())
                    {
                        f.StartPosition = FormStartPosition.Manual;
                        f.Top = frm.Top + (frm.Size.Height - f.Size.Height) / 2;
                        f.Left = frm.Left + (frm.Size.Width - f.Size.Width) / 2;

                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            f.SelectComponent.StartPoint = selectStartPoint;
                            f.SelectComponent.EndPoint = selectEndPoint;
                            Core.Component.Add(f.SelectComponent);
                        }
                    }
                }

                AreaEndSelect?.Invoke(null, null);
                frm.Invalidate();
            }
        }
        #endregion

        #region METHOD
        private static void Draw(Graphics g)
        {
            // by pass first line
            for (int i = 1; i < X; i++)
            {
                point_x[0].X = StepX * i;
                point_x[0].Y = OFFSET_TOP;
                point_x[1].X = StepX * i;
                point_x[1].Y = clientSize.Height - OFFSET_BOTTOM - 1;

                g.DrawLines(pen, point_x);
            }

            for (int i = 0; i < Y; i++)
            {
                point_y[0].X = 0;
                point_y[0].Y = i * StepY + OFFSET_TOP;
                point_y[1].X = clientSize.Width;
                point_y[1].Y = i * StepY + OFFSET_TOP;

                g.DrawLines(pen, point_y);
            }

            point_y[0].X = 0;
            point_y[0].Y = clientSize.Height - OFFSET_BOTTOM - 1;
            point_y[1].X = clientSize.Width;
            point_y[1].Y = clientSize.Height - OFFSET_BOTTOM - 1;
            g.DrawLines(pen, point_y);

            if (mouseDown)
            {
                if (selectStartPoint.X == selectEndPoint.X || selectStartPoint.Y == selectEndPoint.Y)
                {
                    return;
                }
                rect.X = rectPoint.X;
                rect.Y = rectPoint.Y;
                rect.Width = rectSize.Width;
                rect.Height = rectSize.Height;
                g.FillRectangle(brush, rect);
            }
        }

        private static void Update(Size size)
        {
            clientSize = size;
            StepX = clientSize.Width / X;
            StepY = (clientSize.Height - OFFSET_BOTTOM - OFFSET_TOP) / Y;

            // TODO grid changed, call client size changed
            SizeChanged?.Invoke(null, null);
        }
        #endregion
    }
}
