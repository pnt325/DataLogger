using Newtonsoft.Json.Linq;
using System;

namespace new_chart_delections.Configure
{
    public class ComponentItem
    {
        public static JObject Dump(Core.ComponentItem item)
        {
            JObject obj = new JObject();

            JObject pstart = new JObject();
            pstart["x"] = item.StartPoint.X;
            pstart["y"] = item.StartPoint.Y;
            JObject pend = new JObject();
            pend["x"] = item.EndPoint.X;
            pend["y"] = item.EndPoint.Y;

            obj["start_point"] = pstart;
            obj["end_point"] = pend;
            obj["type"] = Components.Type.ToString(item.Type);
            obj["period"] = item.UpdatePeriod;

            switch (item.Type)
            {
                case Components.ComponentTypes.None:
                    break;
                case Components.ComponentTypes.Chart:
                    obj["info"] = ChartInfo.Dump((Components.ChartInfo)item.Info);
                    break;
                case Components.ComponentTypes.Label:
                    obj["info"] = LabelInfo.Dump((Components.LabelInfo)item.Info);
                    break;
                case Components.ComponentTypes.Table:
                    break;
                default:
                    break;
            }
            return obj;
        }

        public static Core.ComponentItem Load(JObject obj)
        {
            Core.ComponentItem item = new Core.ComponentItem();
            item.Uuid = Guid.NewGuid().ToString();
            item.Status = Core.ComponentStaus.Stoped;
            item.StartPoint = new System.Drawing.Point(obj["start_point"]["x"].Value<int>(),
                obj["start_point"]["y"].Value<int>());
            item.EndPoint = new System.Drawing.Point(obj["end_point"]["x"].Value<int>(),
                obj["end_point"]["y"].Value<int>());

            item.Type = Components.Type.ToType(obj["type"].Value<string>());
            item.UpdatePeriod = obj["period"].Value<int>();

            switch (item.Type)
            {
                case Components.ComponentTypes.None:
                    break;
                case Components.ComponentTypes.Chart:
                    item.Info = ChartInfo.Load((JObject)obj["info"]);
                    break;
                case Components.ComponentTypes.Label:
                    item.Info = LabelInfo.Load((JObject)obj["info"]);
                    break;
                case Components.ComponentTypes.Table:
                    break;
                default:
                    break;
            }

            return item;
        }
    }
}
