# Snipper Snippets API

This .NET API demonstrates two important security measures:

- encrypting snippet code before saving to the database
- creating user accounts with password hashing and Basic Auth

We have implemented a secure code snippet sharing platform with user authentication.

## Coach notes

The big concepts at play are:

- [AES encryption](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes) for data at rest
- [BCrypt password hashing](https://github.com/BcryptNet/bcrypt.net) for secure authentication
- [Basic Auth protocol](https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication#basic_authentication_scheme) for user credentials
- [.NET User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) for secure configuration management

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [OpenSSL](https://www.openssl.org/) for key generation (or use Git Bash on Windows)

## Getting Started

### 1. Clone and Setup

```bash
git clone <your-repo-url>
cd Snipper-Snippets-API
dotnet restore
```

### 2. Generate Encryption Key

Generate a secure 32-byte encryption key using OpenSSL:

```bash
openssl rand -hex 32
```

This will give you something like:
```
a7f4c2e8d9b1f6a3c5e7d2f8b4a6c9e1f3a5c7e9b2d4f6a8c1e3d5f7b9a2c4e6
```

(You will notice that this is 64 characters long: 1 byte in hex is represented by a pair of characters.)

### 3. Store the Key Securely

**For Development (User Secrets):**
```bash
dotnet user-secrets set "ENCRYPTION_KEY" "your-generated-key-here"
```

**For Production (Environment Variables):**
```bash
# Linux/Mac
export ENCRYPTION_KEY="your-generated-key-here"

# Windows
set ENCRYPTION_KEY=your-generated-key-here
```

**Verify your secret is set:**
```bash
dotnet user-secrets list
```

### 4. Run the Application

```bash
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5080`
- **HTTPS**: `https://localhost:5081`
- **Swagger UI**: `https://localhost:5081/swagger` (development only)

## API Endpoints

### Authentication

#### Create a User (Sign Up)
```bash
curl -v -X POST \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "password123"
  }' \
  'http://localhost:5080/user'
```

**Note**: User creation sends email and password in the request body. No authorization header needed for registration.

#### Get Current User (Protected)
```bash
curl -v -X GET \
  -H "Authorization: Basic $(echo -n 'user@example.com:password123' | base64)" \
  'http://localhost:5080/user'
```

### Snippets

#### Create a Snippet (Protected)
```bash
curl -v -X POST \
  -H "Content-Type: application/json" \
  -H "Authorization: Basic $(echo -n 'user@example.com:password123' | base64)" \
  -d '{
    "language": "csharp",
    "code": "Console.WriteLine(\"Hello, World!\");"
  }' \
  'http://localhost:5080/snippets'
```

#### Get All Snippets (Protected)
```bash
curl -v -X GET \
  -H "Authorization: Basic $(echo -n 'user@example.com:password123' | base64)" \
  'http://localhost:5080/snippets'
```

#### Filter Snippets by Language
```bash
curl -v -X GET \
  -H "Authorization: Basic $(echo -n 'user@example.com:password123' | base64)" \
  'http://localhost:5080/snippets?lang=csharp'
```

#### Get Snippet by ID (Protected)
```bash
curl -v -X GET \
  -H "Authorization: Basic $(echo -n 'user@example.com:password123' | base64)" \
  'http://localhost:5080/snippets/1'
```

## Security Features

### 🔐 Data Encryption

All snippet code is encrypted using **AES-256-CBC** before being stored. You can verify this by:

1. Adding console logging in `SnippetService.AddSnippet` to see the encrypted data
2. Checking that the stored data is different from the input
3. Verifying that the API returns decrypted, readable code

**Try this:**
- Create a snippet with the code `"Hello, World!"`
- Check the console logs to see the encrypted version stored
- Notice the API returns the original, decrypted code

### 🔒 Password Security

User passwords are hashed using **BCrypt** with 10 salt rounds before storage:

- Original passwords are never stored
- Each password gets a unique salt
- Timing-attack resistant verification
- Industry-standard security practices

### 🛡️ Basic Authentication

The API uses HTTP Basic Auth:

- Standard `Authorization: Basic <base64(email:password)>` headers
- Middleware validates credentials on every request
- Users can only access their own snippets
- Protected endpoints return 401 for invalid credentials

## Testing Authentication

### Valid Credentials
```bash
# This should work (after creating the user)
curl -v -X GET \
  -H "Authorization: Basic $(echo -n 'user@example.com:password123' | base64)" \
  'http://localhost:5080/user'
```

### Missing Authorization
```bash
# This should return 401 Unauthorized
curl -v -X GET 'http://localhost:5080/user'
```

### Wrong Password
```bash
# This should return 401 Unauthorized
curl -v -X GET \
  -H "Authorization: Basic $(echo -n 'user@example.com:wrongpassword' | base64)" \
  'http://localhost:5080/user'
```

## Project Structure

```
Snipper-Snippets-API/
├── Controllers/
│   ├── SnippetsController.cs    # Snippet CRUD operations
│   └── UserController.cs        # User authentication
├── Models/
│   ├── Snippet.cs              # Snippet data models
│   └── User.cs                 # User data models and DTOs
├── Services/
│   ├── SnippetService.cs       # Snippet business logic
│   ├── AuthService.cs          # Authentication logic
│   └── EncryptionService.cs    # AES encryption/decryption
├── Middleware/
│   └── BasicAuthMiddleware.cs  # Basic Auth implementation
├── Program.cs                  # Application configuration
├── appsettings.json           # Configuration (no secrets!)
└── Snipper-Snippets-API.csproj # Project file with dependencies
```

## Configuration Management

### Development
- Uses [.NET User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- Secrets stored outside project directory
- Never committed to source control

### Production
- Uses environment variables
- Compatible with Azure App Service, AWS, Docker, etc.
- Follows cloud security best practices

### Configuration Priority (highest to lowest)
1. Environment Variables (production)
2. User Secrets (development)
3. appsettings.{Environment}.json
4. appsettings.json

## Key Dependencies

- **BCrypt.Net-Next**: Password hashing and verification
- **Microsoft.AspNetCore.Authentication**: Authentication framework
- **System.Security.Cryptography**: AES encryption (built-in)

## Security Best Practices Implemented

✅ **Encryption at Rest**: All snippet code encrypted before database storage  
✅ **Password Hashing**: BCrypt with salt rounds for secure password storage  
✅ **Secure Configuration**: Secrets managed via User Secrets/Environment Variables  
✅ **Authentication**: Standard HTTP Basic Auth with proper middleware  
✅ **User Isolation**: Users can only access their own data  
✅ **Input Validation**: Proper validation and error handling  
✅ **No Password Exposure**: API responses never include password data  

## Next Steps

This project demonstrates enterprise-grade security practices for .NET APIs. Students should:

1. **Understand the Security Model**: Study how encryption and authentication work together
2. **Explore the Code**: Examine how middleware, services, and controllers interact
3. **Test Thoroughly**: Use the provided curl commands to understand the API behavior
4. **Extend Functionality**: Add features like snippet sharing, categories, or search
5. **Deploy Securely**: Practice deploying with proper secret management

Remember: The goal isn't to memorize this implementation, but to understand the security patterns and apply them using your framework's best practices and documentation.

## Troubleshooting

### Common Issues

**"ENCRYPTION_KEY not found"**
```bash
# Make sure you've set the user secret
dotnet user-secrets set "ENCRYPTION_KEY" "your-key-here"
dotnet user-secrets list
```

**401 Unauthorized**
- Check your Base64 encoding: `echo -n 'email:password' | base64`
- Ensure you created the user first with POST /user
- Verify the Authorization header format: `Basic <base64>`

**SSL Certificate Issues (Development)**
```bash
# Trust the development certificate
dotnet dev-certs https --trust
```

For production deployments, use proper SSL certificates from your hosting provider.
