namespace MedPortal.WebApiClient.Models
{
    public class DoctorSearchModel : ISearchItemModel
    {
        public long Id { get; set; }

        public string Alias { get; set; }

        public string Name { get; set; }

        public CitySearchModel City { get; set; }
    }
}
