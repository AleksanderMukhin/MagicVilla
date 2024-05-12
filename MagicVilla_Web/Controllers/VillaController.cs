using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaServices _villaServices;
    private readonly IMapper _mapper;

    public VillaController(IVillaServices villaServices, IMapper mapper)
    {
        _villaServices = villaServices;
        _mapper = mapper;
    }
    
    // GET
    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDto> list = new();

        var response = await _villaServices.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
        }
        return View(list);
    }
    
    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaServices.CreateAsync<APIResponse>(model);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        
        return View(model);
    }
    
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var response = await _villaServices.GetAsync<APIResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaUpdateDto>(model));
        }
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaServices.UpdateAsync<APIResponse>(model);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        
        return View(model);
    }
    
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaServices.GetAsync<APIResponse>(villaId);
        if (response != null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
            return View(model);
        }
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDto model)
    {
            var response = await _villaServices.DeleteAsync<APIResponse>(model.Id);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
            
        return View(model);
    }
}