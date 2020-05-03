using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DataLogger.Configure.Ver01
{
    public class TableInfo
    {
        public static JArray Dump(List<Components.TableInfo> tableInfos)
        {
            JArray jArray = new JArray();
            foreach (Components.TableInfo tableInfo in tableInfos)
            {
                JObject jObject = new JObject();
                jObject["var_name"] = tableInfo.VarName;
                jObject["var_adr"] = tableInfo.VarAddress;
                jObject["var_type"] = Core.MemoryType.ToString(tableInfo.VarType);

                jArray.Add(jObject);
            }

            return jArray;
        }

        public static List<Components.TableInfo> Load(JArray array)
        {
            List<Components.TableInfo> tableInfos = new List<Components.TableInfo>();
            foreach (var obj in array)
            {
                Components.TableInfo table = new Components.TableInfo();
                table.Name = obj["var_name"].Value<string>();
                table.VarAddress = obj["var_adr"].Value<uint>();
                table.VarType = Core.MemoryType.ToType(obj["var_type"].Value<string>());

                tableInfos.Add(table);
            }

            return tableInfos;
        }
    }
}
