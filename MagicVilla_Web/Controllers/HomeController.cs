using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class HomeController : Controller
{
    private readonly IVillaServices _villaServices;
    private readonly IMapper _mapper;

    public HomeController(IVillaServices villaServices, IMapper mapper)
    {
        _villaServices = villaServices;
        _mapper = mapper;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        List<VillaDto> list = new();

        var response = await _villaServices.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
        }
        return View(list);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}