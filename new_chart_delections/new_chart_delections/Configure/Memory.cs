﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace new_chart_delections.Configure
{
    public class Memory
    {
        public static JArray Dump(Dictionary<string, Core.MemoryTypes> VarType)
        {
            JArray js = new JArray();
            foreach (KeyValuePair<string, Core.MemoryTypes> keypair in VarType)
            {
                JObject obj = new JObject();
                obj["name"] = keypair.Key;
                obj["type"] = Core.MemoryType.ToString(keypair.Value);
                js.Add(obj);
            }

            return js;
        }

        public static void Load(JArray jArray)
        {
            string name = "";
            Core.MemoryTypes type;
            foreach(var j in jArray)
            {
                name = j["name"].Value<string>();
                type = Core.MemoryType.ToType(j["type"].Value<string>());

                uint adr = 0;
                Core.Memory.Add(name, type, out adr);
            }
        }
    }
}
