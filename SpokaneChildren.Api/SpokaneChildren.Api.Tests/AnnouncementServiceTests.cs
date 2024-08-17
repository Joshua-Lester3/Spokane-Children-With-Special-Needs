using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;
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
	public async Task PostAnnouncement_TitleIsPopulated_Success()
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = "Test announcement :)",
		};

		// Act
		var response = await _service.AddAnnouncement(dto);

		// Assert
		CollectionAssert.Contains(_context.Announcements.ToList(), response);
	}

	[TestMethod]
	public async Task PostAnnouncement_TitleNull_ThrowsArgumentNullException()
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = null!,
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _service.AddAnnouncement(dto));
	}

	[TestMethod]
	[DataRow("")]
	[DataRow("    ")]
	public async Task PostAnnouncement_TitleEmpty_ThrowsArgumentException(string title)
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = title,
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _service.AddAnnouncement(dto));
	}

	[TestMethod]
	public async Task PostAnnouncement_NormalConditions_ChangesAreSaved()
	{
		// Arrange
		var dto = new AnnouncementDto
		{
			Title = "Test announcement :)",
		};
		var addedResponse = await _service.AddAnnouncement(dto);
		var editingDto = new AnnouncementDto
		{
			Id = addedResponse.Id,
			Title = "New test announcment :D",
		};

		// Act
		var response = await _service.AddAnnouncement(editingDto);

		// Assert
		Assert.AreEqual(editingDto.Title, response.Title);
	}

	[TestMethod]
	public async Task DeleteAnnouncement_ValidId_RemovesAnnouncement()
	{
		// Arrange
		var announcement = await AddAnnouncement();

		// Act
		var result = await _service.DeleteAnnouncement(announcement.Id);

		// Assert
		Assert.IsTrue(result);
		CollectionAssert.DoesNotContain(_context.Announcements.ToList(), announcement);
	}

	[TestMethod]
	public async Task DeleteAnnouncement_InvalidId_ReturnsFalse()
	{
		// Arrange

		// Act
		var result = await _service.DeleteAnnouncement(-1);

		// Assert
		Assert.IsFalse(result);
	}

	private async Task<Announcement> AddAnnouncement()
	{
		var dto = new AnnouncementDto
		{
			Title = "Test announcement :)",
		};
		return await _service.AddAnnouncement(dto);
	}

	[TestMethod]
	public async Task GetAnnouncementList_NormalConditions_ReturnsListOfAnnouncementsByNewest()
	{
		// Arrange
		var announcement1 = await AddAnnouncement();
		var announcement2 = await AddAnnouncement();
		var announcement3 = await AddAnnouncement();
		var expectedOrderedList = new Announcement[] { announcement3, announcement2, announcement1 };

		// Act
		var announcementList = await _service.GetAnnouncementList();

		// Assert
		CollectionAssert.AreEqual(expectedOrderedList, announcementList);
	}
}
