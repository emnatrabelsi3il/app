using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR3_AgentService
{
    public class AgentHeartbeatDto
    {
        public string Numero { get; set; } = string.Empty;

        public string MacAdress { get; set; } = string.Empty;

        public string NomMachine { get; set; } = string.Empty;

        public string AdresseIP { get; set; } = string.Empty;

        public string OsVersion { get; set; } = string.Empty;

        public double RamDisponibleMb { get; set; }

        public double RamTotaleMb { get; set; }

        public double DisqueTotalGb { get; set; }

        public double DisqueLibreGb { get; set; }
    }
}
