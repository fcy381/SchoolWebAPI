namespace SchoolWebAPI.Entities.Base
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedTimeUtc { get; set; }                       
    }
}
