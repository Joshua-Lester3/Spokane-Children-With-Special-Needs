using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;
using SpokaneChildren.Api.Services;

namespace SpokaneChildren.Api.Tests;

[TestClass]
public class UserServiceTests
{

	[TestMethod]
	public async Task GetUser_ValidId_Success()
	{
		// Arrange
		var store = new Mock<IUserStore<AppUser>>();
		store.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
			.ReturnsAsync(new AppUser()
			{
				Id = "123",
			});

		var manager = new UserManager<AppUser>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

		var service = new UserService(manager);

		// Act
		var result = await service.GetUser("123");

		// Assert
		Assert.IsNotNull(result);
		Assert.AreEqual("123", result.Id);
	}

	[TestMethod]
	public async Task GetUser_InvalidId_ReturnsNull()
	{
		// Arrange
		var store = new Mock<IUserStore<AppUser>>();
		store.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
			.ReturnsAsync((AppUser?) null);

		var manager = new UserManager<AppUser>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

		var service = new UserService(manager);

		// Act
		var result = await service.GetUser("123");

		// Assert
		Assert.IsNull(result);
	}

	[TestMethod]
	public async Task AddUser_EmailAndNameDoesNotExist_Success()
	{
		// Arrange
		var dto = new UserDto
		{
			Username = "Jimbob",
			Email = "jimbob123@gmail.com",
			Password = "JimmybobJr345",
			Role = "Moderator",
		};

		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByEmailAsync(dto.Email))
			.ReturnsAsync((AppUser?)null);
		manager.Setup(x => x.FindByNameAsync(dto.Username))
			.ReturnsAsync((AppUser?)null);
		manager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
			.ReturnsAsync(IdentityResult.Success);

		var service = new UserService(manager.Object);

		// Act
		var result = await service.AddUser(dto);

		// Assert
		Assert.AreEqual(IdentityResultEnum.Success, result.Result);
	}

	[TestMethod]
	public async Task AddUser_EmailAndNameExists_ReturnsAccountExists()
	{
		// Arrange
		var dto = new UserDto
		{
			Username = "Jimbob",
			Email = "jimbob123@gmail.com",
			Password = "JimmybobJr345",
			Role = "Moderator",
		};
		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByEmailAsync(dto.Email))
			.ReturnsAsync(new AppUser());
		manager.Setup(x => x.FindByNameAsync(dto.Username))
			.ReturnsAsync(new AppUser());

		var service = new UserService(manager.Object);

		// Act
		var result = await service.AddUser(dto);

		// Assert
		Assert.AreEqual(IdentityResultEnum.AccountExists, result.Result);
	}

	[TestMethod]
	public async Task UpdateUserInfo_UserExists_Success()
	{
		// Arrange
		var dto = new UpdateUserInfoDto
		{
			Id = "123",
			NewUsername = "Thorfinn",
			NewRole = "Moderator",
		};
		var roles = new string[]
		{
			"Admin",
		};
		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByIdAsync(dto.Id))
			.ReturnsAsync(new AppUser());
		manager.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
			.ReturnsAsync(roles);
		manager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
			.ReturnsAsync(IdentityResult.Success);
		manager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
			.ReturnsAsync(IdentityResult.Success);
		manager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
			.ReturnsAsync(IdentityResult.Success);
		var service = new UserService(manager.Object);

		// Act
		var result = await service.UpdateUserInfo(dto);

		// Assert
		Assert.AreEqual(IdentityResultEnum.Success, result.Result);
		manager.VerifyAll();
	}

	[TestMethod]
	public async Task UpdateUserInfo_UserDoesNotExist_ReturnsFalse()
	{
		// Arrange
		var dto = new UpdateUserInfoDto
		{
			Id = "123",
			NewUsername = "Thorfinn",
			NewRole = "Moderator",
		};
		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByIdAsync(dto.Id))
			.ReturnsAsync((AppUser?) null);
		var service = new UserService(manager.Object);

		// Act
		var result = await service.UpdateUserInfo(dto);

		// Assert
		Assert.AreEqual(IdentityResultEnum.AccountDoesNotExist, result.Result);
		manager.VerifyAll();
	}

	[TestMethod]
	public async Task ChangePassword_AccountExists_Success()
	{
		// Arrange
		var dto = new ChangePasswordDto
		{
			Id = "123",
			CurrentPassword = "ThorfinnIsCool",
			NewPassword = "NoWarIsCool",
		};
		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByIdAsync(dto.Id))
			.ReturnsAsync(new AppUser());
		manager.Setup(x => x.ChangePasswordAsync(It.IsAny<AppUser>(), dto.CurrentPassword, dto.NewPassword))
			.ReturnsAsync(IdentityResult.Success);
		var service = new UserService(manager.Object);

		// Act
		var result = await service.ChangePassword(dto);

		// Assert
		Assert.AreEqual(IdentityResultEnum.Success, result.Result);
		manager.VerifyAll();
	}

	[TestMethod]
	public async Task ChangePassword_AccountDoesNotExist_Failure()
	{
		// Arrange
		var dto = new ChangePasswordDto
		{
			Id = "123",
			CurrentPassword = "ThorfinnIsCool",
			NewPassword = "NoWarIsCool",
		};
		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByIdAsync(dto.Id))
			.ReturnsAsync((AppUser?) null);
		var service = new UserService(manager.Object);

		// Act
		var result = await service.ChangePassword(dto);

		// Assert
		Assert.AreEqual(IdentityResultEnum.AccountDoesNotExist, result.Result);
		manager.VerifyAll();
	}

	[TestMethod]
	public async Task DeleteUser_UserExists_Success()
	{
		// Arrange
		var id = "123";
		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByIdAsync(id))
			.ReturnsAsync(new AppUser());
		manager.Setup(x => x.DeleteAsync(It.IsAny<AppUser>()))
			.ReturnsAsync(IdentityResult.Success);
		var service = new UserService(manager.Object);

		// Act
		var result = await service.DeleteUser(id);

		// Assert
		Assert.AreEqual(IdentityResultEnum.Success, result.Result);
		manager.VerifyAll();
	}

	[TestMethod]
	public async Task DeleteUser_UserDoesNotExist_Failure()
	{
		// Arrange
		var id = "123";
		var manager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null!, null!, null!, null!, null!, null!, null!, null!);
		manager.Setup(x => x.FindByIdAsync(id))
			.ReturnsAsync((AppUser?)null);
		var service = new UserService(manager.Object);

		// Act
		var result = await service.DeleteUser(id);

		// Assert
		Assert.AreEqual(IdentityResultEnum.AccountDoesNotExist, result.Result);
		manager.VerifyAll();
	}
}
