using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace xml_file_load
{
    class Program
    {
        static void Main(string[] args)
        {
            string file_path = @"D:\works\project\log_data_from_ecu\version2_053.xml";

            XElement xelement = XElement.Load(file_path);
            var homePhone = from hp in xelement.Elements("frameData")
                            from i in hp.Elements("variables")
                            from x in i.Elements("paramlist")
                            select x;
            int count = 0;
            List<string> parlist = new List<string>();
            foreach(var hom in homePhone)
            {
                Console.WriteLine("<==={0}===>",hom.Attribute("name").Value);
                parlist.Add(hom.Attribute("name").Value.ToString());    // get name of paramlist

                // get list in side paramlist
                var lists = from list in hom.Elements("list")
                            select list;
                foreach(var list in lists)
                {
                    Console.WriteLine(list.Attribute("name").Value);
                }
            }

            Console.WriteLine("Total of value: {0}", count);

            Console.ReadLine();
        }
    }
}
