using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Services;

namespace SpokaneChildren.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EventController : ControllerBase
{
	private readonly EventService _service;

	public EventController(EventService service)
	{
		_service = service;
	}

	[HttpPost("AddEvent")]
	public async Task<IActionResult> AddEvent(EventDto dto)
	{
		if (dto.EventName?.Trim().IsNullOrEmpty() ?? true)
		{
			return BadRequest($"{nameof(dto.EventName)} cannot be empty or null.");
		}
		if (dto.Description?.Trim().IsNullOrEmpty() ?? true)
		{
			return BadRequest($"{nameof(dto.Description)} cannot be empty or null.");
		}
		if (dto.Location?.Trim().IsNullOrEmpty() ?? true)
		{
			return BadRequest($"{nameof(dto.Location)} cannot be empty or null.");
		}
		if (dto.DateTime.Equals(DateTime.MinValue))
		{
			return BadRequest($"{nameof(dto.DateTime)} cannot be default value (01/01/0001).");
		}
		var e = await _service.PostEvent(dto);
		return Ok(e);
	}

	[HttpPost("DeleteEvent/{id}")]
	public async Task<IActionResult> DeleteEvent([FromRoute] int id)
	{
		var result = await _service.DeleteEvent(id);
		if (result)
		{
			return Ok();
		}
		return BadRequest();
	}

	[HttpGet("GetEventList")]
	public async Task<IActionResult> GetEventList()
	{
		var list = await _service.GetEventList();
		return Ok(list);
	}
}
