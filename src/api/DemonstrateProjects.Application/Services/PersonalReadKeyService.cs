using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using DemonstrateProjects.Application.ViewModels;
using DemonstrateProjects.Core.Interfaces;

namespace DemonstrateProjects.Application.Services;

public class PersonalReadKeyService : IPersonalReadKeyService
{
    private readonly IUnitOfWork _unitOfWork;
    public PersonalReadKeyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<Guid> CreateAsync(Guid userId, NewPersonalReadKeyModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<PersonalReadKeyModel>> GetFromUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<PersonalReadKeyModel?> GetAsync(Guid key)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid key)
    {
        throw new NotImplementedException();
    }
}