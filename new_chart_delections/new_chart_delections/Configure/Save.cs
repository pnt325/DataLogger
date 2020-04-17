using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace new_chart_delections.Configure
{
    public class Save
    {
        //public static string GetMd5Hash(MD5 md5Hash,string input)
        //{
        //    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        //    // Create a new Stringbuilder to collect the bytes
        //    // and create a string.
        //    StringBuilder sBuilder = new StringBuilder();

        //    // Loop through each byte of the hashed data 
        //    // and format each one as a hexadecimal string.
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sBuilder.Append(data[i].ToString("x2"));
        //    }

        //    // Return the hexadecimal string.
        //    return sBuilder.ToString();
        //}

        //public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        //{
        //    // Hash the input.
        //    string hashOfInput = GetMd5Hash(md5Hash, input);

        //    // Create a StringComparer an compare the hashes.
        //    StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        //    if (0 == comparer.Compare(hashOfInput, hash))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        public static bool Dump(string filename)
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

            // create
            jobj_file["data"] = save_config;
            using (var md5 = MD5.Create())
            {
                jobj_file["checksum"] = Md5Hash.GetMd5Hash(md5, save_config.ToString());
            }

            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(jobj_file.ToString());
            }

            return true;
        }
    }
}
