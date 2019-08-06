namespace MedPortal.WebApiClient.Models
{
    public interface IGeoSearchItemModel : ISearchItemModel
    {
        double Latitude { get; set; }
        double Longtitude { get; set; }
    }
}
