using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository;

public interface INomberRepository:IRepository<VillaNomber>
{
    Task<VillaNomber> UpdateAsync(VillaNomber entity);
    Task<VillaNomber> CreateAsync(VillaNomber entity);
}