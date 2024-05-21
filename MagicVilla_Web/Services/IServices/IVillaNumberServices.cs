using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNumberServices
{
    Task<T> GetAllAsync<T>(string token);
    Task<T> GetAsync<T>(int id, string token);
    Task<T> CreateAsync<T>(VillaNomberCreateDTO dto, string token);
    Task<T> UpdateAsync<T>(VillaNomberUpdateDTO dto, string token);
    Task<T> DeleteAsync<T>(int id, string token);
}