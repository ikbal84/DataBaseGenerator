using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataBaseGenerator
{
    public class XmlParser
    {
        public List<ModelObject> parseModel(String filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("model");

            List<ModelObject> Objects = new List<ModelObject>();

            foreach (XmlNode node in nodes)
            {
                ModelObject ObjectModel = new ModelObject();

                ObjectModel.Name = node.SelectSingleNode("name").InnerText.Trim();
                XmlNodeList dependencies = node.SelectNodes("dependency");

                foreach (XmlNode dependency in dependencies)
                {
                    if (!String.IsNullOrEmpty(dependency.InnerText.Trim()))
                    {
                        ObjectModel.Dependecies.Add(dependency.InnerText.Trim());
                    }
                    
                }

                //getting properties
                XmlNodeList Props = node.SelectSingleNode("properties").SelectNodes("property");
                foreach (XmlNode property in Props)
                {
                    Property prop = new Property();

                    prop.Name = property.ChildNodes[0].InnerText.Trim();
                    prop.Type = property.ChildNodes[1].InnerText.Trim();

                    GenerationMode GenOption = new GenerationMode();
                    GenOption.Mode = property.ChildNodes[2].InnerText.Trim();

                    switch (GenOption.Mode)
                    {
                        case "increment": GenOption.startValue = property.ChildNodes[3].InnerText.Trim();
                            break;

                        case "dictionary": GenOption.DictionaryPath = property.ChildNodes[3].InnerText.Trim();
                            break;

                        case "fix": GenOption.fixedValue = property.ChildNodes[3].InnerText.Trim();
                            break;

                        case "random":
                            int randomMax = 0;
                            if (Int32.TryParse(property.ChildNodes[3].InnerText.Trim(), out randomMax))
                            {
                                GenOption.randomValue = randomMax;
                            }
                            else {
                                GenOption.randomValue = 0;
                            }
                            
                            break;
                        case "equal":
                            GenOption.equalTo = property.ChildNodes[3].InnerText.Trim();
                            break;

                        default: GenOption.Mode = "fix";
                                 GenOption.fixedValue = "";
                             break;
                    }

                    prop.GenerationOption = GenOption;
                    ObjectModel.Properties.Add(prop);
                }
                //
                Objects.Add(ObjectModel);
            }

            return Objects;

        }
    }
}
