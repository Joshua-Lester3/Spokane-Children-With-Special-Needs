using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Identity;
using SpokaneChildren.Api.Services;

namespace SpokaneChildren.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ResourceController : ControllerBase
{
	private readonly ResourceService _service;

	public ResourceController(ResourceService service)
	{
		_service = service;
	}

	[HttpPost("AddResource")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> AddResource(ResourceDto dto)
	{
		if (dto.Name?.Trim().IsNullOrEmpty() ?? true)
		{
			return BadRequest($"{nameof(dto.Name)} cannot be empty or null.");
		}
		var resource = await _service.PostResource(dto);
		return Ok(resource);
	}

	[HttpPost("DeleteResource/{id}")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> DeleteResource([FromRoute] int id)
	{
		var result = await _service.DeleteResource(id);
		if (result)
		{
			return Ok();
		}
		return BadRequest();
	}

	[HttpGet("GetResourceList")]
	public async Task<IActionResult> GetResourceList()
	{
		var list = await _service.GetResourceList();
		return Ok(list);
	}

	[HttpGet("GetResource")]
	public async Task<IActionResult> GetResource(int id)
	{
		var result = await _service.GetResource(id);
		if (result is null)
		{
			return BadRequest();
		}
		return Ok(result);
	}
}
