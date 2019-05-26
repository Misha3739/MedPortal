namespace MedPortal.Proxy.Data
{
    public class Station
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public long CityId { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string LineColor { get; set; }

        public string LineName { get; set; }

		public long TimeWalking { get; set; }
	}
}