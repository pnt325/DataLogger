using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLogger.Components
{
    public class Table : Layout.ListViewNF
    {
        private TableInfo info;
        private List<TableInfo> items;
        private Point startPoint;
        private Point endPoint;
        private int time_delay;
        private string UUID { get; set; }

        public Table(Core.ComponentItem item)
        {
            // Create table columns
            this.Columns.Add("Name");
            this.Columns.Add("Value");
            this.Columns.Add("Unit");

            // get info
            startPoint = item.StartPoint;
            endPoint = item.EndPoint;
            items = (List<TableInfo>)item.Info;
            UUID = item.Uuid;

            time_delay = 1000 / item.UpdatePeriod;  // ms


            foreach(TableInfo i in items)
            {
                ListViewItem listViewItem = new ListViewItem(i.Name);
                listViewItem.SubItems.Add("0.0");
                listViewItem.SubItems.Add(i.Unit);

                this.Items.Add(listViewItem);
            }
        }


        #region PRIVATE FUNCTION
        private void InitEvent()
        {
            Core.Grid.SizeChanged += Grid_SizeChanged;
            Core.Component.Start += Component_Start;
            Core.Component.Stop += Component_Stop;
            Core.Component.Removed += Component_Removed;
            this.MouseDoubleClick += Table_MouseDoubleClick;
        }

        private void Component_Removed(string uuid)
        {
            if(uuid == UUID)
            {
                Stop();
                DeInitEvent();
                this.Dispose();
            }
        }

        private void Table_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Core.Component.Remove(UUID);
            }
        }

        private void Component_Stop(string uuid)
        {
            if(UUID == uuid)
            {
                Start();
            }
        }

        private void Component_Start(string uuid)
        {
            if(uuid == UUID)
            {
                Stop();
            }
        }

        private void DeInitEvent()
        {
            Core.Grid.SizeChanged -= Grid_SizeChanged;
            Core.Component.Start -= Component_Start;
            Core.Component.Stop -= Component_Stop;
            Core.Component.Removed -= Component_Removed;
            this.MouseDoubleClick -= Table_MouseDoubleClick;
        }

        private void Grid_SizeChanged(object sender, EventArgs e)
        {
            this.Location = Core.Grid.GetPoint(startPoint);
            this.Size = Core.Grid.GetSize(startPoint, endPoint);

            // scale column size.
            // [name][value][uint]
            // [50% ][25%  ][25% ]     
            this.Columns[0].Width = (int)((0.5) * this.Width);
            this.Columns[1].Width = (int)((0.25) * this.Width);
            this.Columns[2].Width = (int)((0.25) * this.Width);

        }

        private bool isStart = false;
        private void Start()
        {
            if(isStart)
            {
                return;
            }

            // re-map address
            for (int i = 0; i < items.Count; i++)
            {
                uint value;
                if(Core.Memory.Address.TryGetValue(items[i].VarName, out value))
                {
                    items[i].VarAddress = value;
                    items[i].VarType = Core.Memory.Types[items[i].VarName];
                }
                else
                {
                    return;
                }
            }

            isStart = true;
            Core.Component.SetStatus(UUID, Core.ComponentStaus.Running);

            Thread th = new Thread(() =>
            {
                object value = 0;
                while(isStart)
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        value = Core.Memory.Read(items[i].VarAddress, items[i].VarType);

                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Items[i].SubItems[1].Text = value.ToString();
                        });
                    }

                    Thread.Sleep(time_delay);
                }
                Core.Component.SetStatus(UUID, Core.ComponentStaus.Stoped);
            });
            th.IsBackground = true;
            th.Start();
        }

        private void Stop()
        {
            isStart = false;
        }
        #endregion

    }


    public class TableInfo
    {
        public string Name { get; set; }
        public string VarName { get; set; }
        public uint VarAddress { get; set; }
        public Core.MemoryTypes VarType { get; set; }
        public string Unit { get; set; }

    }
}
