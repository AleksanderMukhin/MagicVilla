using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository;

public class VillaNomberRepository:Repository<VillaNomber>, INomberRepository
{
    private readonly ApplicationDbContext _db;
    public VillaNomberRepository(ApplicationDbContext db): base(db)
    {
        _db = db;
    }

    public async Task<VillaNomber> UpdateAsync(VillaNomber entity)
    {
        entity.UpdatedDate = DateTime.Now;
        _db.VillaNombers.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<VillaNomber> CreateAsync(VillaNomber entity)
    {
        entity.CreatedDate = DateTime.Now;
        await _db.VillaNombers.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}