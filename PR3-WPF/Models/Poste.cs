using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR3_WPF.Models
{
    public class Poste
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string MacAdress { get; set; }
        public bool IsConnected { get; set; }

        public string NomMachine { get; set; }
        public string AdresseIP { get; set; }
        public string OsVersion { get; set; }
        public long SalleId { get; set; }
        public double? RamDisponibleMb { get; set; }
        public double? RamTotaleMb { get; set; }

        public double? DisqueTotalGb { get; set; }
        public double? DisqueLibreGb { get; set; }

        public DateTime? LastSeen { get; set; }

        public string SalleNom { get; set; }
        public string EtablissementNom { get; set; }
    }
}
