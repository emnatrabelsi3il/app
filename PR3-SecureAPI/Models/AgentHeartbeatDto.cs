namespace PR3_SecureAPI.Models
{
    public class AgentHeartbeatDto
    {
        public string Numero { get; set; }
        public string MacAdress { get; set; }
        public string NomMachine { get; set; }
        public string AdresseIP { get; set; }
        public string OsVersion { get; set; }
        public double RamDisponibleMb { get; set; }
        public double RamTotaleMb { get; set; }
        public double DisqueTotalGb { get; set; }
        public double DisqueLibreGb { get; set; }
    }
}