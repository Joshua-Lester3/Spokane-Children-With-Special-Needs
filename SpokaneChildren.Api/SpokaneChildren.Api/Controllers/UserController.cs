using Microsoft.AspNetCore.Mvc;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Identity;
using SpokaneChildren.Api.Services;

namespace SpokaneChildren.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	private readonly UserService _service;

	public UserController(UserService service)
	{
		_service = service;
	}

	[HttpPost("AddUser")]
	public async Task<IActionResult> AddUser(UserDto dto)
	{
		if (dto.Username is null || dto.Username.Trim().Length == 0)
		{
			return BadRequest("Username cannot be null or empty.");
		}
		if (dto.Email is null || dto.Email.Trim().Length == 0)
		{
			return BadRequest("Email cannot be null or empty.");
		}
		if (dto.Password is null || dto.Password.Trim().Length == 0)
		{
			return BadRequest("Password cannot be null or empty.");
		}
		if (dto.Role is null || dto.Role.Trim().Length == 0)
		{
			return BadRequest("Role cannot be null or empty.");
		}
		if (!Roles.RolesList.Contains(dto.Role))
		{
			return BadRequest("Role must be a valid role.");
		}
		var result = await _service.AddUser(dto);
		if (result.Result == IdentityResultEnum.Success)
		{
			return Ok();
		}
		else
		{
			return BadRequest(result);
		}
	}

	[HttpGet("GetUser")]
	public async Task<IActionResult> GetUser(string id)
	{
		if (id is null || id.Trim().Length == 0)
		{
			return BadRequest("Id cannout be null or empty.");
		}
		var result = await _service.GetUser(id);
		if (result is null)
		{
			return NotFound();
		}
		return Ok(result);
	}

	[HttpPost("ChangePassword")]
	public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
	{
		if (dto.Id is null || dto.Id.Trim().Length == 0)
		{
			return BadRequest("Id cannout be null or empty.");
		}
		if (dto.CurrentPassword is null || dto.CurrentPassword.Trim().Length == 0)
		{
			return BadRequest("CurrentPassword cannout be null or empty.");
		}
		if (dto.NewPassword is null || dto.NewPassword.Trim().Length == 0)
		{
			return BadRequest("NewPassword cannout be null or empty.");
		}
		var result = await _service.ChangePassword(dto);
		if (result.Result == IdentityResultEnum.Success)
		{
			return Ok();
		}
		return BadRequest(result);
	}

	[HttpPost("UpdateUserInfo")]
	public async Task<IActionResult> UpdateUserInfo(UpdateUserInfoDto dto)
	{
		if (dto.Id is null || dto.Id.Trim().Length == 0)
		{
			return BadRequest("Id cannout be null or empty.");
		}
		if (dto.NewUsername is null || dto.NewUsername.Trim().Length == 0)
		{
			return BadRequest("NewUsername cannout be null or empty.");
		}
		if (dto.NewRole is null || dto.NewRole.Trim().Length == 0)
		{
			return BadRequest("NewRole cannout be null or empty.");
		}
		if (!Roles.RolesList.Contains(dto.NewRole))
		{
			return BadRequest("Role must be a valid role.");
		}
		var result = await _service.UpdateUserInfo(dto);
		if (result.Result == IdentityResultEnum.Success)
		{
			return Ok();
		}
		return BadRequest(result);
	}

	[HttpGet("GetUserList")]
	public async Task<List<UserInfoDto>> GetUserList()
	{
		return await _service.GetUserList();
	}

	[HttpPost("DeleteUser/{id}")]
	public async Task<IActionResult> DeleteUser([FromRoute] string id)
	{
		if (id is null || id.Trim().Length == 0)
		{
			return BadRequest("id is required.");
		}
		var response = await _service.DeleteUser(id);
		if (response.Result == IdentityResultEnum.Success)
		{
			return Ok();
		}
		return BadRequest(response);
	}
}
