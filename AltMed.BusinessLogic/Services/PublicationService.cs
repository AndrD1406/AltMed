using AltMed.BusinessLogic.Dtos;
using AltMed.BusinessLogic.Services.Interfaces;
using AltMed.DataAccess.Enums;
using AltMed.DataAccess.Models;
using AltMed.DataAccess.Repository.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Services;

public class PublicationService : IPublicationService
{
    private readonly IEntityRepository<Guid, Publication> publicationRepository;
    private readonly IEntityRepository<Guid, Like> likeRepository;
    private readonly IMapper mapper;

    public PublicationService(
        IEntityRepository<Guid, Publication> publicationRepository,
        IEntityRepository<Guid, Like> likeRepository,
        IMapper mapper)
    {
        this.publicationRepository = publicationRepository;
        this.likeRepository = likeRepository;
        this.mapper = mapper;
    }

    public async Task<PublicationDto> Create(PublicationCreateDto dto, Guid authorId)
    {
        var publication = mapper.Map<Publication>(dto);
        publication.AuthorId = authorId;
        publication.PostedAt = DateTime.UtcNow;
        var created = await publicationRepository.Create(publication);
        return mapper.Map<PublicationDto>(created);
    }

    public async Task<PublicationDto> GetById(Guid id)
    {
        var pub = await publicationRepository.GetById(id);
        return mapper.Map<PublicationDto>(pub);
    }

    public async Task<IEnumerable<PublicationDto>> GetAll()
    {
        var publications = await publicationRepository.GetAll();
        return mapper.Map<IEnumerable<PublicationDto>>(publications);
    }

    public async Task<LikeDto> GetLikeById(Guid id)
    {
        var like = await likeRepository.GetById(id);
        return mapper.Map<LikeDto>(like);
    }

