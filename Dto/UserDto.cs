namespace relationshipOnetoone.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public UserProfileDto? UserProfile { get; set; }
    }
}
