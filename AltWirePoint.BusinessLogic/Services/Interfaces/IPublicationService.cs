using AltWirePoint.BusinessLogic.Models;
using AltWirePoint.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltWirePoint.BusinessLogic.Services.Interfaces;

public interface IPublicationService
{
    Task<PublicationDto> Create(PublicationCreateDto dto, Guid authorId);
    Task<PublicationDto> GetById(Guid id);
    Task<IEnumerable<PublicationDto>> GetAll();
    Task<PublicationDto> Update(Guid id, PublicationCreateDto dto);
    Task Delete(Guid id);
    Task<LikeDto> GetLikeById(Guid id);
    Task<IEnumerable<LikeDto>> GetLikesForPublication(Guid publicationId);
    Task<IEnumerable<CommentDto>> GetCommentsForPublication(Guid publicationId);
    Task<IEnumerable<PublicationDto>> GetPublicationsByAuthor(Guid authorId);
    Task<IEnumerable<PublicationDto>> GetReplies(Guid parentId);
    Task<LikeDto> SetLike(Guid publicationId, Guid authorId);
    Task<IEnumerable<PublicationDto>> GetAllWithDetails();
    Task<CommentDto> AddComment(CommentCreateDto dto);
    Task<IEnumerable<PublicationDto>> GetWithDetailsPaged(int skip, int take);
    Task<IEnumerable<PublicationDto>> GetPublicationsByAuthorPaged(Guid authorId, int skip, int take);
    Task<IEnumerable<PublicationDto>> SearchAsync(string query, int skipCount, int maxResultCount);
}
