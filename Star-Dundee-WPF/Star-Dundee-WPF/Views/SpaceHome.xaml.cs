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
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Data;
using System.ComponentModel;

namespace Star_Dundee_WPF
{
    /// <summary>
    /// Interaction logic for SpaceHome.xaml
    /// </summary>
    public partial class SpaceHome : Page
    {
        FileParser myFileParser;
        //public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public SpaceHome()
        {

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Recording files (*.rec;)|*.rec;|All files (*.*)|*.*";

            string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, "../../DataFiles/"));

            if (Directory.Exists(path))
            {
                openFileDialog.InitialDirectory = path;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            if (openFileDialog.ShowDialog() == true)
            {
                string[] files = openFileDialog.FileNames;
                myFileParser = new FileParser();

                bool filesValid = myFileParser.startParsing(files);
                if (filesValid)
                {
                    // Set the ItemsSource to autogenerate the columns.
                    List<GridColumn> listToDisplay = myFileParser.listOfColumns;
                    //printListOfColumns(listToDisplay);
                    dataGrid1.ItemsSource = listToDisplay;
                    dataGrid1.Columns[1].Visibility = Visibility.Collapsed;
                    //Set the recording to the datacontext
                    this.DataContext = myFileParser.mainRecording;
                    //set graph values
                    setGraphValues();

                }
                else
                {
                    //message to say there was a file issue
                    MessageBox.Show("Error reading file(s) - please try again", "File Error");
                }
            }
        }

