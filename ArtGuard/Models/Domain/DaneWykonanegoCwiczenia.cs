using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Reflection.Metadata.Ecma335;

namespace ArtGuard.Models.Domain
{
    public class DaneWykonanegoCwiczenia
    {
        public int Id { get; set; }
        public int IdSesjiTreningowej { get; set; }
        public SejsaTreningowa SesjaTreningowa { get; set; }
        public int IdCwiczenia { get; set; }
        public Cwiczenie Cwiczenie { get; set; }
        [IntegerValidator(MinValue = 0)]
        public int ObciazenieWCwiczeniu { get; set; }
        [IntegerValidator(MinValue =0)]
        public int LiczbaPowtorzen { get; set; }
        [IntegerValidator(MinValue = 0)]
        public int LiczbaSerii { get; set; }

    }
}
