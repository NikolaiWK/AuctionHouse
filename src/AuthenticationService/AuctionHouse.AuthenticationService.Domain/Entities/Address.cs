namespace AuctionHouse.AuthenticationService.Domain.Entities;

public class UserAddress
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
}