        private void setGraphValues()
        {
            //for the ports on the graphs
            int port1, port2, port3, port4, port5, port6, port7, port8; //Packets
            port1 = port2 = port3 = port4 = port5 = port6 = port7 = port8 = 0;
            int port1A, port2A, port3A, port4A, port5A, port6A, port7A, port8A; //Characters
            port1A = port2A = port3A = port4A = port5A = port6A = port7A = port8A = 0;
            int port1B, port2B, port3B, port4B, port5B, port6B, port7B, port8B; //Errors
            port1B = port2B = port3B = port4B = port5B = port6B = port7B = port8B = 0;
            decimal port1C, port2C, port3C, port4C, port5C, port6C, port7C, port8C; //Packet Rate
            port1C = port2C = port3C = port4C = port5C = port6C = port7C = port8C = 0;
            decimal port1D, port2D, port3D, port4D, port5D, port6D, port7D, port8D; //Data Rate
            port1D = port2D = port3D = port4D = port5D = port6D = port7D = port8D = 0;
            decimal port1E, port2E, port3E, port4E, port5E, port6E, port7E, port8E; //Error Rate
            port1E = port2E = port3E = port4E = port5E = port6E = port7E = port8E = 0;   
            
            //get the chart values
            for (int i = 0; i < myFileParser.mainRecording.ports.Count; i++)
            {
                switch(myFileParser.mainRecording.ports[i].portNumber)
                {
                    case 1:
                        port1 = myFileParser.mainRecording.ports[i].totalPackets;
                        port1A= myFileParser.mainRecording.ports[i].totalCharacters;
                        port1B = myFileParser.mainRecording.ports[i].totalErrors;
                        port1C = myFileParser.mainRecording.ports[i].dataRate;
                        port1D = myFileParser.mainRecording.ports[i].errorRate;
                        port1E = myFileParser.mainRecording.ports[i].packetRate;
                        break;

                    case 2:
                        port2 = myFileParser.mainRecording.ports[i].totalPackets;
                        port2A = myFileParser.mainRecording.ports[i].totalCharacters;
                        port2B = myFileParser.mainRecording.ports[i].totalErrors;
                        port2C = myFileParser.mainRecording.ports[i].dataRate;
                        port2D = myFileParser.mainRecording.ports[i].errorRate;
                        port2E = myFileParser.mainRecording.ports[i].packetRate;
                        
                        break;

                    case 3:
                        port3 = myFileParser.mainRecording.ports[i].totalPackets;
                        port3A = myFileParser.mainRecording.ports[i].totalCharacters;
                        port3B = myFileParser.mainRecording.ports[i].totalErrors;
                        port3C = myFileParser.mainRecording.ports[i].dataRate;
                        port3D = myFileParser.mainRecording.ports[i].errorRate;
                        port3E = myFileParser.mainRecording.ports[i].packetRate;
                        break;

                    case 4:
                        port4 = myFileParser.mainRecording.ports[i].totalPackets;
                        port4A = myFileParser.mainRecording.ports[i].totalCharacters;
                        port4B = myFileParser.mainRecording.ports[i].totalErrors;
                        port4C = myFileParser.mainRecording.ports[i].dataRate;
                        port4D = myFileParser.mainRecording.ports[i].errorRate;
                        port4E = myFileParser.mainRecording.ports[i].packetRate;
                        break;

                    case 5:
                        port5 = myFileParser.mainRecording.ports[i].totalPackets;
                        port5A = myFileParser.mainRecording.ports[i].totalCharacters;
                        port5B = myFileParser.mainRecording.ports[i].totalErrors;
                        port5C = myFileParser.mainRecording.ports[i].dataRate;
                        port5D = myFileParser.mainRecording.ports[i].errorRate;                        
                        port5E = myFileParser.mainRecording.ports[i].packetRate;
                        break;

                    case 6:
                        port6 = myFileParser.mainRecording.ports[i].totalPackets;
                        port6A = myFileParser.mainRecording.ports[i].totalCharacters;
                        port6B = myFileParser.mainRecording.ports[i].totalErrors;
                        port6C = myFileParser.mainRecording.ports[i].dataRate;
                        port6D = myFileParser.mainRecording.ports[i].errorRate;
                        port6E = myFileParser.mainRecording.ports[i].packetRate;
                        break;

                    case 7:
                        port7 = myFileParser.mainRecording.ports[i].totalPackets;
                        port7A = myFileParser.mainRecording.ports[i].totalCharacters;
                        port7B = myFileParser.mainRecording.ports[i].totalErrors;
                        port7C = myFileParser.mainRecording.ports[i].dataRate;
                        port7D = myFileParser.mainRecording.ports[i].errorRate;
                        port7E = myFileParser.mainRecording.ports[i].packetRate;
                        break;

                    case 8:
                        port8 = myFileParser.mainRecording.ports[i].totalPackets;
                        port8A = myFileParser.mainRecording.ports[i].totalCharacters;
                        port8B = myFileParser.mainRecording.ports[i].totalErrors;
                        port8C = myFileParser.mainRecording.ports[i].dataRate;
                        port8D = myFileParser.mainRecording.ports[i].errorRate;
                        port8E = myFileParser.mainRecording.ports[i].packetRate;
                        break;

                }
            }

            myFileParser.mainRecording.graphs.packetTotalCollection.Add(new RowSeries
            {
                Title = "Packets",
                Values = new ChartValues<double> { port8, port7, port6, port5, port4, port3, port2, port1 },                
        });

            myFileParser.mainRecording.graphs.charactersTotalCollection.Add(new RowSeries
            {
                Title = "Characters",
                Values = new ChartValues<double> { port8A, port7A, port6A, port5A, port4A, port3A, port2A, port1A },
            });

            myFileParser.mainRecording.graphs.errorsTotalCollection.Add(new RowSeries
            {
                Title = "Errors",
                Values = new ChartValues<double> { port8B, port7B, port6B, port5B, port4B, port3B, port2B, port1B },
            });

            myFileParser.mainRecording.graphs.dataRateCollection.Add(new RowSeries
            {
                Title = "Data Rate",
                Values = new ChartValues<decimal> { port8C, port7C, port6C, port5C, port4C, port3C, port2C, port1C },
            });

            myFileParser.mainRecording.graphs.errorRateCollection.Add(new RowSeries
            {
                Title = "Error Rate",
                Values = new ChartValues<decimal> { port8D, port7D, port6D, port5D, port4D, port3D, port2D, port1D },
            });

            myFileParser.mainRecording.graphs.packetRateCollection.Add(new RowSeries
            {
                Title = "Packet Rate",
                Values = new ChartValues<decimal> { port8E, port7E, port6E, port5E, port4E, port3E, port2E, port1E },
            });

            myFileParser.mainRecording.graphs.dataRateTimeCollection.Add(new LineSeries
            {
                Title = "Data Rate/Time",
                Values = new ChartValues<double> { 20, 33, 47, 52, 41, 32, 24, 12 },
                PointGeometry = null
            });

           
        }

