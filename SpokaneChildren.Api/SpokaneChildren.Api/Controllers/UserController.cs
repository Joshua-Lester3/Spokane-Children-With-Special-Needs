using Microsoft.AspNetCore.Mvc;
using SpokaneChildren.Api.Dtos;
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

	[HttpGet("AddUser")]
	public async Task AddUser(UserDto dto)
	{

	}
}
