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
using Star_Dundee_WPF.Models;

namespace Star_Dundee_WPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Checkmate CRC = new Checkmate();
        CRC8 crc8 = new CRC8();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            crc8.testCRC();
            //CRC.checkmate("fa001750b8f6cad39e3c5274519fef80baf6759dec3aa625faaf0de4628247cff81c5cea5fa595781490c9dcd4a9b7fbdf370c9c08a0f060315b09536132dff94691f881d9f4404264c25ec14cff5b16540bb50f0a7b4276d6bf207370d4a8a51156da7a74d5583972ee38ab098c6bfbac69e50f680616ea792fe5bd07e41c5406ef752cc6c527cdcd58f9f290bd550c46b61f15b7fe082b8741cba8acedb57685a04b213640496fb2b70520592ec0d8c184b5aa60af80da8f8944cec65e0e9d1c2deef049e337afe17d0ccce94d19e19b6a5b45f8b70b47f05ad387eab1822848fcb302780a7d0ec80f5350b794daa732bb7260e6911214685b1a7c8");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileParser myFileParser = new FileParser();

            myFileParser.readFile();
        }
    }
}
