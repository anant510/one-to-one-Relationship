namespace relationshipOnetoone.models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        // Navigation property
        public UserProfile? UserProfile { get; set; }
    }
}
