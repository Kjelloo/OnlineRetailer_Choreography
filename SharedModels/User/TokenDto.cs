namespace SharedModels.User
{
    public class TokenDto
    {
        public string JWT { get; set; }
        public UserDto User { get; set; }
    }
}