namespace SchoolWebAPI.Entities.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedTimeUtc { get; set; }                       
    }
}
