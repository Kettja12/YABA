using ServicesShared;

namespace Master;
public class SavePasswordRequest:AuthTokenRequest
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
