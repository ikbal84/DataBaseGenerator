using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseGenerator
{
    public class ModelObject
    {
        public string Name { get; set; }
        public List<String> Dependecies { get; set; }
        public List<Property> Properties { get; set; }

        public ModelObject()
        {
             this.Dependecies = new List<string>();
             this.Properties = new List<Property>();
        }
    }
}
