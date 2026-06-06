using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    public class Poste
    {
        public int Id { get; set; }

        public string Numero { get; set; } = string.Empty;

        public string MacAdress { get; set; } = string.Empty;

        public long SalleId { get; set; }

        public bool IsConnected { get; set; }

        public string? NomMachine { get; set; }

        public string? AdresseIP { get; set; }

        public string? OsVersion { get; set; }

        public double? RamDisponibleMb { get; set; }

        public double? RamTotaleMb { get; set; }

        public double? DisqueTotalGb { get; set; }

        public double? DisqueLibreGb { get; set; }

        public DateTime? LastSeen { get; set; }

        public string? SalleNom { get; set; }

        public string? EtablissementNom { get; set; }
    }
}