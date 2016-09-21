using Star_Dundee_WPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.IO;
using Microsoft.Win32;




namespace Star_Dundee_WPF
{
    /// <summary>
    /// Interaction logic for SpaceHome.xaml
    /// </summary>
    public partial class SpaceHome : Page
    {
        public SpaceHome()
        {
            InitializeComponent();

            // Set the ItemsSource to autogenerate the columns.
            dataGrid1.ItemsSource = OverviewTest.GetSampleOverviewData();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Recording files (*.rec;)|*.rec;|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string[] files = openFileDialog.FileNames;
                FileParser myFileParser = new FileParser();
                myFileParser.parse(files);
            }
        }
    }

    public class ErrorHighlighter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorString = value as string;
            if (errorString == null) return null;

            if (errorString == "No Error") return Brushes.Green;
            else if (errorString == "Disconnect") return Brushes.Crimson;
            else if (errorString == "Parity") return Brushes.DarkRed;
            else if (errorString == "CRC") return Brushes.DarkSalmon;
            else if (errorString == "EEP") return Brushes.Red;
            else if (errorString == "Timeout") return Brushes.IndianRed;
            else if (errorString == "Babbling Idiot") return Brushes.Plum;
            else if (errorString == "") return Brushes.LightBlue;
            else return Brushes.LightSteelBlue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();

        }
    }
}

   