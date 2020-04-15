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

            job["title"] = "test";
            job["location"] = new JObject();
            job["location"]["x"] = 123;
            job["location"]["y"] = 456;

            Console.WriteLine(job);

            Console.ReadLine();
        }
    }
}
