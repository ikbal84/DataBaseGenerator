using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataBaseGenerator
{
    public class DbGenerator
    {
        private StringBuilder SqlScript;

        public DbGenerator()
        {
            this.SqlScript = new StringBuilder();
        }
        
        public void Generate(List<ModelObject> objects, String GeneratedfilePath)
        {
            try
            {
                if (File.Exists(GeneratedfilePath))
                {
                    File.Delete(GeneratedfilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Write("File error");
            }

            foreach (var objectToCreate in objects)
            {
                this.SqlScript.Append(GenerateSingleObject(objectToCreate, 1000));
            }

            WriteToFile(GeneratedfilePath, this.SqlScript.ToString());
        }

        private String GenerateSingleObject(ModelObject ObjectToGenerate, int total)
        {
            String InsertStatement = GenerateHeaderInsertSqript(ObjectToGenerate);
            
            StringBuilder InsertObjectScript = new StringBuilder();


            Dictionary<String, String[]> PropertiesValuesFromFiles = new Dictionary<String, String[]>();

            foreach (var prop in ObjectToGenerate.Properties)
            {
                if (prop.GenerationOption.Mode.Equals("dictionary"))
                {
                    PropertiesValuesFromFiles.Add(prop.Name, GetStringArrayFromFile(prop.GenerationOption.DictionaryPath));
                }
            }

            //
            for (int i = 0; i < total; i++)
            {
                StringBuilder InsertLine = new StringBuilder();
                InsertLine.Append(InsertStatement);

                InsertLine.Append(" values(");


                ModelObject model = new ModelObject();

                bool isFirstProperty = true;
                foreach (var prop in ObjectToGenerate.Properties)
                {
                    if (!isFirstProperty)
                    {
                        InsertLine.Append(",");
                    }

                    isFirstProperty = false;

                   

                    switch (prop.GenerationOption.Mode)
                    {
                        case "increment":
                            model.Properties.Add(
                                             new Property()
                                             {
                                                 Name = prop.Name,
                                                 Type = prop.Type,
                                                 GenerationOption = new GenerationMode()
                                                 {
                                                     startValue = prop.GenerationOption.startValue,
                                                     Mode = prop.GenerationOption.Mode
                                                 },
                                                 GeneratedValue = (Int32.Parse(prop.GenerationOption.startValue) + i).ToString()
                                             }
                                             );
                            break;
                        case "dictionary": 
                            model.Properties.Add(
                                             new Property()
                                             { 
                                                 Name = prop.Name, 
                                                 Type = prop.Type, 
                                                 GenerationOption = new GenerationMode()
                                                 { 
                                                     DictionaryPath = prop.GenerationOption.DictionaryPath,
                                                     Mode = prop.GenerationOption.Mode
                                                 },
                                                 GeneratedValue =  GetRandomFromDico(PropertiesValuesFromFiles[prop.Name])
                                             }
                                             );
                           
                            break;

                        case "fix": 
                            model.Properties.Add(
                                             new Property()
                                             {
                                                 Name = prop.Name,
                                                 Type = prop.Type,
                                                 GenerationOption = new GenerationMode()
                                                 {
                                                     fixedValue = prop.GenerationOption.fixedValue,
                                                     Mode = prop.GenerationOption.Mode
                                                 },
                                                 GeneratedValue = prop.GenerationOption.fixedValue
                                             }
                                                               );
                            break;

                        case "random":
                            Random rand = new Random();
                            model.Properties.Add(
                                              new Property()
                                              {
                                                  Name = prop.Name,
                                                  Type = prop.Type,
                                                  GenerationOption = new GenerationMode()
                                                  {
                                                      randomValue = prop.GenerationOption.randomValue,
                                                      Mode = prop.GenerationOption.Mode
                                                  },
                                                  GeneratedValue = rand.Next(prop.GenerationOption.randomValue).ToString()
                                              }
                                                );
                            
                              break;

                        case "equal":
                              model.Properties.Add(
                                                new Property()
                                                {
                                                    Name = prop.Name,
                                                    Type = prop.Type,
                                                    GenerationOption = new GenerationMode()
                                                    {
                                                        equalTo = prop.GenerationOption.equalTo,
                                                        Mode = prop.GenerationOption.Mode
                                                    },
                                                    GeneratedValue = model.Properties.Find(p => p.Name == prop.GenerationOption.equalTo).GeneratedValue
                                                }
                                                  );

                            break;
                        default:
                            InsertLine.Append("'Error'");
                            break;
                    }

                     InsertLine.Append("'" + model.Properties.Find(p => p.Name == prop.Name).GeneratedValue + "'");
                }

                InsertLine.Append(");\n");

                InsertObjectScript.Append(InsertLine);
            }

            return InsertObjectScript.ToString();

        }

        private String GetRandomFromDico(String [] values)
        {
            Random rand = new Random();

            return values[rand.Next(values.Length)];
        }



        private String[] GetStringArrayFromFile(String DicoFilePath)
        {
            String FileContent = null;

            using (StreamReader fileStream = File.OpenText(DicoFilePath))
            {
                FileContent = fileStream.ReadToEnd();
            }

            if (!String.IsNullOrEmpty(FileContent))
            {
                return FileContent.Split(';');
            }
            else
            {
                return null;
            }

        }

        private String GenerateHeaderInsertSqript(ModelObject ObjectToGenerate)
        {
            StringBuilder LineHeader = new StringBuilder();
            LineHeader.Append("Insert into " + ObjectToGenerate.Name +" ");
            LineHeader.Append("(");
            
            bool isFirstProperty = true;
            foreach (var prop in ObjectToGenerate.Properties)
            {
                if (!isFirstProperty)
                {
                    LineHeader.Append(",");
                }

                LineHeader.Append("'" + prop.Name +"'");
                isFirstProperty = false;
            }

            LineHeader.Append(")");

            return LineHeader.ToString();
        }

        private void WriteToFile(String GeneratedfilePath, String Content)
        {
            try
            {
                using (FileStream fs = File.Create(GeneratedfilePath))
                {
                    // Add some text to file
                    Byte[] Text = new UTF8Encoding(true).GetBytes(Content);
                    fs.Write(Text, 0, Text.Length);

                }
            }
            catch (Exception ex)
            {
                Console.Write("File error");
            }
        }
    }
}
