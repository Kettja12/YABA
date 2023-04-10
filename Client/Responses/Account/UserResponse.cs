namespace Client.Responses.Account;

public class UserResponse:BaseResponse
{
    public UserWithRights User { get; set; } = new();
}
