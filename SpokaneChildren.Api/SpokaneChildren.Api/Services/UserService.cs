using Microsoft.AspNetCore.Identity;
using SpokaneChildren.Api.Dtos;
using SpokaneChildren.Api.Models;

namespace SpokaneChildren.Api.Services;

public class UserService
{
	private readonly UserManager<AppUser> _userManager;

	public UserService(UserManager<AppUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<IdentityResultDto> AddUser(UserDto userDto)
	{
		if (await _userManager.FindByEmailAsync(userDto.Email) == null && await _userManager.FindByNameAsync(userDto.Username) == null)
		{
			AppUser user = new AppUser
			{
				Email = userDto.Email,
				UserName = userDto.Username
			};

			IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);

			if (result.Succeeded)
			{
				return new IdentityResultDto() { Result = IdentityResultEnum.Success };
			}
			else
			{
				return new IdentityResultDto { Result = IdentityResultEnum.Failure, Errors = result.Errors };
			}

		}
		return new IdentityResultDto { Result = IdentityResultEnum.AccountExists };
	}

	public async Task<IdentityResultDto> UpdateUserInfo(UpdateUserInfoDto userDto)
	{
		var user = await _userManager.FindByIdAsync(userDto.Id);
		if (user is null)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.AccountDoesNotExist };
		}
		user.UserName = userDto.NewUsername;
		var result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		var roles = await _userManager.GetRolesAsync(user);
		foreach (var role in roles)
		{
			result = await _userManager.RemoveFromRoleAsync(user, role);
			if (!result.Succeeded)
			{
				return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
			}
		}
		result = await _userManager.AddToRoleAsync(user, userDto.NewRole);
		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		return new IdentityResultDto() { Result = IdentityResultEnum.Success };
	}

	public async Task<IdentityResultDto> ChangePassword(ChangePasswordDto dto)
	{
		var user = await _userManager.FindByIdAsync(dto.Id);
		if (user is null)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.AccountDoesNotExist };
		}
		var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		return new IdentityResultDto() { Result = IdentityResultEnum.Success };
	}

	public async Task<IdentityResultDto> DeleteUser(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		if (user is null)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.AccountDoesNotExist };
		}
		var result = await _userManager.DeleteAsync(user);
		if (!result.Succeeded)
		{
			return new IdentityResultDto() { Result = IdentityResultEnum.Failure, Errors = result.Errors };
		}
		return new IdentityResultDto() { Result = IdentityResultEnum.Success };
	}

	public async Task<AppUser?> GetUser(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		return user;
	}
}

public class IdentityResultDto
{
	public IdentityResultEnum Result { get; set; }
	public IEnumerable<IdentityError>? Errors { get; set; }
}

public enum IdentityResultEnum
{
	Success,
	AccountExists,
	AccountDoesNotExist,
	Failure,
}
