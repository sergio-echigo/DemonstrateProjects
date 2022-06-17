using DemonstrateProjects.Application.InputModels;
using DemonstrateProjects.Application.Services.Interfaces;
using DemonstrateProjects.Application.ViewModels;
using DemonstrateProjects.Core.Entities;
using DemonstrateProjects.Core.Interfaces;

namespace DemonstrateProjects.Application.Services;

public class PersonalReadKeyService : IPersonalReadKeyService
{
    private readonly IUnitOfWork _unitOfWork;
    public PersonalReadKeyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> CreateAsync(Guid userId, NewPersonalReadKeyModel model)
    {
        PersonalReadKey entity = new()
        {
            UserId = userId,
            ExpiresWhen = model.ExpiresWhen
        };

        await _unitOfWork.PersonalReadKeys.Add(entity);
        await _unitOfWork.SaveChangesAsync();

        /* This value is automatically assigned as Guid.NewGuid()! See Entities! */
        return entity.Key;
    }

    public async Task<IQueryable<PersonalReadKeyModel>> GetFromUserAsync(Guid userId)
    {
        return (await _unitOfWork.PersonalReadKeys.GetByUserIdAsync(userId))
            .Select(x => new PersonalReadKeyModel()
            {
                Key = x.Key,
                UserId = x.UserId,
                ExpiresWhen = x.ExpiresWhen
            });
    }

    public async Task<PersonalReadKeyModel?> GetAsync(Guid key)
    {
        var personalKey = await _unitOfWork.PersonalReadKeys.GetEntityAsync(key);
        
        /* Not required, but only for removing some warnings! And I'm tired of writing "We're verifying..." */
        if (personalKey is null)
            return null;
        
        return new PersonalReadKeyModel()
        {
            Key = personalKey.Key,
            ExpiresWhen = personalKey.ExpiresWhen
        };
    }

    public async Task DeleteAsync(Guid key)
    {
        var personalKey = await _unitOfWork.PersonalReadKeys.GetEntityAsync(key);
        
        /* Not required, but only for removing some warnings! And I'm tired of writing "We're verifying..." */
        if (personalKey is null)
            return;
        
        await _unitOfWork.PersonalReadKeys.DeleteAsync(key);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAllAsync(Guid userId)
    {
        _unitOfWork.Projects.DeleteAllAsync(userId);
        await _unitOfWork.SaveChangesAsync();
    }
}