using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/VillaNumberAPI")]
[ApiController]

public class VillaNomberController : ControllerBase
{
    protected APIResponse _response;
    private readonly INomberRepository _dbNomber;
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;

    public VillaNomberController(INomberRepository dbNomber, IMapper mapper, IVillaRepository dbVilla)
    {
        _response = new();
        _dbNomber = dbNomber;
        _dbVilla = dbVilla;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetNumbers()
    {
        try
        {
            IEnumerable<VillaNomber> nombersList = await _dbNomber.GetAllAsync(includeProperties:"Villa");
            _response.Result = _mapper.Map<IEnumerable<VillaNomberDTO>>(nombersList);
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
    
    [HttpGet("{id:int}", Name = "GetNomber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<APIResponse>> GetNomber(int id)
    {
        try
        {
            if (id==0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var nomber = await _dbNomber.GetAsync(u=>u.VillaNo==id);
            if (nomber== null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaNomberDTO>(nomber);
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
    [Authorize (Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[FromBody] указывает ASP.NET Core, что объект villaDto должен быть получен из тела запроса.
    public async Task<ActionResult<APIResponse>> CreateNomber([FromBody] VillaNomberCreateDTO createDto)
    {
        try{
            //Проверка на уникальное имя. Работает только без [ApiController]
            if (await _dbNomber.GetAsync(u=>u.VillaNo == createDto.VillaNo)!=null)
            {
                ModelState.AddModelError("ErrorMessages","Nomber already Exists!");
                return BadRequest(ModelState);
            }

            if (await _dbVilla.GetAsync(u=>u.Id==createDto.VillaId)==null)
            {
                ModelState.AddModelError("ErrorMessages","Villa ID is invalid!");
                return BadRequest(ModelState);
            }
            
            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            VillaNomber villaNomber = _mapper.Map<VillaNomber>(createDto);
            await _dbNomber.CreateAsync(villaNomber);

            _response.Result = _mapper.Map<VillaNomberDTO>(villaNomber);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetNomber", new {id= villaNomber.VillaNo},_response);
        } catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return _response;
    }
    
    [HttpDelete("{id:int}", Name = "DeleteNomber")]
    [Authorize (Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteNomber(int id)
    {
        try {
            if (id == 0)
            {
                return BadRequest();
            }

            var nomber = await _dbNomber.GetAsync(u => u.VillaNo == id);
            if (nomber == null)
            {
                return NotFound();
            }

            await _dbNomber.RemoveAsync(nomber);
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
    
    [HttpPut("{id:int}", Name = "UpdateNomber")]
    [Authorize (Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateNomber(int id, [FromBody] VillaNomberUpdateDTO updateDto)
    {
        try
        {
            if (updateDto == null || id != updateDto.VillaNo)
            {
                return BadRequest();
            }
            
            if (await _dbVilla.GetAsync(u=>u.Id==updateDto.VillaId)==null)
            {
                ModelState.AddModelError("ErrorMessages","Villa ID is invalid!");
                return BadRequest(ModelState);
            }

            // Получаем существующую сущность из базы данных по id
            VillaNomber existingNomber = await _dbNomber.GetAsync(u => u.VillaNo == id);

            // Если сущность не найдена, возвращаем статус 404 Not Found
            if (existingNomber == null)
            {
                return NotFound();
            }

            // Маппинг обновленных данных на существующую сущность
            _mapper.Map(updateDto, existingNomber);

            // Вызываем метод обновления
            await _dbNomber.UpdateAsync(existingNomber);

            // Формируем ответ об успешном обновлении
            return NoContent(); // Возвращаем 204 No Content

        }
        catch (Exception e)
        {
            // Обработка исключения в случае ошибки
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.Message }; // Лучше использовать e.Message вместо e.ToString()
            return StatusCode(500, _response); // Возвращаем статус 500 Internal Server Error с сообщением об ошибке
        }
    }

    
    [HttpPatch("{id:int}", Name = "UpdatePartialNomber")]
    [Authorize (Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialNomber(int id, JsonPatchDocument<VillaNomberUpdateDTO> patchDTO)
    {
        try
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            // Получаем сущность из базы данных по идентификатору
            var nomber = await _dbNomber.GetAsync(u => u.VillaNo == id, tracked:false);

            // Если сущность не найдена, возвращаем BadRequest
            if (nomber == null)
            {
                return BadRequest();
            }

            // Преобразуем сущность в DTO объект
            VillaNomberUpdateDTO villaNomberDto = _mapper.Map<VillaNomberUpdateDTO>(nomber);

            // Применяем изменения из JsonPatchDocument к DTO объекту
            patchDTO.ApplyTo(villaNomberDto, ModelState);

            // Проверяем валидность модели после применения изменений
            if (!TryValidateModel(villaNomberDto))
            {
                return BadRequest(ModelState);
            }

            // Обновляем сущность в базе данных с учетом примененных изменений
            _mapper.Map(villaNomberDto, nomber);
            await _dbNomber.UpdateAsync(nomber);

            // Возвращаем статус NoContent при успешном обновлении
            return NoContent();
        }
        catch (Exception e)
        {
            // Обработка исключения в случае ошибки
            return StatusCode(500, $"Internal server error: {e.Message}");
        }
    }
}