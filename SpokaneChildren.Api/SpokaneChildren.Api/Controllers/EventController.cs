﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Identity;
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
	[Authorize(Roles = Roles.Admin)]
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
	[Authorize(Roles = Roles.Admin)]
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
	public async Task<IActionResult> GetEventList(int page, int countPerPage = 3)
	{
		if (page < 0)
		{
			return BadRequest("Page cannot be negative.");
		}
		if (countPerPage < 1)
		{
			return BadRequest("CountPerPage cannot be less than one.");
		}
		var list = await _service.GetEventList(page, countPerPage);
		return Ok(list);
	}

	[HttpGet("GetEvent")]
	public async Task<IActionResult> GetEvent(int id)
	{
		var result = await _service.GetEvent(id);
		if (result is null)
		{
			return BadRequest();
		}
		return Ok(result);
	}
}
