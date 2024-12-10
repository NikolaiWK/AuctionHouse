using Microsoft.AspNetCore.Identity;

namespace AuctionHouse.AuthenticationService.Domain.Entities
{
    public class User : IdentityUser<int>
    {
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public UserAddress Address { get; set; }
       public string ProfileImageURL { get; set; }

    }
}
