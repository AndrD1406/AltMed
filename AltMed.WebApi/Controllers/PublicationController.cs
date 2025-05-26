using AltMed.BusinessLogic.Dtos;
using AltMed.BusinessLogic.Services;
using AltMed.BusinessLogic.Services.Interfaces;
using AltMed.DataAccess.Identity;
using AltMed.DataAccess.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AltMed.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PublicationController : ControllerBase
{
    private readonly IPublicationService publicationService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMapper mapper;

    public PublicationController(IPublicationService publicationService, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        this.publicationService = publicationService;
        this.mapper = mapper;
        this.userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Publication), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] PublicationCreateDto publication)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var created = await publicationService.Create(publication, Guid.Parse(userId));

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Publication), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var publication = await publicationService.GetById(id);
        if (publication == null)
            return NotFound();

        return Ok(publication);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<Publication>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var publications = await publicationService.GetAll();
        return Ok(publications);
    }

    [HttpGet()]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PublicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWithDetails()
    {
        var dtos = await publicationService.GetAllWithDetails();
        return Ok(dtos);
    }

    [HttpPut]
    [ProducesResponseType(typeof(LikeDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Like(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var likeDto = await publicationService.SetLike(id, userId);

        return Ok(likeDto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PublicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid id)
    {
        var publications = await this.publicationService.GetPublicationsByAuthor(id);
        return Ok(publications);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();

        var dto = mapper.Map<ProfileDto>(user);

        return Ok(dto);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Comment([FromBody] CommentCreateDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdStr == null || !Guid.TryParse(userIdStr, out var currentUserId))
            return Unauthorized();

        if (dto.AuthorId != currentUserId)
            return Forbid();

        var comment = await publicationService.AddComment(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = comment.Id },
            comment
        );
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PublicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWithDetailsPaged(
    [FromQuery] int skip = 0,
    [FromQuery] int take = 10)
    {
        var page = await publicationService.GetWithDetailsPaged(skip, take);
        return Ok(page);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PublicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserIdPaged(
        [FromQuery] Guid id,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10)
    {
        var page = await publicationService.GetPublicationsByAuthorPaged(id, skip, take);
        return Ok(page);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PublicationDto>), StatusCodes.Status200OK)]
    public Task<IEnumerable<PublicationDto>> Search(string query, int skipCount = 0, int maxResultCount = 10)
    => publicationService.SearchAsync(query, skipCount, maxResultCount);

    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<PublicationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PublicationDto>> Update(Guid id, [FromBody] PublicationCreateDto dto)
    {
        var updated = await publicationService.Update(id, dto);
        return Ok(updated);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await publicationService.Delete(id);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsForPublication(Guid id)
    {
        var comments = await publicationService.GetCommentsForPublication(id);
        return Ok(comments);
    }
}
