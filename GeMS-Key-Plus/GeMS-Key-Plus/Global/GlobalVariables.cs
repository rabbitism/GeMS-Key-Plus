using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeMS_Key_Plus.Global
{
    public static class GlobalVariables
    {
        public static string QueryString { get; set; }
        public static List<string> AllHotKeys { get; } = new List<string>()
        {
            "A","B","C","D","E","F","G","H",
        };
    }
}
