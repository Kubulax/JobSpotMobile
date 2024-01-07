using System;
using System.Collections.Generic;
using System.Text;

namespace JobSpot.Classes
{
    public class FilterParam
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }

        public FilterParam(string columnName, string value)
        {
            ColumnName = columnName;
            Value = value;
        }
    }
}
