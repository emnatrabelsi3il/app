using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR3_AgentService
{
   public class AgentCommandDto
    {
        public int Id { get; set; }

        public int? PosteId { get; set; }

        public string MacAdress { get; set; } = string.Empty;

        public string TypeCommande { get; set; } = string.Empty;

        public string Portee { get; set; } = string.Empty;

        public string Statut { get; set; } = string.Empty;
    }
}
