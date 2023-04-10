
using ServicesShared;

namespace Master;

public class SeUserDatabaseRequest:AuthTokenRequest
{
    required public string DatabaseName { get; set; }
}
