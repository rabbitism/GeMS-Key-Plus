using GeMS_Key_Plus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeMS_Key_Plus.Global
{
    public static class GlobalVariables
    {
        public static string QueryString { get; set; }
        public static List<string> AllHotKeys { get; } = new List<string>()
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","D1","D2","D3"
            ,"D4","D5","D6","D7","D8","D9","D0",
        };

        public static LRUCache<string, string> ActionHistory { get; set; } = new LRUCache<string, string>(30);
    }
}
