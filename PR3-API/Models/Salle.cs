namespace PR3_API.Models
{
    public class Salle
    {
        public int Id { get; set; }
        public string Numero { get; set; }

        public int EtablissementId { get; set; }
    }
}
