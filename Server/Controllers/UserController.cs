using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopPlatform.Server.Services.Infrastuce;
using ShopPlatform.Shared.ModelsDTO;
using ShopPlatform.Shared.Responses;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ServiceResponse<UserLoginResponseDTO>> Login(UserLoginRequestDTO userLogin)
    {
        return new ServiceResponse<UserLoginResponseDTO>()
        {
            Value = await _userService.Login(userLogin.Email, userLogin.Password)
        };
    }

    [HttpGet("Users")]
    [AllowAnonymous]
    public async Task<ServiceResponse<List<UserDTO>>> GetUser()
    {
        return new ServiceResponse<List<UserDTO>>()
        {
            Value = await _userService.GetUser()
        };
    }

    [HttpPost("Create")]
    public async Task<ServiceResponse<UserDTO>> CreateUser([FromBody] UserDTO user)
    {
        return new ServiceResponse<UserDTO>()
        {
            Value = await _userService.CreateUser(user)
        };
    }

    [HttpPut("Update")]
    public async Task<ServiceResponse<UserDTO>> UpdateUser([FromBody] UserDTO user)
    {
        return new ServiceResponse<UserDTO>()
        {
            Value = await _userService.UpdateUser(user)
        };
    }

    // ✅ Kullanımı sadeleştirilmiş, sadece GET ile:
    [HttpGet("{id}")]
    public async Task<ServiceResponse<UserDTO>> GetUserById(Guid id)
    {
        return new ServiceResponse<UserDTO>
        {
            Value = await _userService.GetUserById(id)
        };
    }

    [HttpDelete("Delete")]
    public async Task<ServiceResponse<bool>> DeleteUser([FromBody] Guid Id)
    {
        return new ServiceResponse<bool>()
        {
            Value = await _userService.DeleteUserById(Id)
        };
    }
}
