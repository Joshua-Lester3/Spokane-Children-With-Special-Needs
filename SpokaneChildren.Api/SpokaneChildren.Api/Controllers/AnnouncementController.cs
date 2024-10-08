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
public class AnnouncementController : ControllerBase
{
	private readonly AnnouncementService _service;

	public AnnouncementController(AnnouncementService service)
	{
		_service = service;
	}

	[HttpPost("AddAnnouncement")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> PostAnnouncement(AnnouncementDto dto)
	{
		if (dto.Title?.Trim().IsNullOrEmpty() ?? true)
		{
			return BadRequest("Title cannot be empty or null.");
		}
		var announcement = await _service.AddAnnouncement(dto);
		return Ok(announcement);
	}

	[HttpPost("DeleteAnnouncement/{id}")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> DeleteAnnouncement([FromRoute] int id)
	{
		var result = await _service.DeleteAnnouncement(id);
		if (result)
		{
			return Ok();
		}
		return BadRequest();
	}

	[HttpGet("GetAnnouncementList")]
	public async Task<IActionResult> GetAnnouncementList(int page, int countPerPage = 5)
	{
		if (page < 0)
		{
			return BadRequest("Page cannot be negative.");
		}
		if (countPerPage < 1)
		{
			return BadRequest("CountPerPage cannot be less than one.");
		}
		var list = await _service.GetAnnouncementList(page, countPerPage);
		return Ok(list);
	}

	[HttpGet("GetAnnouncement")]
	public async Task<IActionResult> GetAnnouncement(int id)
	{
		var result = await _service.GetAnnouncement(id);
		if (result is null)
		{
			return BadRequest();
		}
		return Ok(result);
	}
}
