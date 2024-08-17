using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


}