    public async Task<IEnumerable<LikeDto>> GetLikesForPublication(Guid publicationId)
    {
        var likes = (await likeRepository.GetAll())
            .Where(l => l.PublicationId == publicationId);
        return mapper.Map<IEnumerable<LikeDto>>(likes);
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsForPublication(Guid publicationId)
    {
        var comments = (await publicationRepository.GetAll())
            .Where(p => p.ParentId == publicationId);
        return mapper.Map<IEnumerable<CommentDto>>(comments);
    }

    public async Task<IEnumerable<PublicationDto>> GetPublicationsByAuthor(Guid authorId)
    {
        var pubs = (await publicationRepository.GetAll())
            .Where(p => p.AuthorId == authorId && p.ParentId == null);
        return mapper.Map<IEnumerable<PublicationDto>>(pubs);
    }

    public async Task<IEnumerable<PublicationDto>> GetReplies(Guid parentId)
    {
        var replies = (await publicationRepository.GetAll())
            .Where(p => p.ParentId == parentId);
        return mapper.Map<IEnumerable<PublicationDto>>(replies);
    }

    public async Task<PublicationDto> Update(Guid id, PublicationCreateDto dto)
    {
        var existing = await publicationRepository.GetById(id);
        mapper.Map(dto, existing);
        var updated = await publicationRepository.Update(existing);
        return mapper.Map<PublicationDto>(updated);
    }

    public async Task Delete(Guid id)
    {
        var publicationToDelete = await this.publicationRepository.GetById(id);

        await publicationRepository.Delete(publicationToDelete);
    }

    public async Task<LikeDto> SetLike(Guid publicationId, Guid authorId)
    {
        var existing = (await likeRepository.GetAll())
            .FirstOrDefault(l
                => l.PublicationId == publicationId
                && l.AuthorId == authorId);

        if (existing != null)
        {
            existing.IsLiked = !existing.IsLiked;
            var updated = await likeRepository.Update(existing);
            return mapper.Map<LikeDto>(updated);
        }

        var like = new Like
        {
            PublicationId = publicationId,
            AuthorId = authorId,
            IsLiked = true
        };

        var created = await likeRepository.Create(like);
        return mapper.Map<LikeDto>(created);
    }

    public async Task<IEnumerable<LikeDto>> GetAllLikes()
    => mapper.Map<IEnumerable<LikeDto>>(await likeRepository.GetAll());

    public async Task<IEnumerable<PublicationDto>> GetAllWithDetails()
    {
        var publications = await publicationRepository.GetAll();

        var allLikes = (await likeRepository.GetAll())
            .Select(l => mapper.Map<LikeDto>(l))
            .ToLookup(l => l.PublicationId);

        var allComments = publications
            .Where(p => p.ParentId.HasValue)
            .Select(p => mapper.Map<CommentDto>(p))
            .ToLookup(c => c.ParentId);

        var result = publications.Select(pub =>
        {
            var dto = mapper.Map<PublicationDto>(pub);
            dto.Likes = allLikes[pub.Id].ToList();
            dto.Comments = allComments[pub.Id].ToList();
            return dto;
        });

        return result;
    }

    public async Task<CommentDto> AddComment(CommentCreateDto dto)
    {
        var commentEntity = mapper.Map<Publication>(dto);

        var created = await publicationRepository.Create(commentEntity);

        return mapper.Map<CommentDto>(created);
    }

    private async Task<IEnumerable<PublicationDto>> MapWithLikesAndComments(
        List<Publication> pageEntities)
    {
        var dtos = pageEntities.Select(mapper.Map<PublicationDto>).ToList();

        var allLikes = (await likeRepository.GetAll())
            .Select(mapper.Map<LikeDto>)
            .Where(l => pageEntities.Select(p => p.Id).Contains(l.PublicationId))
            .ToLookup(l => l.PublicationId);

        var commentEntities = await publicationRepository
        .GetByFilterNoTracking(
            p => p.ParentId.HasValue
                 && pageEntities.Select(x => x.Id).Contains(p.ParentId.Value),
            includeProperties: nameof(Publication.Author)
        )
        .ToListAsync();

        var allComments = commentEntities
            .Select(mapper.Map<CommentDto>)
            .ToLookup(c => c.ParentId);

        foreach (var dto in dtos)
        {
            dto.Likes = allLikes[dto.Id].Where(l => l.IsLiked).ToList();
            dto.Comments = allComments[dto.Id].ToList();
        }

        return dtos;
    }

    public async Task<IEnumerable<PublicationDto>> GetWithDetailsPaged(int skip, int take)
    {
        var pageEntities = await publicationRepository
            .Get(
                skip: skip,
                take: take,
                includeProperties: nameof(Publication.Author),
                whereExpression: p => p.ParentId == null,
                orderBy: new Dictionary<Expression<Func<Publication, object>>, SortDirection>
                {
                    { p => p.PostedAt, SortDirection.Descending }
                },
                asNoTracking: true
            )
            .ToListAsync();

        return await MapWithLikesAndComments(pageEntities);
    }

    public async Task<IEnumerable<PublicationDto>> GetPublicationsByAuthorPaged(
        Guid authorId, int skip, int take)
    {
        var pageEntities = await publicationRepository
            .Get(
                skip: skip,
                take: take,
                includeProperties: nameof(Publication.Author),
                whereExpression: p => p.ParentId == null && p.AuthorId == authorId,
                orderBy: new Dictionary<Expression<Func<Publication, object>>, SortDirection>
                {
                    { p => p.PostedAt, SortDirection.Descending }
                },
                asNoTracking: true
            )
            .ToListAsync();

        return await MapWithLikesAndComments(pageEntities);
    }

    public async Task<IEnumerable<PublicationDto>> SearchAsync(string query, int skipCount, int maxResultCount)
    {
        var lowerQuery = (query ?? string.Empty).ToLower();

        var entities = await publicationRepository
            .Get(
                skip: skipCount,
                take: maxResultCount,
                whereExpression: p => p.Content.ToLower().Contains(lowerQuery),
                orderBy: new Dictionary<Expression<Func<Publication, object>>, SortDirection>
                {
                { p => p.PostedAt, SortDirection.Descending }
                })
            .ToListAsync();

        // 3) map to DTOs
        return entities
            .Select(p => mapper.Map<PublicationDto>(p))
            .ToList();
    }

}


