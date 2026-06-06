namespace PR3_SecureAPI.Models
{
    public class Poste
    {
        public int Id { get; set; }

        public string Numero { get; set; }

        public string MacAdress { get; set; }

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
    }


}
