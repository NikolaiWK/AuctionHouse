using AuctionHouse.AuthenticationService.API.DTO;
using AuctionHouse.AuthenticationService.API.Interface;
using AuctionHouse.AuthenticationService.Domain.Entities;
using AuctionHouse.AuthenticationService.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static AuctionHouse.AuthenticationService.API.DTO.RegisterDto;
using static AuctionHouse.AuthenticationService.API.DTO.UserDto;

namespace AuctionHouse.AuthenticationService.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly AuthDbContext _context;

        public AccountController(UserManager<User> userManager, ITokenService tokenService, AuthDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfileImageURL = ""
               
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "User registered successfully!" });
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                
                
            };
}

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);



            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),

            };
        }
    }
}

