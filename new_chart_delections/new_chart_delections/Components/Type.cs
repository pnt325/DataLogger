namespace DataLogger.Components
{
    public class Type
    {
        const string STR_CHART = "Chart";
        const string STR_LABEL = "Label";
        const string STR_TABLE = "Table";

        public static string[] Names()
        {
            return new string[] { STR_CHART, STR_LABEL, STR_TABLE };
        }

        public static ComponentTypes ToType(string strType)
        {
            ComponentTypes type = ComponentTypes.None;
            switch (strType)
            {
                case STR_CHART:
                    type = ComponentTypes.Chart;
                    break;
                case STR_LABEL:
                    type = ComponentTypes.Label;
                    break;
                case STR_TABLE:
                    type = ComponentTypes.Table;
                    break;
                default:
                    break;
            }

            return type;
        }

        public static string ToString(ComponentTypes type)
        {
            string strType = "";
            switch (type)
            {
                case ComponentTypes.None:
                    strType = "None";
                    break;
                case ComponentTypes.Chart:
                    strType = STR_CHART;
                    break;
                case ComponentTypes.Label:
                    strType = STR_LABEL;
                    break;
                case ComponentTypes.Table:
                    strType = STR_TABLE;
                    break;
                default:
                    break;
            }

            return strType;
        }
    }

    public enum ComponentTypes
    {
        None,
        Chart,
        Label,
        Table
    }
}
