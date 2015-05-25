using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBaseGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String xmlModelFilePath;
        private List<ModelObject> Objects;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadXMLDataBaseSchema_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Text documents (.xml)|*.xml";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                this.xmlModelFilePath = dlg.FileName;
                XmlFilePath.Text = this.xmlModelFilePath;
            }
            
        }

        private void AddObjectPannel(String ObjectName)
        {

                var window = this;
                var stackPanel = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 298,
                    Width = 279,
                    VerticalAlignment = VerticalAlignment.Top,

                };

                stackPanel.Children.Add(new Label { Content = ObjectName });
                stackPanel.Children.Add(new Label { Content = "Path" });
                stackPanel.Children.Add(new TextBox { Text = "", Name = ObjectName + "_Path" });
                stackPanel.Children.Add(new Button { Content = "Load", Name = ObjectName });
                window.Content = stackPanel;

  
        }

        private void GenerateObjectsDescription_Click(object sender, RoutedEventArgs e)
        {
            //read the XML File

            XmlParser Parser = new XmlParser();

            List<ModelObject> Objects = Parser.parseModel(this.xmlModelFilePath);

            DbGenerator Generator = new DbGenerator();

            Generator.Generate(Objects, @"C:\Users\Moh\Documents\generatedScript.sql");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder FileContent = new StringBuilder();

            Random rand = new Random();
            using (StreamReader fileStream = File.OpenText(@"C:\Users\Moh\Documents\hotelsAddresses.txt"))
            {

                String Content = fileStream.ReadToEnd();

                String[] table = Content.Split(';');

                for (int i = 0; i < table.Length; i++)
                {
                    String str = rand.Next(1,100).ToString() +" "+ table[i].ToLower().Trim() + ";";
                    FileContent.Append(str);
                }
  
            }


            using (FileStream fs = File.Create(@"C:\Users\Moh\Documents\hotelsAddresses.txt"))
            {
                // Add some text to file
                Byte[] Text = new UTF8Encoding(true).GetBytes(FileContent.ToString());
                fs.Write(Text, 0, Text.Length);

            }
        }
    }
}
