using Newtonsoft.Json.Linq;

namespace DataLogger.Configure.Ver01
{
    public class LabelInfo
    {
        public static JObject Dump(Components.LabelInfo info)
        {
            JObject obj = new JObject();
            obj["title"] = info.Title;
            obj["var_name"] = info.VarName;
            obj["var_adr"] = info.VarAddress;
            obj["var_type"] = Core.MemoryType.ToString(info.VarType);

            return obj;
        }

        public static Components.LabelInfo Load(JObject obj)
        {
            Components.LabelInfo info = new Components.LabelInfo();
            info.Title = obj["title"].Value<string>();
            info.VarName = obj["var_name"].Value<string>();
            info.VarAddress = obj["var_adr"].Value<uint>();
            info.VarType = Core.MemoryType.ToType(obj["var_type"].Value<string>());

            return info;
        }
    }
}
