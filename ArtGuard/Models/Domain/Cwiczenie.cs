using Microsoft.Build.Framework;

namespace ArtGuard.Models.Domain
{
    public class Cwiczenie
    {
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
       

    }
}
