using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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

        var response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
        }
        return View(list);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaServices.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa created succesfully";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Error created";
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var response = await _villaServices.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaUpdateDto>(model));
        }
        return NotFound();
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
    {
        if (ModelState.IsValid)
        {
            TempData["success"] = "Villa updated succesfully";
            var response = await _villaServices.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        TempData["error"] = "Error updated";
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaServices.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            VillaDto model = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));
            return View(model);
        }
        return NotFound();
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVilla(VillaDto model)
    {
            var response = await _villaServices.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted succesfully";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Error deleted";    
        return View(model);
    }
}