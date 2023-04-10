namespace ServicesShared;
public class SimpleRequest:AuthTokenRequest
{
    public string? StringField { get; set; }
    public int IntField { get; set; }
    public double DoubleField { get; set; }
}
