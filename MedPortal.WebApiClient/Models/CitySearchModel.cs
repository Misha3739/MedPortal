namespace MedPortal.WebApiClient.Models
{
    public class CitySearchModel : IGeoSearchItemModel
    {
        public long Id { get; set; }

        public string Alias { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longtitude { get; set; }
    }
}
