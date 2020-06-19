using Newtonsoft.Json.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace DataLogger.Configure
{
    public class Save
    {
        public static string FileName = "";

        public static void Show()
        {
            //string filename = "";
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = CfgFile.FileFilter;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    FileName = sfd.FileName;
                }
            }

            if (FileName == "")
            {
                return;
            }

            Dump(FileName);
        }


        public static bool Show(string filename)
        {
            if (File.Exists(filename) == false)
            {
                return false;
            }

            return Dump(filename);
        }

        private static bool Dump(string filename)
        {
            JObject jobj_file = new JObject();
            JObject save_config = new JObject();

            // memory;
            save_config["memory"] = Memory.Dump(Core.Memory.Types);

            // grid
            save_config["grid"] = new JObject();
            save_config["grid"]["x"] = Core.Grid.X;
            save_config["grid"]["y"] = Core.Grid.Y;

            // component
            JArray js = new JArray();

            foreach (Core.ComponentItem item in Core.Component.Items)
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

            // Gen datas.
            jobj_file["data"] = save_config;
            using (var md5 = MD5.Create())
            {
                jobj_file["checksum"] = Md5Hash.GetMd5Hash(md5, save_config.ToString());
            }

            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(jobj_file.ToString(Newtonsoft.Json.Formatting.None));
            }

            return true;
        }
    }
}
