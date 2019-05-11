using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HClinic : IHEntity
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string ShortName { get; set; }
        
        [MaxLength(200)]
        public string RewriteName { get; set; }
        
        public string Url { get; set; }
        
        public long HCityId { get; set; }
        public HCity HCity { get; set; }
        
        public long HStreetId { get; set; }
        public HStreet HStreet { get; set; }
        
        public string Description { get; set; }
        
        public string House { get; set; }
        
        [MaxLength(20)]
        public string Phone { get; set; }
        
        public string Logo { get; set; }
        
        public long HDistrictId { get; set; }
        public HDistrict HDistrict { get; set; }
        
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        
        public long? ParentId { get; set; }
        public HClinic Parent { get; set; }
        
        public bool OnlineRecordDoctor { get; set; }
        public bool IsActive { get; set; }
        
        public long OriginId { get; set; }
    }
}