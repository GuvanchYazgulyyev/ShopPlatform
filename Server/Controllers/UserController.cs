using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPlatform.Server.Services.Infrastuce;
using ShopPlatform.Shared.ModelsDTO;
using ShopPlatform.Shared.Responses;

namespace ShopPlatform.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // Login Operation
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ServiceResponse<UserLoginResponseDTO>> Login(UserLoginRequestDTO userLogin)
        {
            return new ServiceResponse<UserLoginResponseDTO>()
            {
                Value = await _userService.Login(userLogin.Email, userLogin.Password)
            };
        }

        /// <summary>
        ///  Kullancıları Getir.
        /// </summary>
        /// <returns></returns>

        [HttpGet("Users")]
        public async Task<ServiceResponse<List<UserDTO>>> GetUser()
        {
            return new ServiceResponse<List<UserDTO>>()
            {
                Value = await _userService.GetUser()
            };
        }
        ///
        // Create User
        [HttpPost("Create")]
        public async Task<ServiceResponse<UserDTO>> CreateUser([FromBody] UserDTO user)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await _userService.CreateUser(user)
            };
        }
        ///User Update
        ///
        [HttpPut("Update")]
        public async Task<ServiceResponse<UserDTO>> UpdateUser([FromBody] UserDTO user)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await _userService.UpdateUser(user)
            };
        }

        /// Id Ye Göre Getir 
        [HttpPost("UserById/{Id}")]
        public async Task<ServiceResponse<UserDTO>> GetUserById(Guid Id)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await _userService.GetUserById(Id)
            };
        }

        [HttpGet("{Id}")]
        public async Task<ServiceResponse<UserDTO>> GetUserById2(Guid Id)
        {
            return new ServiceResponse<UserDTO>
            {
                Value = await _userService.GetUserById(Id)
            };
        }

        // Kullanıcı Sil
        [HttpDelete("Delete")]
        public async Task<ServiceResponse<bool>> DeleteUser([FromBody] Guid Id)
        {
            return new ServiceResponse<bool>()
            {
                Value = await _userService.DeleteUserById(Id)
            };
        }

    }
}
