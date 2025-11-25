using Microsoft.AspNetCore.Identity;
using Play.Common.Abs.Auth;
using Play.Common.Abs.RabbitMq;
using Play.User.Core.DTO;
using Play.User.Core.Repositories;
using UserCreated = Play.User.Core.Events.UserCreated;
using UsernameChanged = Play.User.Core.Events.UsernameChanged;

namespace Play.User.Core.Auth;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<Entities.User> _passwordHasher;
    private readonly IJwtManager _jwtManager;
    private readonly IBusPublisher _busPublisher;

    public UserService(IUserRepository userRepository,
        IPasswordHasher<Entities.User> passwordHasher,
        IJwtManager jwtManager,
        IBusPublisher busPublisher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtManager = jwtManager;
        _busPublisher = busPublisher;
    }

    public async Task SignUp(SignUpDto dto)
    {
        var emptyClaims = new Dictionary<string, IEnumerable<string>>();
        var user = await _userRepository.GetUser(u => u.Email == dto.Email);
        if (user is not null)
        {
            throw new InvalidOperationException("Email in use");
        }
        
        var hashedPassword = _passwordHasher.HashPassword(default, dto.Password);
        user = new Entities.User(
            dto.Username, 
            dto.Email, 
            hashedPassword, 
            string.IsNullOrWhiteSpace(dto.Role) ? "user" : dto.Role, 
            dto.Claims is null ? emptyClaims : dto.Claims);
        
        await _userRepository.CreateUser(user);
        await _busPublisher.PublishAsync(new UserCreated(user.Id, user.Username));
    }

    public async Task<JwtToken> SignIn(SignInDto dto)
    {
        var user = await _userRepository.GetUser(u => u.Email == dto.Email);
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
        var user = await _userRepository.GetUserById(userId);
        if (user is not null)
        {
            return new AccountDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Claims = user.Claims,
                State = user.State
            };
        }

        return null;
    }

    public async  Task<string> GetUserState(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user is null)
        {
            return null;
        }
        
        return user.State;
    }
    
    public async Task ChangeUsername(Guid userId, ChangeUsernameDto dto)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user is null)
        {
            throw new InvalidOperationException("User was not found.");
        }
        
        user.Username = dto.NewUsername;
        await _userRepository.UpdateUser(user);
        await _busPublisher.PublishAsync(new UsernameChanged(user.Id, user.Username));
    }
}