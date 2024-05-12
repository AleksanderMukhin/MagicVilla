using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberServices _villaNumberServices;
    private readonly IMapper _mapper;
    // GET
    public VillaNumberController(IVillaNumberServices villaNumberServices, IMapper mapper)
    {
        _villaNumberServices = villaNumberServices;
        _mapper = mapper;
    }

    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNomberDTO> list = new();

        var response = await _villaNumberServices.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNomberDTO>>(Convert.ToString(response.Result));
        }
        return View(list);
    }
}