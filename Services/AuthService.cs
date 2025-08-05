using Snipper_Snippets_API.Models;

namespace Snipper_Snippets_API.Services
{
    public class AuthService
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        public UserResponse CreateUser(CreateUserRequest request)
        {
            if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 10);

            var user = new User
            {
                Id = _nextId++,
                Email = request.Email,
                Password = hashedPassword
            };

            _users.Add(user);

            return new UserResponse { Id = user.Id, Email = user.Email };
        }

        public UserResponse? AuthenticateUser(string email, string password)
        {
            var user = _users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return null;

            var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return isValidPassword ? new UserResponse { Id = user.Id, Email = user.Email } : null;
        }
    }
}