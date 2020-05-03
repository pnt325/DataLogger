using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DataLogger.Core
{
    // static method
    public static class Component
    {
        #region EVENT
        public delegate void TriggerEvent(string uuid);
        public delegate void AddControlEvent(Control control);

        public static event TriggerEvent Start;
        public static event TriggerEvent Stop;
        public static event TriggerEvent Removed;
        public static event AddControlEvent AddControl;
        #endregion

        public static List<ComponentItem> Items { get; set; } = new List<ComponentItem>();

        /// <summary>
        /// Start comonent log data
        /// </summary>
        /// <param name="uuid"></param>
        public static void SetStart(string uuid)
        {
            Start?.Invoke(uuid);
        }

        public static void SetStatus(string uuid, ComponentStaus status)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Uuid == uuid)
                {
                    Items[i].Status = status;
                }
            }
        }

        public static ComponentStaus GetStatus(string uuid)
        {
            ComponentStaus status = ComponentStaus.Stoped;
            for (int i = 0; i < Items.Count; i++)
            {
                if(Items[i].Uuid == uuid)
                {
                    status = Items[i].Status;
                }
            }

            return status;
        }

        /// <summary>
        /// Stop component log data
        /// </summary>
        /// <param name="uuid"></param>
        public static void SetStop(string uuid)
        {
            Stop?.Invoke(uuid);
        }

        /// <summary>
        /// Remove component item
        /// </summary>
        /// <param name="uuid"></param>
        public static void Remove(string uuid)
        {
            foreach (ComponentItem item in Items)
            {
                if (uuid == item.Uuid)
                {
                    Items.Remove(item);
                    Removed?.Invoke(uuid);
                    break;
                }
            }
        }

        /// <summary>
        /// Add new componenet item
        /// </summary>
        /// <param name="item"></param>
        public static void Add(ComponentItem item)
        {
            switch (item.Type)
            {
                case Components.ComponentTypes.None:
                    break;
                case Components.ComponentTypes.Chart:
                    Components.Chart chart = new Components.Chart(item);
                    AddControl?.Invoke(chart);
                    Items.Add(item);
                    break;
                case Components.ComponentTypes.Label:
                    Components.Label label = new Components.Label(item);
                    AddControl?.Invoke(label);
                    Items.Add(item);
                    break;
                case Components.ComponentTypes.Table:
                    Components.Table table = new Components.Table(item);
                    AddControl?.Invoke(table);
                    Items.Add(item);
                    break;
                default:
                    break;
            }
        }
    }

    public class ComponentItem
    {
        public string Uuid { get; set; }
        public string Title { get; set; }
        public ComponentStaus Status { get; set; }  // working status
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Components.ComponentTypes Type { get; set; }
        public int UpdatePeriod { get; set; }     // update sample as hz
        public object Info { get; set; }    // contain component info

        public ComponentItem()
        {
            Status = new ComponentStaus();
            StartPoint = new Point();
            EndPoint = new Point();
            Type = new Components.ComponentTypes();
            Info = new object();
        }
    }

    public enum ComponentStaus
    {
        Stoped,
        Running
    }
}
