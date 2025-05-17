using AltMed.DataAccess.Models;
using AltMed.DataAccess.Repository.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Services;

public class PublicationService
{
    private readonly IEntityRepository<Guid, Publication> repository;
    private readonly IMapper mapper;

    public PublicationService(IEntityRepository<Guid, Publication> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    //public 
}
