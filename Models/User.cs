namespace Snipper_Snippets_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class CreateUserRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public required string Email { get; set; }
    }
}