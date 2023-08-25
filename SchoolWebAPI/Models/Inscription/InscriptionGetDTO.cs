namespace SchoolWebAPI.Models.Inscription
{
    public class InscriptionGetDTO
    {
        public SchoolWebAPI.Entities.Course Course { get; set; } = null!;

        public SchoolWebAPI.Entities.Teacher Teacher { get; set; } = null!;

        public SchoolWebAPI.Entities.Student Student { get; set; } = null!;
    }
}
