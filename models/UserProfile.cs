namespace relationshipOnetoone.models
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public string FullName { get; set; }

        // Foreign key property
        public int UserId { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
