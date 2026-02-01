using AltWirePoint.BusinessLogic.Models.Identity;
using AltWirePoint.BusinessLogic.Models.Profile;
using AltWirePoint.BusinessLogic.Services.Interfaces;
using AltWirePoint.DataAccess.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AltWirePoint.WebApi.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IJwtService jwtService;
    private readonly IMapper mapper;

    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IJwtService jwtService, IMapper mapp)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.jwtService = jwtService;
        mapper = mapp;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = mapper.Map<ApplicationUser>(registerRequest);

        var result = await userManager.CreateAsync(user, registerRequest.Password);

        if (result.Succeeded)
        {
            var authenticationResponse = await jwtService.CreateJwtToken(user);

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        return BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser? user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Ok(true);
        }
        return Ok(false);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null) return BadRequest("Invalid email or password");

        var result = await signInManager.CheckPasswordSignInAsync(
            user,
            loginRequest.Password,
            lockoutOnFailure: true
        );

        if (result.Succeeded)
        {
            var authenticationResponse = await jwtService.CreateJwtToken(user);

            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        if (result.IsLockedOut)
        {
            return BadRequest("Account is locked out. Try again later.");
        }

        return BadRequest("Invalid email or password");
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpirationDateTime = DateTime.MinValue;
                await userManager.UpdateAsync(user);
            }
        }

        return NoContent();
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh(TokenModel tokenModel)
    {
        if (tokenModel == null)
        {
            return BadRequest("Invalid client request");
        }

        string? token = tokenModel.Token;
        string? refreshToken = tokenModel.RefreshToken;

        ClaimsPrincipal? principal = jwtService.GetPrincipalFromJwtToken(token);
        if (principal == null)
        {
            return BadRequest("Invalid access token");
        }

        string? email = principal.FindFirstValue(ClaimTypes.Email);

        ApplicationUser? user = await userManager.FindByEmailAsync(email);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid refresh token");
        }

        AuthenticationResponse authenticationResponse = await jwtService.CreateJwtToken(user);

        user.RefreshToken = authenticationResponse.RefreshToken;
        user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;

        await userManager.UpdateAsync(user);

        return Ok(authenticationResponse);
    }

    [HttpPost("[action]")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (request.NewPassword != request.ConfirmPassword)
            return BadRequest("New password and confirmation do not match.");

        var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userIdClaim == null) return Unauthorized();
        var user = await userManager.FindByIdAsync(userIdClaim);
        if (user == null) return NotFound();

        var result = await userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword
        );

        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        return NoContent();
    }

    [HttpPut("[action]")]
    [Authorize]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditProfile([FromBody] ProfileEditRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(request.Name))
            user.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.Logo))
            user.Logo = request.Logo;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        var profileDto = mapper.Map<ProfileDto>(user);
        return Ok(profileDto);
    }
}
