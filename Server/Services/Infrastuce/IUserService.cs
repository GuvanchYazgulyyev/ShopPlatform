using ShopPlatform.Shared.ModelsDTO;

namespace ShopPlatform.Server.Services.Infrastuce
{
    public interface IUserService
    {
        public Task<UserDTO> GetUserById(Guid id);
        public Task<List<UserDTO>> GetUser();
        public Task<UserDTO> CreateUser(UserDTO User);
        public Task<UserDTO> UpdateUser(UserDTO User);
        public Task<bool> DeleteUserById(Guid id);

        Task<UserLoginResponseDTO> Login(string EMail, string Passwrd);

        public Task DeleteUser(Guid id);
    }
}
