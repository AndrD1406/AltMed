using AltWirePoint.BusinessLogic.Models;
using AltWirePoint.BusinessLogic.Models.Publication;

namespace AltWirePoint.BusinessLogic.Services.Interfaces;

public interface IPublicationService
{
    Task<PublicationDto> Create(PublicationCreateRequest dto, Guid authorId);
    Task<PublicationDto> GetById(Guid id);
    Task<IEnumerable<PublicationDto>> GetAll();
    Task<PublicationDto> Update(Guid id, PublicationCreateRequest dto);
    Task Delete(Guid id);
    Task<LikeDto> GetLikeById(Guid id);
    Task<IEnumerable<LikeDto>> GetLikesForPublication(Guid publicationId);
    Task<IEnumerable<CommentDto>> GetCommentsForPublication(Guid publicationId);
    Task<IEnumerable<PublicationDto>> GetPublicationsByAuthor(Guid authorId);
    Task<IEnumerable<PublicationDto>> GetReplies(Guid parentId);
    Task<LikeDto> SetLike(Guid publicationId, Guid authorId);
    Task<IEnumerable<PublicationDto>> GetAllWithDetails();
    Task<CommentDto> AddComment(CommentCreateRequest dto);
    Task<IEnumerable<PublicationDto>> GetWithDetailsPaged(int skip, int take);
    Task<IEnumerable<PublicationDto>> GetPublicationsByAuthorPaged(Guid authorId, int skip, int take);
    Task<IEnumerable<PublicationDto>> SearchAsync(string query, int skipCount, int maxResultCount);
}
