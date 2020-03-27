using System;
using System.Collections.Generic;
using System.Drawing;

namespace new_chart_delections.gui_new
{
    public class ComponentType
    {
        const string strLineChart = "Line Chart";
        public enum Type
        {
            None,
            LineChart,
        }

        public static Type GetType(string strType)
        {
            Type vType = Type.None;
            switch (strType)
            {
                case strLineChart:
                    vType = Type.LineChart;
                    break;
                default:
                    break;
            }
            return vType;
        }

        public static string[] GetNames()
        {
            return new string[] { strLineChart };
        }

        public static string ToString(Type vType)
        {
            if (Type.None == vType)
                return "";
            string strType = "";
            switch (vType)
            {
                case gui_new.ComponentType.Type.None:
                    break;
                case gui_new.ComponentType.Type.LineChart:
                    strType = strLineChart;
                    break;
                default:
                    break;
            }
            return strType;
        }
    }

    public class ComponentManage
    {
        private readonly object lock_object = new object();

        public event EventHandler StartMonitor;
        public event EventHandler StopMonitor;

        public List<ComponentArea> areaItems { get; set; }

        public ComponentManage()
        {
            areaItems = new List<ComponentArea>();
        }


        public bool RemoveAreaItem(string uuid)
        {
            lock(lock_object)
            {
                int index = this.areaItems.IndexOf(new ComponentArea() { UUID = uuid });
                if (index >= 0)
                {
                    areaItems.RemoveAt(index);
                    return true;
                }
                return false;
            }
        }

        public void AddAreadItem(ComponentArea componentArea)
        {
            lock(lock_object)
            {
                areaItems.Add(componentArea);
            }
        }

        /// <summary>
        /// Start all component exist on grid.
        /// </summary>
        public void StartAll()
        {
            StartMonitor?.Invoke(null, null);
        }


        /// <summary>
        /// Stop all component exist on grid.
        /// </summary>
        public void StopAll()
        {
            StopMonitor?.Invoke(null, null);
        }
    }

    public class ComponentArea : IEquatable<ComponentArea>
    {
        public string UUID { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ComponentArea componentItem = obj as ComponentArea;
            if (componentItem == null) return false;
            else return Equals(obj);
        }

        public bool Equals(ComponentArea componentItem)
        {
            if (componentItem == null) return false;
            return (this.UUID.Equals(componentItem.UUID));
        }
    }
}
