using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeMS_Key_Plus.Models
{
    public class LinkButton
    {
        public int Id { get; set; }
        public string Hotkey { get; set; }
        public string ButtonName { get; set; }
        public string Category { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool RequireSplit { get; set; }
        public bool IsPrimary { get; set; }
        public string SpecialDelimiters { get; set; }
    }
}
