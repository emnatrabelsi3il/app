using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR3_WPF.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public Utilisateur Utilisateur { get; set; }
    }
}
