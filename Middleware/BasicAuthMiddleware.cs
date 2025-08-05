using System.Text;
using Snipper_Snippets_API.Services;

namespace Snipper_Snippets_API.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthService _authService;

        public BasicAuthMiddleware(RequestDelegate next, AuthService authService)
        {
            _next = next;
            _authService = authService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/user") &&
                context.Request.Method == "POST")
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

            if (authHeader == null || !authHeader.StartsWith("Basic "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            try
            {
                var encodedCredentials = authHeader["Basic ".Length..];
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var credentials = decodedCredentials.Split(':');

                if (credentials.Length != 2)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid credentials format");
                    return;
                }

                var email = credentials[0];
                var password = credentials[1];

                var user = _authService.AuthenticateUser(email, password);
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid credentials");
                    return;
                }

                context.Items["User"] = user;
                await _next(context);
            }
            catch
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }
}