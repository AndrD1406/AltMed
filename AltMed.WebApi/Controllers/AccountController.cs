using AltMed.BusinessLogic.Dtos.Identity;
using AltMed.BusinessLogic.Services.Interfaces;
using AltMed.DataAccess.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AltMed.WebApi.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly RoleManager<ApplicationRole> roleManager;
    private readonly IJwtService jwtService;
    private readonly IMapper mapper;

    public AccountController(UserManager<ApplicationUser> userMng,
        SignInManager<ApplicationUser> signInMng, RoleManager<ApplicationRole> roleMng, IJwtService jwtSvc, IMapper mapp)
    {
        userManager = userMng;
        signInManager = signInMng;
        roleManager = roleMng;
        jwtService = jwtSvc;
        mapper = mapp;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // Validation 
        if (!ModelState.IsValid)
        {
            string errorMessages = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
            return Problem(errorMessages);
        }

        // Create user
        ApplicationUser user = mapper.Map<ApplicationUser>(registerDto);

        IdentityResult result = null;
        try
        {
            result = await userManager.CreateAsync(user, registerDto.Password);
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
        }

        if (result.Succeeded)
        {
            // sign-in
            // isPersister: false - must be deleted automatically when the browser is closed
            await signInManager.SignInAsync(user, isPersistent: false);

            var authenticationResponse = jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;

            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
        return Problem(errorMessage);
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

    [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // Validation 
        if (!ModelState.IsValid)
        {
            string errorMessages = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
            return BadRequest(errorMessages);
        }

        var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return NoContent();

            await signInManager.SignInAsync(user, isPersistent: false);

            var authenticationResponse = jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;

            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            try
            {
                await userManager.UpdateAsync(user);
            }
            catch (Exception exc)
            {
                Console.WriteLine($"await userManager.UpdateAsync(user): {exc.Message}");
            }

            return Ok(authenticationResponse);
        }
        return BadRequest("Invalid email or password");
    }

    [HttpGet("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        Response.Cookies.Delete("refreshToken");

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

        AuthenticationResponse authenticationResponse = jwtService.CreateJwtToken(user);

        user.RefreshToken = authenticationResponse.RefreshToken;
        user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;

        await userManager.UpdateAsync(user);

        return Ok(authenticationResponse);
    }
}
