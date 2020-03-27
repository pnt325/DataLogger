using System.Drawing;

namespace new_chart_delections.GridLine
{
    public class Grid
    {
        public int X { get; set; }  // the count of grid line in workspace
        public int Y { get; set; }
        public float XStep { get; set; }    // the step distance
        public float YStep { get; set; }    // the step distance

        const int startHeigh = 24;  // offset the start heigh

        Pen pen = new Pen(Color.Black, 1.0f);
        Brush brush = new SolidBrush(Color.FromName("info"));

        /// <summary>
        /// Change grid
        /// </summary>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <param name="clientSize"></param>
        public void SetGrid(int gridX, int gridY, Size clientSize)
        {
            X = gridX;
            Y = gridY;
        }

        public void Draw(Graphics g, Size size)
        {
            g.Clear(Color.White);
            g.ResetTransform();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            // calculate step size.
            XStep = size.Width / X;
            YStep = (size.Height - startHeigh) / Y;

            // Draw x axis
            for (int i = 1; i < X; i++)
            {
                PointF[] points =
                {
                    new PointF(XStep*i, startHeigh),
                    new PointF(XStep*i, size.Height)
                };
                g.DrawLines(pen, points);
            }

            // Draw y axis
            for (int i = 0; i < Y; i++)
            {
                PointF[] points =
                {
                    new PointF(0, YStep*i + startHeigh),
                    new PointF(size.Width, YStep*i + startHeigh)
                };
                g.DrawLines(pen, points);
            }
        }
    }

}
