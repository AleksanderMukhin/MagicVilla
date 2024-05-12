using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/VillaAPI")]
// Когда контроллер помечен атрибутом [ApiController], ASP.NET Core предоставляет набор правил и конвенций по умолчанию
// для обработки запросов и генерации ответов. Например, если метод действия возвращает null, ASP.NET Core автоматически
// возвращает статус кода 404 Not Found.
[ApiController]

public class VillaAPIController : ControllerBase
{
    protected APIResponse _response;
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;
    public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
    {
        _dbVilla = dbVilla;
        _mapper = mapper;
        this._response = new();
    }
    
    //Указывают тип запроса для swagger
    [HttpGet]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            _response.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return _response;

    }
    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id==0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villa = await _dbVilla.GetAsync(u=>u.Id==id);
            if (villa== null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaDto>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return _response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[FromBody] указывает ASP.NET Core, что объект villaDto должен быть получен из тела запроса.
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
    {
        try{
        //Проверка на уникальное имя. Работает только без [ApiController]
        if (await _dbVilla.GetAsync(u=>u.Name.ToLower()==createDto.Name.ToLower())!=null)
        {
            ModelState.AddModelError("CustomError","Villa already Exists!");
            return BadRequest(ModelState);
        }
        if (createDto == null)
        {
            return BadRequest(createDto);
        }

        Villa villa = _mapper.Map<Villa>(createDto);
        await _dbVilla.CreateAsync(villa);

        _response.Result = _mapper.Map<VillaDto>(villa);
        _response.StatusCode = HttpStatusCode.Created;
        return CreatedAtRoute("GetVilla", new {id= villa.Id},_response);
        } catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return _response;
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            await _dbVilla.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        } catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return _response;
    }

    //Обновляет данные всех полей (Все поля)
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
    {
        try
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            Villa model = _mapper.Map<Villa>(updateDto);

            await _dbVilla.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        } catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return _response;
    }

    //Обновляет данные одного поля (Без [ApiController] Работать не будет)
    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }

        var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked:false);
        
        VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
        
        if (villa==null)
        {
            return BadRequest();
        }
        patchDTO.ApplyTo(villaDto,ModelState);
        Villa model = _mapper.Map<Villa>(villaDto);
        
        await _dbVilla.UpdateAsync(model);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
    }
    
}