using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpokaneChildren.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AnnouncementController : ControllerBase
{
	[HttpGet("AddAnnouncement")]
	public bool AddAnnouncement()
	{
		return false;
	}
}
