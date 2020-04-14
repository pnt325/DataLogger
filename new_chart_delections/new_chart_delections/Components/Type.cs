namespace new_chart_delections.Components
{
    public class Type
    {
        const string STR_CHART = "Chart";
        const string STR_LABEL = "Label";
        const string STR_TABLE = "Table";

        public static string[] GetNames()
        {
            return new string[] { STR_CHART, STR_LABEL, STR_TABLE };
        }

        public static Types ToType(string strType)
        {
            Types type = Types.None;
            switch (strType)
            {
                case STR_CHART:
                    type = Types.Chart;
                    break;
                case STR_LABEL:
                    type = Types.Label;
                    break;
                case STR_TABLE:
                    type = Types.Table;
                    break;
                default:
                    break;
            }

            return type;
        }

        public static string ToString(Types type)
        {
            string strType = "";
            switch (type)
            {
                case Types.None:
                    strType = "None";
                    break;
                case Types.Chart:
                    strType = STR_CHART;
                    break;
                case Types.Label:
                    strType = STR_LABEL;
                    break;
                case Types.Table:
                    strType = STR_TABLE;
                    break;
                default:
                    break;
            }

            return strType;
        }
    }

    public enum Types
    {
        None,
        Chart,
        Label,
        Table
    }
}
