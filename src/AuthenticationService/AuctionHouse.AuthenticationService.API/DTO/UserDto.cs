using Microsoft.AspNetCore.Identity;



namespace AuctionHouse.AuthenticationService.API.DTO
{
    public class UserDto 
    {
        public string Email { get; set; }
        public string Token { get; set; }
        
    }
}
