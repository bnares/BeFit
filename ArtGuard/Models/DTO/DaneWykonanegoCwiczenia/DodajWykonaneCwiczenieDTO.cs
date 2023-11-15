namespace ArtGuard.Models.DTO.DaneWykonanegoCwiczenia
{
    public class DodajWykonaneCwiczenieDTO
    {
        public int IdCwiczenia { get; set; }
        public int ObciazenieWCwiczeniu { get; set; }

        public int LiczbaPowtorzen { get; set; }
        public int LiczbaSerii { get; set; }
    }
}
