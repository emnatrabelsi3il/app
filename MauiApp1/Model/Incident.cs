using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    public class Incident
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int UtilisateurId { get; set; }

        public int? PosteId { get; set; }

        public int? SalleId { get; set; }

        public int? EtablissementId { get; set; }
    }
}
