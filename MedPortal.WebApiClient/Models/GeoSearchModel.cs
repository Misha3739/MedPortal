namespace MedPortal.WebApiClient.Models
{
    public class GeoSearchModel : IGeoSearchItemModel
    {
        public long Id { get; set; }

        public string Alias { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoCategoryEnum Type { get; set; }
    }
}
