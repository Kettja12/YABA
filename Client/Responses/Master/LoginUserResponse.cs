namespace Client.Responses.Master;
public class LoginUserResponse : BaseResponse
{
    public LoginUser LoginUser { get; set; } = new();
}