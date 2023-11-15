using System.ComponentModel.DataAnnotations;

namespace ArtGuard.Models.Domain
{
    public class SejsaTreningowa
    {
        public int Id { get; set; }

        [Required]
        public DateTime DataCwiczenia { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<DaneWykonanegoCwiczenia> ListaOdbytychCwiczenWSesji { get; set; } = new List<DaneWykonanegoCwiczenia> { };

    }
}
