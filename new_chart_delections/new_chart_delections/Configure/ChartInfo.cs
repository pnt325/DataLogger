using Newtonsoft.Json.Linq;
using System.Drawing;

namespace DataLogger.Configure
{
    public class ChartInfo
    {
        public static JObject Dump(Components.ChartInfo info)
        {
            JObject obj = new JObject();
            obj["title"] = info.Title;
            obj["sample"] = info.Sample;

            JArray lines = new JArray();
            foreach (Components.ChartLine line in info.Lines)
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

            foreach (var li in obj["lines"])
            {
                info.Lines.Add(LineInfo.Load((JObject)li));
            }

            return info;
        }
    }

    public class LineInfo
    {
        public static JObject Dump(Components.ChartLine line)
        {
            JObject obj = new JObject();

            obj["name"] = line.Name;
            obj["var_name"] = line.VarName;
            obj["var_adr"] = line.VarAddress;
            obj["var_type"] = Core.MemoryType.ToString(line.VarType);
            obj["color"] = line.Color.ToArgb(); // int

            return obj;
        }

        public static Components.ChartLine Load(JObject obj)
        {
            Components.ChartLine line = new Components.ChartLine();
            line.Name = obj["name"].Value<string>();
            line.VarName = obj["var_name"].Value<string>();
            line.VarAddress = obj["var_adr"].Value<uint>();
            line.VarType = Core.MemoryType.ToType(obj["var_type"].Value<string>());
            line.Color = Color.FromArgb(obj["color"].Value<int>());

            return line;
        }
    }

}
