using System.Windows.Forms;

namespace new_chart_delections.gui
{
    class CtrlDoubleBuffered
    {
        public static void Set(Control control, bool value)
        {
            System.Reflection.PropertyInfo controlProperty = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
        }
    }
}
