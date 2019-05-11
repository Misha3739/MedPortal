namespace MedPortal.Data.DTO
{
    public interface IHEntity
    {
        long Id { get; set; }
        
        long? OriginId { get; set; }
    }
}