using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string file_data = "(I believe that actually the MD5 implementation used doesn't need to be disposed, but I'd probably still do so anyway";

            using(var md5 = MD5.Create())
            {
                byte[] has = md5.ComputeHash(Encoding.UTF8.GetBytes(file_data));
                Console.WriteLine(has.Length);
                Console.WriteLine(Encoding.Default.GetString(has));
            }

            Console.ReadLine();
        }
    }
}
