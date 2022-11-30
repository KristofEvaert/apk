using AddHardware.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for Scherm.xaml
    /// </summary>
    public partial class Scherm : Window
    {
        public static int nummer;
        public static string melding;
       
        public Scherm()
        {
            InitializeComponent();
            foreach (var locatie in Server.ServerGetLocaties())
            {
                LocatieComboBox.Items.Add(locatie.Name + " " + locatie.Address);
            }
        }
        public SchermModel scherm = new SchermModel();
        public void MaakSchermAan()
        {
            scherm.Naam = "Scherm" + Server.GetVolgNummer("https://api.apkgroup.eu/api/v1/hardwareinfo/Scherm").ToString();
            scherm.Lengte = Convert.ToDecimal(AfmetingLengteTextBox.Text);
            scherm.Breedte = Convert.ToDecimal(AfmetingBreedteTextBox.Text);
            scherm.Webcam = false;
            scherm.HDMI = false;
            scherm.VGA = false;
            scherm.USBC = false;
            scherm.Display = false;
            if ((bool)WebcamCheckbox.IsChecked)
            {
                scherm.Webcam = true;
            }
            if (HDMICheck.IsChecked == true)
            {
                scherm.HDMI = true;
            }
            if (VGAcheck.IsChecked == true)
            {
                scherm.VGA = true;
            }
            if (USBCCheck.IsChecked == true)
            {
                scherm.USBC = true;
            }
            if (DisplayPCheck.IsChecked == true)
            {
                scherm.Display = true;
            }
            scherm.Locatie = LocatieComboBox.SelectedItem.ToString();
            scherm.volgNummer = Server.GetVolgNummer("https://api.apkgroup.eu/api/v1/hardwareinfo/Scherm");
            scherm.gebruiker = System.Environment.UserName;
        }

        private void VoegToe_Click(object sender, RoutedEventArgs e)
        {
            if (AfmetingLengteTextBox.Text == string.Empty)
            {
                MessageBox.Show("gelieve alle waarden in te geven");               
                return;
            }
            if (AfmetingBreedteTextBox.Text == string.Empty)
            {
                MessageBox.Show("gelieve alle waarden in te geven");              
                return;
            }
            if (LocatieComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("gelieve alle waarden in te geven");               
                return;
            }
            else
            {
                MaakSchermAan();
                Server.PostToDb(scherm, "https://api.apkgroup.eu/api/v1/hardwareinfo/Scherm");
                volgnummer.Content = "volgnummer: " + nummer.ToString().PadLeft(5, '0');
            }
        }
    }
}
