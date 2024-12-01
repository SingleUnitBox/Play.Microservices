using Play.User.Service.DTO;

namespace Play.User.Service.Auth;

public interface IAuthService
{
    Task SignUp(SignUpDto dto);
    Task<JwtToken> SignIn(SignInDto dto);
    Task<AccountDto> GetAccount(Guid userId);
}