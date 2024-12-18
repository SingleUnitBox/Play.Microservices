using Play.User.Core.DTO;

namespace Play.User.Core.Auth;

public interface IUserService
{
    Task SignUp(SignUpDto dto);
    Task<JwtToken> SignIn(SignInDto dto);
    Task<AccountDto> GetAccount(Guid userId);
    Task ChangeUsername(Guid userId, ChangeUsernameDto dto);
}