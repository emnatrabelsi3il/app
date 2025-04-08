namespace PR3_Blazor.Components.Models
{
    public class Incident
{
    public int Id { get; set; }

    public string Description { get; set; }

    public int UtilisateurId { get; set; }

    public int? PosteId { get; set; }

    public int? SalleId { get; set; }

    public int? EtablissementId { get; set; }

        public Utilisateur? Responsable { get; set; } // Nouvelle propriété

        public Poste? poste { get; set; }

        public Salle? salle { get; set; }

        public Etablissement? etablissement { get; set; }
    }
}
