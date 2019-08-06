namespace MedPortal.WebApiClient.Models
{
    public interface IGeoSearchItemModel : ISearchItemModel
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}
