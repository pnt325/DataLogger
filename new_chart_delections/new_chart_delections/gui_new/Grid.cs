using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace new_chart_delections.gui_new
{
    public class Grid
    {
        #region PROPERTY
        public const int OFFSET_TOP = 24;       // px
        public const int OFFSET_BOTTOM = 22;    // px
        public const int X_DEFAULT = 4;
        public const int Y_DEFAULT = 4;

        private const int DEFAULT_GRID_SIZE = 4;

        public int X { get; set; } = X_DEFAULT; // default value
        public int Y { get; set; } = Y_DEFAULT;
        public int StepX { get; set; }
        public int StepY { get; set; }

        public event EventHandler GridChanged;
        public event EventHandler Invalidate;

        public delegate void AreaSelectedDelegate(Point startPoint, Point endPoint);
        public AreaSelectedDelegate AreaSelected;

        #endregion

        #region PRIVATE
        private Point[] pointx = new Point[2];
        private Point[] pointy = new Point[2];

        private Pen pen = new Pen(Color.Black, 1.0f);
        private Brush brush = new SolidBrush(Color.FromArgb(128, Color.Silver));    // color to fill rectangle
        private Rectangle rect = new Rectangle();
        private Size clientSize = new Size();

        public bool SetGrid(int x, int y)
        { 
            // Check with component management
            foreach(ComponentArea area in Program.ComponentManage.AreaItems)
            {
               if(x < area.EndPoint.X || y < area.EndPoint.Y)
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

            UpdateGrid(clientSize);
            return true;
        }

        private void UpdateGrid(Size size)
        {
            clientSize = size;
            StepX = clientSize.Width / X;
            StepY = (clientSize.Height - OFFSET_BOTTOM - OFFSET_TOP) / Y;
            GridChanged?.Invoke(null, null);
        }

        private void DrawGrid(Graphics g)
        {
            // by pass first line
            for (int i = 1; i < X; i++)
            {
                pointx[0].X = StepX * i;
                pointx[0].Y = OFFSET_TOP;
                pointx[1].X = StepX * i;
                pointx[1].Y = clientSize.Height - OFFSET_BOTTOM - 1;

                g.DrawLines(pen, pointx);
            }

            for (int i = 0; i < Y; i++)
            {
                pointy[0].X = 0;
                pointy[0].Y = i * StepY + OFFSET_TOP;
                pointy[1].X = clientSize.Width;
                pointy[1].Y = i * StepY + OFFSET_TOP;

                g.DrawLines(pen, pointy);
            }

            pointy[0].X = 0;
            pointy[0].Y = clientSize.Height - OFFSET_BOTTOM - 1;
            pointy[1].X = clientSize.Width;
            pointy[1].Y = clientSize.Height - OFFSET_BOTTOM - 1;
            g.DrawLines(pen, pointy);

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
        #endregion

        #region PUBLIC
        public Grid(Form frm)
        {
            frm.Paint += Control_Paint;
            frm.MouseDown += Control_MouseDown;
            frm.MouseUp += Control_MouseUp;
            frm.MouseMove += Control_MouseMove;
            frm.ClientSizeChanged += Control_ClientSizeChanged;

            UpdateGrid(frm.ClientSize);
        }

        public void ChangeGrid(int x, int y)
        {
            if ((x < DEFAULT_GRID_SIZE) || (y < DEFAULT_GRID_SIZE) || (x == X && y == Y))
                return;

            // check for component exist.

            // 
            UpdateGrid(clientSize); // because size not change reuse last size


            Invalidate?.Invoke(null, null); // notify to form that grid change need to redraw grid.
        }

        private void Control_ClientSizeChanged(object sender, EventArgs e)
        {
            //GridChanged?.Invoke(null, null);
            Form frm = (Form)sender;
            UpdateGrid(frm.ClientSize);
        }


        public Point GetLocation(Point startPoint)
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

        public Size GetSize(Point startPoint, Point endPoint)
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

        #region CONTROL EVENT
        private bool mouseDown = false;
        private Point rectPoint = new Point();  // to draw mouse select rectangle
        private Size rectSize = new Size();     // to draw mouse select rectangle
        private Point selectEndPoint = new Point();   // end point with grid(X,Y)
        private Point selectStartPoint = new Point();
        private void Control_MouseMove(object sender, MouseEventArgs e)
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
            foreach (ComponentArea component in Program.ComponentManage.AreaItems)
            {
                Rectangle rec1 = new Rectangle();
                rec1.X = selectStartPoint.X;
                rec1.Y = selectStartPoint.Y;
                rec1.Width = selectEndPoint.X - selectStartPoint.X;
                rec1.Height = selectEndPoint.Y - selectStartPoint.Y;

                Rectangle rec2 = new Rectangle();
                rec2.X = component.StartPoint.X;
                rec2.Y = component.StartPoint.Y;
                rec2.Width = component.EndPoint.X - component.StartPoint.X;
                rec2.Height = component.EndPoint.Y - component.StartPoint.Y;

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

            Cursor.Current = Cursors.Default;

            frm.Invalidate();
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            Form frm = (Form)sender;
            if (e.Button == MouseButtons.Left)
            {
                // reset mouse-down.
                mouseDown = false;

                if (selectStartPoint.X != selectEndPoint.X && selectStartPoint.Y != selectEndPoint.Y && Cursor.Current != Cursors.No)
                {
                    AreaSelected?.Invoke(selectStartPoint, selectEndPoint);
                }
                frm.Invalidate();
            }
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
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

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.ResetTransform();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            DrawGrid(e.Graphics);
        }
        #endregion
    }
}
