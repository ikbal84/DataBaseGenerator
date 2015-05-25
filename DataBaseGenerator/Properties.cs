using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseGenerator
{
    public class Property
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public GenerationMode GenerationOption {get; set;}
        public String GeneratedValue { get; set; }
     }

}
