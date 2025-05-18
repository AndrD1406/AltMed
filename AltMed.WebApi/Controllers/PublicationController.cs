using AltMed.BusinessLogic.Dtos;
using AltMed.BusinessLogic.Services;
using AltMed.BusinessLogic.Services.Interfaces;
using AltMed.DataAccess.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AltMed.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PublicationController : ControllerBase
{
    private readonly IPublicationService publicationService;

    public PublicationController(IPublicationService publicationService)
    {
        this.publicationService = publicationService;
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
}
