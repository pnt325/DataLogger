using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogging
{
    public class ConfigureValue
    {
        // Name
        public string Name { get; set; }    // namr of chanel
        public string Type { get; set; }    // Type of value
        public string Unit { get; set; }    // Unit of value
        public string[] Paramlist { get; set; }

        // Value
        public int Value { get; set; }  // Value of signal
        public int Min { get; set; }    // Minimun value
        public int Max { get; set; }    // Maximum value
        public int Tick { get; set; }   // Time tick of value
    }

    public class Paramlist
    {
        public string Name { get; set; }
        public string[] Value { get; set; }
    }
}
