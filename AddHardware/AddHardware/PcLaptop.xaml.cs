using AddHardware.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AddHardware
{
    /// <summary>
    /// Interaction logic for PcLaptop.xaml
    /// </summary>
    public partial class PcLaptop : Window
    {
        public static int nummer;
        public static string melding;
        public static List<Locatie> locatieLijst;

        public PcLaptop()
        {
            InitializeComponent();

            foreach (var locatie in Server.ServerGetLocaties())
            {
                LocatieCombobox.Items.Add(locatie.Name + " " + locatie.Address);
            };

        }

        private void VoegToe_Click(object sender, RoutedEventArgs e)
        {
            if (LocatieCombobox.SelectedIndex != -1)
            {
                VoegPcOfLaptopToeAanDb();
                OutputVolgnummer.Text = Server.nummer.ToString().PadLeft(5, '0');
                OutputErrorMessage.Text = Server.melding;
            }
            else
            {
                MessageBox.Show("Je dient eerst een locatie te kiezen alvorens je een pc of laptop kan toevoegen aan de databank");
            }


        }
        #region Methods
        private void VoegPcOfLaptopToeAanDb()
        {
            #region GetSystemInfo
            ProcessStartInfo procReadCsv = new ProcessStartInfo("cmd", "/c " + "Systeminfo /fo csv");

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procReadCsv.RedirectStandardOutput = true;
            procReadCsv.UseShellExecute = false;
            // Do not create the black window.
            procReadCsv.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            Process procCSV = new Process();
            procCSV.StartInfo = procReadCsv;
            procCSV.Start();

            // Get the output into a string
            string namen = procCSV.StandardOutput.ReadLine();
            string values = procCSV.StandardOutput.ReadLine();
            values = values.Replace(';', ',');
            #endregion

            #region splitnamenAndValues
            string namen2 = namen.Replace("\",\"", ";");
            string values2 = values.Replace("\",\"", ";");
            string[] namenArr = namen2.Split(';');
            string[] valuesArr = values2.Split(';');
            string error = "Niet Beschikbaar";
            valuesArr[valuesArr.Length - 1] = (error);
            #endregion           

            #region get host name            
            string hostName = AsForDataFromPCInfo(namenArr, valuesArr, "\"Host Name");
            //Console.WriteLine("hostName: " + hostName);
            #endregion

            #region os name            
            string osName = AsForDataFromPCInfo(namenArr, valuesArr, "OS Name");
            //Console.WriteLine("os name: " + osName);
            #endregion

            #region registered owner            
            string registeredOwner = AsForDataFromPCInfo(namenArr, valuesArr, "Registered Owner");
            //Console.WriteLine("registered owner: " + registeredOwner);
            #endregion

            #region product id            
            string productId = AsForDataFromPCInfo(namenArr, valuesArr, "Product ID");
            //Console.WriteLine("product id: " + productId);
            #endregion

            #region installdate           
            string installDate = AsForDataFromPCInfo(namenArr, valuesArr, "Original Install Date");
            string[] dateT = installDate.Split(',');
            string date = dateT[0];
            //Console.WriteLine("installdate: " + date);
            #endregion

            #region system manufacturer           
            string systemManufacturer = AsForDataFromPCInfo(namenArr, valuesArr, "System Manufacturer");
            //Console.WriteLine("system manufacturer: " + systemManufacturer);
            #endregion

            #region system model           
            string systemModel = AsForDataFromPCInfo(namenArr, valuesArr, "System Model");
            //Console.WriteLine("system model: " + systemModel);
            #endregion

            #region system type            
            string systemType = AsForDataFromPCInfo(namenArr, valuesArr, "System Type");
            //Console.WriteLine("system type: " + systemType);
            #endregion

            #region processor installed           
            string processor = AsForDataFromPCInfo(namenArr, valuesArr, "Processor(s)");
            //Console.WriteLine("processor: " + processor);
            #endregion

            #region bios version           
            string bios = AsForDataFromPCInfo(namenArr, valuesArr, "BIOS Version");
            //Console.WriteLine("bios: " + bios);
            #endregion

            #region locatie            
            string locatie = AsForDataFromPCInfo(namenArr, valuesArr, "Input Locale");
            //Console.WriteLine("locatie: " + locatie);
            #endregion

            #region total phisical memory           
            string totalFisiekGeheugen = AsForDataFromPCInfo(namenArr, valuesArr, "Total Physical Memory");
            string[] geheugen = totalFisiekGeheugen.Split(' ');
            int fisiekGeheugen = (int)Math.Floor(Convert.ToDouble(geheugen[0]));
            //Console.WriteLine("totalFisiekGeheugen: " + totalFisiekGeheugen);
            #endregion          

            #region Drive space
            // Create a DriveInfo instance of the D:\ drive
            DriveInfo dDrive = new DriveInfo("C");
            double freeSpacePerc = 0;
            // When the drive is accessible..
            if (dDrive.IsReady)
            {
                // Calculate the percentage free space
                freeSpacePerc =
                   (dDrive.AvailableFreeSpace / (float)dDrive.TotalSize) * 100;
                //  dDrive.Name, dDrive.DriveFormat, dDrive.DriveType               
            }

            decimal hardeSchijfMB = Math.Round((decimal)dDrive.TotalSize / 1048576, 2);
            string vrijRuimteC = "hardeSchijfCappaciteit: " + Math.Round(freeSpacePerc, 2).ToString() + "%";
            string capaciteitC = "capaciteit harde schijf: " + hardeSchijfMB.ToString();
            int capC = (int)Math.Floor(hardeSchijfMB);
            decimal precentFreeC = (decimal)Math.Round(freeSpacePerc, 2);
            //Console.WriteLine(vrijRuimteC);
            //Console.WriteLine(capaciteitC);
            #endregion

            #region user
            var user = System.Environment.UserName;
            string userName = "userName: " + user;
            //Console.WriteLine(userName);
            #endregion

            #region system data
            string serieNumber = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Product, SerialNumber FROM Win32_BaseBoard");
            ManagementObjectCollection information = searcher.Get();
            foreach (ManagementObject obj in information)
            {
                foreach (PropertyData data in obj.Properties)
                {
                    // Console.WriteLine("{0} = {1}", data.Name, data.Value);
                    serieNumber = data.Value.ToString();
                }
            }
            searcher.Dispose();
            // Console.WriteLine("serieNummer: " + serieNumber);
            #endregion

            #region werklocatie
            string werkLocatie = LocatieCombobox.SelectedItem.ToString();

            #endregion

            #region aanmaken nieuwe pc
          
            int x = Server.GetVolgNummer("https://Api.apkgroup.eu/api/v1/hardwareinfo/PC");
           
            Pc nieuwePc = new Pc(x, hostName, osName, registeredOwner, productId, date, systemManufacturer, systemModel, processor, bios, locatie, fisiekGeheugen, user, serieNumber, capC, precentFreeC, werkLocatie);
            
            #endregion

            // PostPcToDb(nieuwePc);
            Server.PostToDb(nieuwePc, "https://Api.apkgroup.eu/api/v1/hardwareinfo/PC");
        }

        public static string AsForDataFromPCInfo(string[] namenArray, string[] valueArray, string controlleString)
        {
            int index = namenArray.Length - 1;
            foreach (var item in namenArray)
            {
                if (item == controlleString)
                {
                    index = Array.IndexOf(namenArray, item);
                }
            }
            string returnString = valueArray[index];
            return returnString;
        }

        #endregion

        private void SluitAf_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