        private void printListOfColumns(List<GridColumn> listToDisplay)
        {
            foreach (GridColumn item in listToDisplay)
            {
                Console.WriteLine();
                Console.Write(item.getTime() + "\t");

                for (int i = 1; i < 9; i++)
                {
                    Console.Write(item.getPort(i) + " ");
                }
            }
        }

        private void dataDrid1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // make sure only 1 cell is clicked
            if (dataGrid1.SelectedCells.Count != 1)
                return;

            // Update port summary
            string portHeader = (string)dataGrid1.SelectedCells[0].Column.Header;
            int portIndex = dataGrid1.SelectedCells[0].Column.DisplayIndex;
            portIndex -= 1;
            //Console.WriteLine("port Index: " + portIndex);
            //Console.WriteLine("Port Clicked: " + portHeader);

            updatePortSummury(portIndex);


            //Update packet summary
            GridColumn row = (GridColumn)dataGrid1.CurrentItem;

            Console.WriteLine("Cell Time: " + row.time.ToString());
            Console.WriteLine("Cell Index: " + row.index.ToString());

            //get timestamp and index
            string cellTime = row.time.ToString();
            string cellIndex = row.index.ToString();

            //look for the matching packet with the timestamps in the port if that cell has a packet

            updatePacketSummary(cellIndex, portIndex);

        }

        private void updatePacketSummary(string cellIndex, int port)
        {

            // if(cellIndex != port)

            string[] packetSummary = new string[21] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            myFileParser.mainRecording.packetSummary = packetSummary;
            Packet myPacket = new Packet();
            int pIndex = myFileParser.mainRecording.ports.FindIndex(p => p.portNumber == port);

            Console.WriteLine("pINdex: " + pIndex);
            bool hasError = false;
            bool isRMAP = false;

            //check if port exists
            bool exists = false;
            int portIndex = port;

            for (int i = 0; i < myFileParser.mainRecording.ports.Count; i++)
            {
                if (myFileParser.mainRecording.ports[i].portNumber == port)
                {
                    exists = true;
                    portIndex = i;
                }

            }
            port -= 1;

            int packetPort = 0;
            //Get the right port
            for (int j = 0; j < myFileParser.mainRecording.ports.Count; j++)
            {
                if(myFileParser.mainRecording.ports[j].portNumber == pIndex)
                {
                    packetPort = j;
                    break;
                }
            }
            if (myFileParser.mainRecording.ports.Count == 1)
                pIndex = 0;

                //Get the packet
                if (exists)
            {
                for (int i = 0; i < myFileParser.mainRecording.ports[pIndex].packets.Count; i++)
                {
                    if (myFileParser.mainRecording.ports[pIndex].packets[i].packetIndex != null)
                    {
                        if (cellIndex == myFileParser.mainRecording.ports[pIndex].packets[i].packetIndex.ToString())
                        {
                            myPacket = myFileParser.mainRecording.ports[pIndex].packets[i];
                            Console.WriteLine("index in packet: " + myFileParser.mainRecording.ports[pIndex].packets[i].packetIndex.ToString());
                            if (myPacket.protocol !=null)
                            {
                                if (myPacket.protocol.Equals("RMAP"))
                                {
                                    isRMAP = true;
                                }
                                else
                                {
                                    isRMAP = false;
                                }
                            }
                            else
                            {
                                isRMAP = false;
                            }
                            hasError = myPacket.getErrorStatus();
                            break;
                        }
                    }
                   
                }
            }

            if (myPacket.packetIndex == null)
            {
                writePanel.Visibility = System.Windows.Visibility.Collapsed;
                writeRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                readPanel.Visibility = System.Windows.Visibility.Collapsed;
                readRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                dataPanel.Visibility = System.Windows.Visibility.Collapsed;
                notRmapPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (!hasError && isRMAP)
                {
                    myFileParser.mainRecording.packetSummary = getPacketSummary(myPacket);
                }
                else if (!hasError && !isRMAP)
                {
                    myFileParser.mainRecording.packetSummary = getCustomSummary(myPacket,false);
                }
                else
                {
                    //do something with error details?
                    myFileParser.mainRecording.packetSummary = getCustomSummary(myPacket,true);
                }
            }

        }



        private string[] getCustomSummary(Packet packet, bool isErroneous)
        {
            //Get command/packet type
            string[] packetSummary = new string[21] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

            if (!isErroneous)
            {


                //packetSummary[0] = myRMAP.command;
               // packetSummary[1] = packet.protocol;
                //packetSummary[2] = myRMAP.destinationKey.ToString("X");
                // packetSummary[3] = myRMAP.sourcelogicalAddress.ToString("X");
                //packetSummary[17] = packet.protocol;
               // packetSummary[18] = packet.packetType.ToString();

                if (packet.dataArray != null)
                {

                    packetSummary[19] = String.Join(" ", packet.dataArray.Select(s => s.ToString()));
                }
            }
            else
            {
                packetSummary[1] = packet.protocol;
                packetSummary[18] = packet.packetType.ToString();


                packetSummary[19] += "Error Type : " + packet.getErrorType().ToString();
                
            }

            
            writePanel.Visibility = System.Windows.Visibility.Collapsed;
            writeRepPanel.Visibility = System.Windows.Visibility.Collapsed;
            readPanel.Visibility = System.Windows.Visibility.Collapsed;
            readRepPanel.Visibility = System.Windows.Visibility.Collapsed;
            notRmapPanel.Visibility = System.Windows.Visibility.Visible;
            dataPanel.Visibility = System.Windows.Visibility.Visible;

            return packetSummary;
        }

        private string[] getPacketSummary(Packet packet)
        {
            //if(placeholder)
            //Get the rmap details for the packet
            RMAP myRMAP = new RMAP();
            myRMAP.buildPacket(packet.dataArray);

            //Get command/packet type
            string[] packetSummary = new string[21] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" , "", "", "", "" };

            packetSummary[0] = myRMAP.command;
            packetSummary[1] = packet.protocol;
            packetSummary[2] = myRMAP.destinationKey.ToString("X2");
            packetSummary[3] = myRMAP.sourcelogicalAddress.ToString("X2");
            packetSummary[4] = String.Join(" ", myRMAP.transactionID.Select(s => s.ToString("X2")));
            packetSummary[5] = myRMAP.extWriteAdd.ToString("X2");
            packetSummary[6] = myRMAP.dataLengthInt.ToString();
            packetSummary[7] = myRMAP.headerCRC.ToString("X2");
            packetSummary[8] = myRMAP.dataCRC.ToString("X2");
            packetSummary[9] = myRMAP.status.ToString("X2");
            packetSummary[10] = myRMAP.destinationlogicalAddress.ToString("X2");
            packetSummary[13] = myRMAP.replyCRC.ToString("X2");
            packetSummary[14] = myRMAP.extReadAdd.ToString("X2");
            packetSummary[15] = myRMAP.readAddress.ToString("X2");
            packetSummary[16] = myRMAP.writeAddress.ToString("X2");
            packetSummary[17] = packet.protocol;
            packetSummary[18] = packet.packetType.ToString();
            if (myRMAP.data != null)
            {

                packetSummary[19] = String.Join(" ", myRMAP.data.Select(s => s.ToString("X2")));
            }
            
            // packetSummary[20] = packet.theData.getTheData().ToString();

            Console.WriteLine("Command: " + packetSummary[0]);
            Console.WriteLine("Protocol: " + packet.protocol);

            if(packet.protocol != "RMAP")
            {
                writePanel.Visibility = System.Windows.Visibility.Collapsed;
                writeRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                readPanel.Visibility = System.Windows.Visibility.Collapsed;
                readRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                notRmapPanel.Visibility = System.Windows.Visibility.Visible;
                dataPanel.Visibility = System.Windows.Visibility.Visible;
            }

            else if (myRMAP.command == "WRITE")
            {
                writePanel.Visibility = System.Windows.Visibility.Visible;
                writeRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                readPanel.Visibility = System.Windows.Visibility.Collapsed;
                readRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                notRmapPanel.Visibility = System.Windows.Visibility.Collapsed;
                dataPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else if (myRMAP.command == "WRITE REPLY")
            {
                writePanel.Visibility = System.Windows.Visibility.Collapsed;
                writeRepPanel.Visibility = System.Windows.Visibility.Visible;
                readPanel.Visibility = System.Windows.Visibility.Collapsed;
                readRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                notRmapPanel.Visibility = System.Windows.Visibility.Collapsed;
                dataPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else if (myRMAP.command == "READ")
            {
                writePanel.Visibility = System.Windows.Visibility.Collapsed;
                writeRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                readPanel.Visibility = System.Windows.Visibility.Visible;
                readRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                notRmapPanel.Visibility = System.Windows.Visibility.Collapsed;
                dataPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else if (myRMAP.command == "READ REPLY")
            {
                writePanel.Visibility = System.Windows.Visibility.Collapsed;
                writeRepPanel.Visibility = System.Windows.Visibility.Collapsed;
                readPanel.Visibility = System.Windows.Visibility.Collapsed;
                readRepPanel.Visibility = System.Windows.Visibility.Visible;
                notRmapPanel.Visibility = System.Windows.Visibility.Collapsed;
                dataPanel.Visibility = System.Windows.Visibility.Visible;
            }

            return packetSummary;
           
        }

        //string hexValue = intValue.ToString("X");
        private void updatePortSummury(int port)
        {
            string[] portSummary = new string[6] { "", "", "", "", "", "" };
            myFileParser.mainRecording.portSummary = portSummary;
            //check if port exists
            bool exists = false;
            int portIndex = port;

            for (int i = 0; i < myFileParser.mainRecording.ports.Count; i++)
            {
                if (myFileParser.mainRecording.ports[i].portNumber == port)
                {
                    exists = true;
                    portIndex = i;
                }

            }
            port -= 1;
            if (exists)
            {
                myFileParser.mainRecording.portSummary = getPortSummary(portIndex);
                portPanel.Visibility = System.Windows.Visibility.Visible;
            }


            DataContext = myFileParser.mainRecording;

            //if empty hide lables
            if (!exists)
            {
                portPanel.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public string[] getPortSummary(int port)
        {
            string[] portSummary = new string[6] { "", "", "", "", "", "" };

            portSummary[0] = myFileParser.mainRecording.ports[port].totalPackets.ToString();
            portSummary[1] = myFileParser.mainRecording.ports[port].totalErrors.ToString();
            portSummary[2] = myFileParser.mainRecording.ports[port].totalCharacters.ToString();
            portSummary[3] = myFileParser.mainRecording.ports[port].dataRate.ToString();
            portSummary[4] = myFileParser.mainRecording.ports[port].packetRate.ToString();
            portSummary[5] = myFileParser.mainRecording.ports[port].errorRate.ToString();

            return portSummary;
        }
    }

       


    public class ErrorHighlighter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errorString = value as string;
            if (errorString == null) return null;
            if (errorString == "Packet") return Brushes.Green;
            else if (errorString == "Disconnect") return Brushes.Crimson;
            else if (errorString == "Parity") return Brushes.DarkRed;
            else if (errorString == "CRCHeader") return Brushes.DarkSalmon;
            else if (errorString == "CRCData") return Brushes.DarkSalmon;
            else if (errorString == "CRC") return Brushes.DarkSalmon;
            else if (errorString == "EEP") return Brushes.Red;
            else if (errorString == "Timeout") return Brushes.IndianRed;
            else if (errorString == "BabblingIdiot") return Brushes.Plum;
            else if (errorString == "Length") return Brushes.Bisque;
            else if (errorString == "Sequence") return Brushes.BurlyWood;
            else if (errorString == "None") return Brushes.Tomato;
            else if (errorString == "") return Brushes.LightBlue;
            else return Brushes.LightSteelBlue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }

    
}


   

   