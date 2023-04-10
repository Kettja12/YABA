namespace ServicesShared;
public class LoginUserResponse : BaseResponse
{
    public LoginUser LoginUser { get; set; } = new();
}