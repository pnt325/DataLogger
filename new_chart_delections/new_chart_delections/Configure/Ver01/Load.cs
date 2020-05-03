using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace DataLogger.Configure.Ver01
{
    public class Load
    {
        private static string FileName = "";
        public static void Show()
        {
            FileName = "";  // reset filename
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = CfgFile.FileFilter;
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

            // verify
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

            // Version
            int version = obj["ver"].Value<int>();
            if (version != CfgFile.Version)
            {
                return false;
            }
            else
            {
                if (version < CfgFile.Version)
                {

                }
                else if (version > CfgFile.Version)
                {

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
