namespace AuctionHouse.AuthenticationService.API.Interface;

public class TokenConfig
{
    public string Key { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
}