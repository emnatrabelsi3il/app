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

        public string Numero { get; set; }

        public string MacAdress { get; set; }

        public long SalleId { get; set; }

        public bool IsConnected { get; set; }
    }
}
