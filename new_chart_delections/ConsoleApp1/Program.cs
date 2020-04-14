using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            JObject job = new JObject();

            job["color"] = Color.IndianRed.ToArgb();

            Color s = Color.FromArgb(job["color"].Value<int>());

            Console.WriteLine(job);

            Console.WriteLine(Guid.NewGuid());
            Console.WriteLine(Guid.NewGuid());

            Console.ReadLine();
        }
    }
}
