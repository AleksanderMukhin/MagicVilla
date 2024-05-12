using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNumberServices
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int id);
    Task<T> CreateAsync<T>(VillaNomberCreateDTO dto);
    Task<T> UpdateAsync<T>(VillaNomberUpdateDTO dto);
    Task<T> DeleteAsync<T>(int id);
}