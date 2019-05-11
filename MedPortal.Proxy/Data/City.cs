namespace MedPortal.Proxy.Data
{
    public class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public object Phone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public long SearchType { get; set; }
        public bool HasDiagnostic { get; set; }
        public long TimeZone { get; set; }
    }
}