using Microsoft.AspNetCore.Identity;
using Play.Common;
using Play.User.Service.DTO;

namespace Play.User.Service.Auth;

public class AuthService : IAuthService
{
    private readonly IRepository<Entities.User> _userRepository;
    private readonly IPasswordHasher<Entities.User> _passwordHasher;
    private readonly IJwtManager _jwtManager;

    public AuthService(IRepository<Entities.User> userRepository,
        IPasswordHasher<Entities.User> passwordHasher,
        IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtManager = jwtManager;
    }

    public async Task SignUp(SignUpDto dto)
    {
        var emptyClaims = new Dictionary<string, IEnumerable<string>>();
        var user = await _userRepository.GetAsync(u => u.Email == dto.Email);
        if (user is not null)
        {
            return;
        }
        
        var hashedPassword = _passwordHasher.HashPassword(default, dto.Password);
        user = new Entities.User(
            dto.Email, 
            dto.Email, 
            hashedPassword, 
            string.IsNullOrWhiteSpace(dto.Role) ? "user" : dto.Role, 
            dto.Claims is null ? emptyClaims : dto.Claims);
        
        await _userRepository.CreateAsync(user);
    }

    public async Task<JwtToken> SignIn(SignInDto dto)
    {
        var user = await _userRepository.GetAsync(u => u.Email == dto.Email);
        if (user is null)
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        var isPasswordVerified = _passwordHasher.VerifyHashedPassword(default, user.Password, dto.Password)
                                 == PasswordVerificationResult.Success;
        if (isPasswordVerified is false)
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        var token = _jwtManager.GenerateToken(user.Id.ToString(), user.Role, user.Claims);
        return token;
    }

    public async Task<AccountDto> GetAccount(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is not null)
        {
            return new AccountDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Claims = user.Claims,
            };
        }

        return null;
    }
}