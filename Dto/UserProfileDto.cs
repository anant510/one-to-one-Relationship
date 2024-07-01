namespace relationshipOnetoone.Dto
{
    public class UserProfileDto
    {
        public int UserProfileId { get; set; }
        public string FullName { get; set; }

        public int UserId { get; set; }
        //public ICollection<UserDto> users { get; set; }
    }
}
