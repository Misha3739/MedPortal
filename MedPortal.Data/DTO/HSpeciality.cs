using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HSpeciality : IHEntity
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Alias { get; set; }
        
        public long OriginId { get; set; }

        public string NameGenitive { get; set; }

        public string NamePlural { get; set; }

        public string NamePluralGenitive { get; set; }

        public bool IsSimpe { get; set; }

        public string BranchName { get; set; }

        public string BranchAlias { get; set; }
    }
}