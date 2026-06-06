namespace PR3_SecureAPI.Models
{
    public class CommandePoste
    {
        public int Id { get; set; }

        public int? PosteId { get; set; }

        public string MacAdress { get; set; } = string.Empty;

        public string TypeCommande { get; set; } = string.Empty;

        public string Portee { get; set; } = string.Empty; // Single ou Global

        public string Statut { get; set; } = "Pending"; // Pending, Done, Failed

        public DateTime DateCreation { get; set; } = DateTime.Now;

        public DateTime? DateExecution { get; set; }

        public string Resultat { get; set; } = string.Empty;
    }
}