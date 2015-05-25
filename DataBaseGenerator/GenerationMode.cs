using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseGenerator
{
    public class GenerationMode
    {
        public String Mode { get; set; } //dictionary, fix, random
        public String fixedValue { get; set; }
        public String DictionaryPath { get; set; }
        public int randomValue { get; set; }
        public String equalTo { get; set; }
        public String startValue { get; set; }
    }
}
