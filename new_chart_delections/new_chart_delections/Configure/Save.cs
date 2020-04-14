using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace new_chart_delections.Configure
{
    public class Save
    {
        public static bool Dump(string filename)
        {
            JObject save_config = new JObject();

            // memory;
            save_config["memory"] = Memory.Dump(Core.Memory.VarTypes);

            // grid
            save_config["grid"] = new JObject();
            save_config["grid"]["x"] = Core.Grid.X;
            save_config["grid"]["y"] = Core.Grid.Y;

            // component
            JArray js = new JArray();

            foreach(Core.ComponentItem item in Core.Component.Items)
            {
                js.Add(ComponentItem.Dump(item));
            }
            save_config["components"] = js;

            // connection
            JObject jConnection = new JObject();
            jConnection["uart"] = new JObject();
            jConnection["uart"]["port"] = Program.Uart.PortName;
            jConnection["uart"]["baudrate"] = Program.Uart.BaudRate;

            save_config["connection"] = jConnection;

            using(StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(save_config.ToString());
            }

            return true;
        }
    }
}
