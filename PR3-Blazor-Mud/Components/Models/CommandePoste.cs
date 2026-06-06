namespace PR3_Blazor_Mud.Components.Models
{
    public class CommandePoste
    {
        public int Id { get; set; }

        public int? PosteId { get; set; }

        public string MacAdress { get; set; } = string.Empty;

        public string TypeCommande { get; set; } = string.Empty;

        public string Portee { get; set; } = string.Empty;

        public string Statut { get; set; } = string.Empty;

        public DateTime DateCreation { get; set; }

        public DateTime? DateExecution { get; set; }

        public string Resultat { get; set; } = string.Empty;
    }
}