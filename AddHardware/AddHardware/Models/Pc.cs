using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddHardware.Models
{
    public class Pc
    {

        public Pc()
        {

        }
        [Required]
        public int volgnummer { get; set; }
        [StringLength(50)]
        public string hostName { get; set; }


        [StringLength(50)]
        public string osName { get; set; }

        [StringLength(50)]
        public string registeredOwner { get; set; }

        [StringLength(50)]
        public string productId { get; set; }

        public string installDate { get; set; }

        [StringLength(50)]
        public string systemManufacturer { get; set; }

        [StringLength(50)]
        public string systemModel { get; set; }

        [StringLength(255)]
        public string processor { get; set; }

        [StringLength(255)]
        public string bios { get; set; }

        [StringLength(50)]
        public string locatie { get; set; }

        public int totalFysiekGeheugen { get; set; }

        [StringLength(50)]
        public string userName { get; set; }

        [StringLength(50)]
        public string serieNummer { get; set; }

        public int hardeSchijfCapaciteitMB { get; set; }

        public decimal hardeSchijfCapaciteitPercent { get; set; }

        public string  werkLocatie { get; set; }

        public Pc(int volgnummer, string hostName, string osName, string registeredOwner, string productId, string installDate, string systemManufacturer, string systemModel, string processor, string bios, string locatie, int totalFysiekGeheugen, string userName, string serieNummer, int hardeSchijfCapaciteitMB, decimal hardeSchijfCapaciteitPercent, string werkLocatie)
        {
            this.volgnummer = volgnummer;
            this.hostName = hostName;
            this.osName = osName;
            this.registeredOwner = registeredOwner;
            this.productId = productId;
            this.installDate = installDate;
            this.systemManufacturer = systemManufacturer;
            this.systemModel = systemModel;
            this.processor = processor;
            this.bios = bios;
            this.locatie = locatie;
            this.totalFysiekGeheugen = totalFysiekGeheugen;
            this.userName = userName;
            this.serieNummer = serieNummer;
            this.hardeSchijfCapaciteitMB = hardeSchijfCapaciteitMB;
            this.hardeSchijfCapaciteitPercent = hardeSchijfCapaciteitPercent;
            this.werkLocatie = werkLocatie;

        }
    }
}
