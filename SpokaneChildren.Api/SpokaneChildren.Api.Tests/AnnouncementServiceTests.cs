using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Services;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class AnnouncementServiceTests : DatabaseTestBase
{
	private AnnouncementService _service = null!;
	private AppDbContext _context = null!;

	[TestInitialize]
	public void Init()
	{
		_context = new AppDbContext(Options);
		_service = new AnnouncementService(_context);
	}

	[TestMethod]
	public void AddAnnouncement_NormalConditions_Success()
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = "Test announcement :)",
		};

		// Act
		var response = _service.AddAnnouncement(dto);

		// Assert
		CollectionAssert.Contains(_context.Announcements.ToList(), response);
	}
}
