using AltMed.BusinessLogic.Dtos;
using AltMed.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Services.Interfaces;

public interface IPublicationService
{
    Task<Publication> Create(PublicationCreateDto dto, Guid authorId);

    Task<Publication> GetById(Guid id);
}
