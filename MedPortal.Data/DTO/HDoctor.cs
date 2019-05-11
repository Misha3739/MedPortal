using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HDoctor : IHEntity
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public string Rating { get; set; }
        
        public Sex Sex { get; set; }
        
        public string Img { get; set; }
        public string ImgFormat { get; set; }
        public string AddPhoneNumber { get; set; }
        public string Category { get; set; }
        public string Degree { get; set; }
        public string Rank { get; set; }
        public string Description { get; set; }
        public string TextEducation { get; set; }
        public string TextDegree { get; set; }
        public string TextSpec { get; set; }
        public string TextCourse { get; set; }
        public string TextExperience { get; set; }
        
        public long ExperienceYear { get; set; }
        public long Price { get; set; }
        public long? SpecialPrice { get; set; }
        public long Departure { get; set; }
        
        [MaxLength(100)]
        public string Alias { get; set; }
        
        public bool IsActive { get; set; }
        public long KidsReception { get; set; }
        public long OpinionCount { get; set; }
        public string TextAbout { get; set; }
        
        public long? TelemedId { get; set; }
        public HTelemed Telemed { get; set; }
        public string RatingReviewsLabel { get; set; }
        public bool IsExclusivePrice { get; set; }
        
        public long? OriginId { get; set; }
    }
}