using Microsoft.EntityFrameworkCore;
using Snipper_Snippets_API.Data;
using Snipper_Snippets_API.Models;

namespace Snipper_Snippets_API.Services
{
    public class AuthService
    {
        private readonly SnipperDbContext _context;
        public AuthService(SnipperDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 10);

            var user = new User
            {
                Email = request.Email,
                Password = hashedPassword
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new UserResponse { Id = user.Id, Email = user.Email };
        }

        public async Task<UserResponse?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
                return null;

            var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return isValidPassword ? new UserResponse { Id = user.Id, Email = user.Email } : null;
        }
    }
}