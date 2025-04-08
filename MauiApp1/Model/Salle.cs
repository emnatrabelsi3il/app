using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    public class Salle
    {
        public int Id { get; set; }
        public string Numero { get; set; }

        public int EtablissementId { get; set; }
    }
}
