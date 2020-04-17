using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace new_chart_delections.Configure
{
    public class Load
    {
        private static string FileName = "";
        public static void Show()
        {
            FileName = "";  // reset filename
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Grid layout (*.cfg)|*.cfg";
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    FileName = ofd.FileName;
                }
            }

            if(FileName != "")
            {
                FromFile(FileName);
            }
        }
        public static bool FromFile(string filename)
        {
            // load data
            // verify data rule
            // modify new data.
            JObject obj;
            using (StreamReader sr = new StreamReader(filename))
            {
                try
                {
                    obj = JObject.Parse(sr.ReadToEnd());
                }
                catch
                {
                    return false;
                }

            }

            if (obj == null)
            {
                return false;
            }

            /// verify
            var data = obj.GetValue("data");
            if (data == null)
            {
                return false;
            }

            var verify = obj.GetValue("checksum");
            if (verify == null)
            {
                return false;
            }

            using (var md5 = MD5.Create())
            {
                if(Md5Hash.VerifyMd5Hash(md5, data.ToString(), verify.ToString()) == false)
                {
                    return false;
                }
            }



            // load memory
            Memory.Load((JArray)obj["data"]["memory"]);

            // grid
            int x = obj["data"]["grid"]["x"].Value<int>();
            int y = obj["data"]["grid"]["y"].Value<int>();
            Core.Grid.SetSize(x, y);

            // Connection
            Program.Uart.BaudRate = obj["data"]["connection"]["uart"]["baudrate"].Value<int>();

            // Component
            // remove current component
            // need to coppy to new list. prevent acees to object was removed
            foreach(Core.ComponentItem item in Core.Component.Items.ToList())
            {
                Core.Component.Remove(item.Uuid);
            }

            foreach (var item in obj["data"]["components"])
            {
                Core.ComponentItem componentItem = ComponentItem.Load((JObject)item);
                Core.Component.Add(componentItem);

                //Core.Component.AddToForm(componentItem);
            }

            return true;
        }
    }
}
