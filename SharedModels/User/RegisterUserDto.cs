namespace SharedModels.User
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string BillingAddress { get; set; }
    }
}