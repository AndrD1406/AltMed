using AltMed.BusinessLogic.Dtos;
using AltMed.BusinessLogic.Services.Interfaces;
using AltMed.DataAccess.Models;
using AltMed.DataAccess.Repository.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Services;

public class PublicationService : IPublicationService
{
    private readonly IEntityRepository<Guid, Publication> repository;
    private readonly IMapper mapper;

    public PublicationService(IEntityRepository<Guid, Publication> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<Publication> Create(PublicationCreateDto dto, Guid authorId)
    {
        var publication = mapper.Map<Publication>(dto);
        publication.AuthorId = authorId;
        publication.PostedAt = DateTime.UtcNow;

        return await repository.Create(publication);
    }

    public Task<Publication> GetById(Guid id)
    {
        return repository.GetByIdWithDetails(id, "Author,Likes,Comments");
    }
}
