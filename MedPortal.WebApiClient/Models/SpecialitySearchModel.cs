namespace MedPortal.WebApiClient.Models
{
    public class SpecialitySearchModel : ISearchItemModel
    {
        public long Id { get; set; }

        public string Alias { get; set; }

        public string Name { get; set; }
    }
}
