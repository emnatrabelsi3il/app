using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Model
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public Utilisateur Utilisateur { get; set; }

    }
}
