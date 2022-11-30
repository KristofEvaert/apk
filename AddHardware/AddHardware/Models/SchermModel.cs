using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddHardware.Models
{
    public class SchermModel
    {
        public string Naam { get; set; }
        public decimal Lengte { get; set; }

        public decimal Breedte { get; set; }

        public bool HDMI { get; set; }
        public bool VGA { get; set; }

        public bool USBC { get; set; }
        public bool Display { get; set; }

        public bool Webcam { get; set; }
        public string Locatie { get; set; }

        public int volgNummer { get; set; }
        public string gebruiker { get; set; }
    }
}
