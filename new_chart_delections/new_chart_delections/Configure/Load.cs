using Newtonsoft.Json.Linq;
using System.IO;

namespace new_chart_delections.Configure
{
    public class Load
    {
        public static bool FromFile(string filename)
        {
            // load data
            // verify data rule
            // modify new data.
            JObject obj;
            using (StreamReader sr = new StreamReader(filename))
            {
                obj = JObject.Parse(sr.ReadToEnd());
            }

            if (obj == null)
            {
                return false;
            }

            // load memory
            Memory.Load((JArray)obj["memory"]);

            // grid
            int x = obj["grid"]["x"].Value<int>();
            int y = obj["grid"]["y"].Value<int>();
            Core.Grid.SetSize(x, y);

            // Connection
            Program.Uart.BaudRate = obj["connection"]["uart"]["baudrate"].Value<int>();

            // Component
            // remove current component
            foreach(Core.ComponentItem item in Core.Component.Items)
            {
                Core.Component.Remove(item.Uuid);
            }

            foreach (var item in obj["components"])
            {
                Core.ComponentItem componentItem = ComponentItem.Load((JObject)item);
                Core.Component.Add(componentItem);

                //Core.Component.AddToForm(componentItem);
            }

            return true;
        }
    }
}
