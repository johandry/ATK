using System;
using System.Collections.Generic;
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
using System.Configuration;

using System.IO;
using System.Net;
using System.Net.Sockets;

using CsvHelper;

using System.ComponentModel;


namespace ATK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BindingList<Command> CommandsList = new BindingList<Command>();
        private String previousDescription;
        private String previousLabel;
        private String host = "localhost";
        private Int32 port = 3001;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                previousLabel = (String)DescriptionLabel.Content;
                DescriptionLabel.Content = "Connection Log";
                previousDescription = DescriptionTextBlock.Text;

                DescriptionTextBlock.Text = "Connecting.....";
                TcpClient tcpclnt = new TcpClient(host, port);
                DescriptionTextBlock.Text += "Connected\n";
                NetworkStream stream = tcpclnt.GetStream();

                String str = ((Command)CommandComboBox.SelectedItem).ItemCommand + "\n";
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(str);
                DescriptionTextBlock.Text += "Transmitting command '" + str + "'.....";
                stream.Write(data, 0, data.Length);
                DescriptionTextBlock.Text += "Done\n";

                str = ParameterTextBox.Text + "\n";
                data = System.Text.Encoding.ASCII.GetBytes(str);
                DescriptionTextBlock.Text += "Transmitting parameter '" + str + "'.....";
                stream.Write(data, 0, data.Length);
                DescriptionTextBlock.Text += "Done\n";

                data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                DescriptionTextBlock.Text += "Results: " + responseData + "\n";

                stream.Close();
                tcpclnt.Close();

                DescriptionTextBlock.Text += "\n";
                DescriptionTextBlock.Text = previousDescription;
                DescriptionLabel.Content = previousLabel;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to " + host + ":" + port + ":  " + ex.StackTrace, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                DescriptionTextBlock.Text = previousDescription;
                DescriptionLabel.Content = previousLabel;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            PopulateCommandItems();
            PopulateCommandComboBox();
        }

        private void PopulateCommandComboBox()
        {

            CommandComboBox.ItemsSource = CommandsList;
            CommandComboBox.DisplayMemberPath = "ItemName";
            CommandComboBox.SelectedValuePath = "ItemCommand";
        }

        private void PopulateCommandItems()
        {
            var fileName = @"C:\Users\Johandry.Amador\Documents\GitHub\ATK\Client\Visual Studio - C#\ATK\Properties\DataSources\Commands.csv";
            var fileReader = File.OpenText(fileName);
            var csvReader = new CsvHelper.CsvReader(fileReader);
            while (csvReader.Read())
            {
                CommandsList.Add(new Command() { ItemName = csvReader.CurrentRecord[3], 
                    ItemCommand = csvReader.CurrentRecord[0],
                    ItemDescription = csvReader.CurrentRecord[4]
                });
            }
        }

        private void CommandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var oSelectedCommand = (Command)CommandComboBox.SelectedItem;

            DescriptionTextBlock.Text = oSelectedCommand.ItemDescription + "\n" +
                "(" + oSelectedCommand.ItemCommand + "," + ParameterTextBox.Text + ")\n";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
