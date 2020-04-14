using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace new_chart_delections.Configure
{
    public class ChartInfo
    {
        public static JObject Dump(Components.ChartInfo info)
        {
            JObject obj = new JObject();
            obj["title"] = info.Title;
            obj["sample"] = info.Sample;

            JArray lines = new JArray();
            foreach(Components.Line line in info.Lines)
            {
                lines.Add(LineInfo.Dump(line));
            }
            obj["lines"] = lines;

            return obj;
        }

        public static Components.ChartInfo Load(JObject obj)
        {
            Components.ChartInfo info = new Components.ChartInfo();
            info.Title = obj["title"].ToString();
            info.Sample = obj["sample"].Value<int>();

            foreach(var li in obj["lines"])
            {
                info.Lines.Add(LineInfo.Load((JObject)li));
            }

            return info;
        }
    }

    public class LineInfo
    {
        public static JObject Dump(Components.Line line)
        {
            JObject obj = new JObject();

            obj["name"] = line.Name;
            obj["var_name"] = line.VarName;
            obj["var_adr"] = line.VarAddress;
            obj["var_type"] = Core.MemoryType.ToString(line.VarType);
            obj["color"] = line.Color.ToArgb(); // int

            return obj;
        }

        public static Components.Line Load(JObject obj)
        {
            Components.Line line = new Components.Line();
            line.Name = obj["name"].Value<string>();
            line.VarName = obj["var_name"].Value<string>();
            line.VarAddress = obj["var_adr"].Value<uint>();
            line.VarType = Core.MemoryType.ToType(obj["var_type"].Value<string>());
            line.Color = Color.FromArgb(obj["color"].Value<int>());

            return line;
        }
    }

}
