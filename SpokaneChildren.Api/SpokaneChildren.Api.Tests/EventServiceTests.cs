using SpokaneChildren.Api.Data;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;
using SpokaneChildren.Api.Services;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class EventServiceTests : DatabaseTestBase
{
	private EventService _service = null!;
	private AppDbContext _context = null!;

	[TestInitialize]
	public void Init()
	{
		_context = new AppDbContext(Options);
		_service = new EventService(_context);
	}

	[TestMethod]
	public async Task PostEvent_FieldsArePopulated_Success()
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = "Test Event :)",
			Description = "Fun event",
			DateTime = DateTime.UtcNow,
			Location = "East side park",
		};

		// Act
		var response = await _service.PostEvent(dto);

		// Assert
		CollectionAssert.Contains(_context.Events.ToList(), response);
	}

	[TestMethod]
	[DataRow(null, "Fun event", "East side park")]
	[DataRow("Test Event :)", null, "East side park")]
	[DataRow("Test Event :)", "Fun event", null)]
	public async Task PostEvent_ARequiredFieldIsNull_ThrowsArgumentNullException(string eventName, string description, string location)
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = eventName,
			Description = description,
			Location = location,
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _service.PostEvent(dto));
	}

	[TestMethod]
	[DataRow("   ", "Fun event", "East side park")]
	[DataRow("Test Event :)", "    ", "East side park")]
	[DataRow("Test Event :)", "Fun event", "    ")]
	[DataRow("", "Fun event", "East side park")]
	[DataRow("Test Event :)", "", "East side park")]
	[DataRow("Test Event :)", "Fun event", "")]
	public async Task PostEvent_RequiredFieldIsEmptyOrWhitespace_ThrowsArgumentException(string eventName, string description, string location)
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = eventName,
			Description = description,
			Location = location,
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _service.PostEvent(dto));
	}

	[TestMethod]
	public async Task PostEvent_DateTimeIsDefaultValue_ThrowsArgumentException()
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = "Test Event :)",
			Description = "Fun event",
			Location = "East side park",
			DateTime = new DateTime(),
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _service.PostEvent(dto));
	}

	[TestMethod]
	public async Task PostEvent_UpdatedAddedEvent_ChangesAreSaved()
	{
		// Arrange
		var dto = new EventDto
		{
			EventName = "Test Event :)",
			Description = "Fun event",
			DateTime = DateTime.UtcNow,
			Location = "East side park",
		};
		var addedResponse = await _service.PostEvent(dto);
		var editingDto = new EventDto
		{
			EventId = addedResponse.EventId,
			EventName = "New test Event :)",
			Description = "Fun new event",
			DateTime = DateTime.UtcNow,
			Location = "West side park",
		};

		// Act
		var response = await _service.PostEvent(editingDto);

		// Assert
		Assert.AreEqual(editingDto.EventName, response.EventName);
		Assert.AreEqual(editingDto.Description, response.Description);
		Assert.AreEqual(editingDto.Location, response.Location);
	}

	[TestMethod]
	public async Task DeleteEvent_ValidId_RemovesEvent()
	{
		// Arrange
		var e = await AddEvent();

		// Act
		var response = await _service.DeleteEvent(e.EventId);

		// Assert
		Assert.IsTrue(response);
		CollectionAssert.DoesNotContain(_context.Events.ToList(), e);
	}

	[TestMethod]
	public async Task DeleteEvent_InvalidId_ReturnsFalse()
	{
		// Arrange

		// Act
		var response = await _service.DeleteEvent(-1);

		// Assert
		Assert.IsFalse(response);
	}

	private async Task<Event> AddEvent()
	{
		var dto = new EventDto
		{
			EventName = "Test Event :)",
			Description = "Fun event",
			DateTime = DateTime.UtcNow,
			Location = "East side park",
		};
		return await _service.PostEvent(dto);
	}

	[TestMethod]
	public async Task GetEventList_FirstPage_ReturnsPaginatedSortedList()
	{
		// Arrange
		var event1 = await AddEvent();
		var event2 = await AddEvent();
		var event3 = await AddEvent();
		var event4 = await AddEvent();
		var event5 = await AddEvent();
		var event6 = await AddEvent();
		var expectedOrderedList = new Event[] { event1, event2, event3, event4, event5 };

		// Act
		var eventList = await _service.GetEventList(0, 5);

		// Assert
		CollectionAssert.AreEqual(expectedOrderedList, eventList);
	}

	[TestMethod]
	public async Task GetEventList_SecondPage_ReturnsPaginatedSortedList()
	{
		// Arrange
		var event1 = await AddEvent();
		var event2 = await AddEvent();
		var event3 = await AddEvent();
		var event4 = await AddEvent();
		var event5 = await AddEvent();
		var event6 = await AddEvent();
		var expectedOrderedList = new Event[] { event6 };

		// Act
		var eventList = await _service.GetEventList(1, 5);

		// Assert
		CollectionAssert.AreEqual(expectedOrderedList, eventList);
	}

	[TestMethod]
	[DataRow(5, 0)]
	[DataRow(-1, 5)]
	public async Task GetEventList_InvalidArgument_ReturnsPaginatedSortedList(int page, int countPerPage)
	{
		// Arrange

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _service.GetEventList(page, countPerPage));
	}

	[TestMethod]
	public async Task GetEvent_ValidId_Success()
	{
		// Arrange
		var e = await AddEvent();

		// Act
		var result = await _service.GetEvent(e.EventId);

		// Assert
		Assert.AreEqual(e, result);
	}

	[TestMethod]
	public async Task GetEvent_InvalidId_ReturnsNull()
	{
		// Arrange

		// Act
		var result = await _service.GetEvent(-1);

		// Assert
		Assert.IsNull(result);
	}
}
