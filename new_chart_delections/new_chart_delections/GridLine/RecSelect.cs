using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace new_chart_delections.GridLine
{
    public class RecSelect
    {
        PointF mouseDown, mouseMove, mouseMoveOld;    // point to draw rectangle
        Brush brush = new SolidBrush(Color.FromArgb(128, Color.Silver));    // color to fill rectangle

        public bool IsMouseDown { get; set; }

        public int xStart, yStart;
        public int xCount, yCount;

        public RecSelect()
        {
            mouseMoveOld = new PointF(0, 0);
        }

        /// <summary>
        /// Draw select rectangle
        /// </summary>
        public void Draw(Graphics g)
        {
            if (IsMouseDown)
            {
                g.FillRectangle(brush, mouseDown.X, mouseDown.Y, (mouseMove.X - mouseDown.X),
                    (mouseMove.Y - mouseDown.Y));
            }
        }

        /// <summary>
        /// Detect to get start point rectangle
        /// Only call in mouse down
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="e"></param>
        public void GetStartPoint(Grid grid, Point e)
        {
            float x = 0, y = 0;
            for (int i = 0; i < grid.X; i++)
            {
                if (e.X >= (i * grid.XStep) && e.X < (i + 1) * grid.XStep)
                {
                    x = (i) * grid.XStep;
                    xStart = i;
                    break;
                }
            }
            for (int i = 0; i < grid.Y; i++)
            {
                if (e.Y >= (i * grid.YStep + 24) && e.Y < (((i + 1) * grid.YStep) + 24))
                {
                    y = (i) * grid.YStep + 24;
                    yStart = i;
                    break;
                }
            }

            if(x == 0)
            {
                mouseDown = new PointF(x, y + 1);
            }
            else
            {
                mouseDown = new PointF(x + 1, y + 1);
            }
        }

        /// <summary>
        /// Detect rectangle end point, call on mousedown and mousemove
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="e"></param>
        /// <param name="size"></param>
        public void GetEndPoint(Grid grid, Point e, Size size)
        {
            float x = 0, y = 0;
            for (int i = 0; i < grid.X; i++)
            {
                if (e.X >= (i * grid.XStep) && e.X < (i + 1) * grid.XStep)
                {
                    if (i == grid.X - 1)
                    {
                        x = size.Width;
                    }
                    else
                    {
                        x = (i + 1) * grid.XStep;
                    }

                    xCount = i - xStart + 1;

                    break;
                }
            }
            for (int i = 0; i < grid.Y; i++)
            {
                if (e.Y >= (i * grid.YStep + 24) && e.Y < (((i + 1) * grid.YStep) + 24))
                {
                    if (i == grid.Y - 1)
                    {
                        y = size.Height;
                    }
                    else
                    {
                        y = (i + 1) * grid.YStep + 24;
                    }

                    yCount = i - yStart + 1;
                    break;
                }
            }

            mouseMove = new PointF(x, y);
        }

        public bool IsChanged()
        {
            if(mouseMove.X != mouseMoveOld.X || mouseMove.Y != mouseMoveOld.Y)
            {
                mouseMoveOld = mouseMove;
                return true;
            }
            return false;
        }
    }
}
