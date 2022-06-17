using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.ViewModels;

namespace DemonstrateProjects.Application.Services.Interfaces;

public interface IPersonalReadKeyService
{
    Task<Guid> CreateAsync(Guid userId, NewPersonalReadKeyModel model);

    Task<IQueryable<PersonalReadKeyModel>> GetFromUserAsync(Guid userId);
    Task<PersonalReadKeyModel?> GetAsync(Guid key);

    Task DeleteAsync(Guid key);
    Task DeleteAllAsync(Guid userId);
}