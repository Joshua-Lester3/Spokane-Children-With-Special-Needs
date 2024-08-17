using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpokaneChildren.Api.Dtos;
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
	public async Task<IActionResult> PostAnnouncement(AnnouncementDto dto)
	{
		if (dto.Title.IsNullOrEmpty())
		{
			return BadRequest("Title cannot be empty or null.");
		}
		var announcement = await _service.AddAnnouncement(dto);
		return Ok(announcement);
	}

	[HttpPost("DeleteAnnouncement/{id}")]
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
	public async Task<IActionResult> GetAnnouncementList()
	{
		var list = await _service.GetAnnouncementList();
		return Ok(list);
	}
}
