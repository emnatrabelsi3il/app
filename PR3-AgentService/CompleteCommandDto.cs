using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR3_AgentService
{
    public class CompleteCommandDto
    {
        public bool Success { get; set; }

        public string Resultat { get; set; } = string.Empty;
    }
}
