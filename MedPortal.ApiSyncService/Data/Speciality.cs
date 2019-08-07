namespace MedPortal.Proxy.Data
{
    public class Speciality
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        public string NameGenitive { get;  set; }

        public string NamePlural { get; set; }

        public string NamePluralGenitive { get; set; }

        public bool IsSimpe { get; set; }

        public string BranchName { get; set; }

        public string BranchAlias { get; set; }
    }
}