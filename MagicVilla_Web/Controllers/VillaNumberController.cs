using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberServices _villaNumberServices;
    private readonly IVillaServices _villaServices;
    private readonly IMapper _mapper;
    // GET
    public VillaNumberController(IVillaServices villaServices, IVillaNumberServices villaNumberServices, IMapper mapper)
    {
        _villaServices = villaServices;
        _villaNumberServices = villaNumberServices;
        _mapper = mapper;
    }

    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNomberDTO> list = new();

        var response = await _villaNumberServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNomberDTO>>(Convert.ToString(response.Result));
        }
        return View(list);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVM villaNumberVM = new();
        var response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        return View(villaNumberVM);
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberServices.CreateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if (response.ErrorMessages.Count>0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
        }
        
        var resp = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (resp != null && resp.IsSuccess)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(resp.Result)).Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        VillaNumberUpdateVM villaNumberVM = new();
        var response = await _villaNumberServices.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            VillaNomberDTO model = JsonConvert.DeserializeObject<VillaNomberDTO>(Convert.ToString(response.Result));
            villaNumberVM.VillaNumber=_mapper.Map<VillaNomberUpdateDTO>(model);
        }
        
        response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(villaNumberVM);
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberServices.UpdateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
        }
        
        var resp = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (resp != null && resp.IsSuccess)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(resp.Result)).Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        VillaNumberDeleteVM villaNumberVM = new();
        var response = await _villaNumberServices.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            VillaNomberDTO model = JsonConvert.DeserializeObject<VillaNomberDTO>(Convert.ToString(response.Result));
            villaNumberVM.VillaNumber=_mapper.Map<VillaNomberDTO>(model);
        }
        
        response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(villaNumberVM);
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
    {
        var response = await _villaNumberServices.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccess)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }
            
        return View(model);
    }
}