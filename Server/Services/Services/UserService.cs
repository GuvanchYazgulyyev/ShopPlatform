using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopPlatform.Server.Context;
using ShopPlatform.Server.Services.Infrastuce;
using ShopPlatform.Shared.ModelsDTO;
using ShopPlatform.Shared.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopPlatform.Server.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly BlazorShopDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, BlazorShopDbContext context, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }


        /// <summary>
        /// Müşteri Ekle.
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDTO> CreateUser(UserDTO user)
        {
            try
            {
                // Eğer Id boşsa sabit GUID ata (ya da belirli bir string'e göre oluştur)
                if (user.Id == Guid.Empty)
                    user.Id = GuidGenerator.GenerateDeterministicGuid(user.EMailAddress);  // Örneğin E-Mail adresine göre deterministik GUID oluşturuluyor.

                // Bu Id zaten kayıtlı mı kontrol et
                var findData = await _context.Users.Where(f => f.Id == user.Id).FirstOrDefaultAsync();
                if (findData != null)
                    throw new ApiExeption("Daha Önce Kaydınız Yapılmış!!!!");

                // Kullanıcıyı ekle
                var entity = _mapper.Map<Entities.Users>(user);
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();

                return _mapper.Map<UserDTO>(entity);
            }
            catch (ApiExeption ex)
            {

                throw;
            }

        }



        public static class GuidGenerator
        {
            // Belli bir namespace kullanıyoruz (örnek olarak bir sabit GUID, değiştirebilirsin)
            private static readonly Guid _customNamespace = Guid.Parse("bde15a6e-f09b-4d1b-8d08-69e926b236c7");

            // Deterministic bir GUID oluşturuyoruz
            public static Guid GenerateDeterministicGuid(string name)
            {
                using var sha1 = SHA1.Create();
                var namespaceBytes = _customNamespace.ToByteArray();
                var nameBytes = Encoding.UTF8.GetBytes(name);

                // Namespace ve name byte'larını birleştiriyoruz
                var combined = new byte[namespaceBytes.Length + nameBytes.Length];
                Buffer.BlockCopy(namespaceBytes, 0, combined, 0, namespaceBytes.Length);
                Buffer.BlockCopy(nameBytes, 0, combined, namespaceBytes.Length, nameBytes.Length);

                var hash = sha1.ComputeHash(combined);

                // UUIDv5 formatına uygun hale getiriyoruz
                hash[6] = (byte)((hash[6] & 0x0F) | 0x50); // Versiyon 5
                hash[8] = (byte)((hash[8] & 0x3F) | 0x80); // RFC 4122 Formatı

                var guidBytes = new byte[16];
                Array.Copy(hash, guidBytes, 16);

                return new Guid(guidBytes); // Bu, tam istediğiniz formatta bir GUID dönecektir.
            }
        }



        /// <summary>
        /// Veriyi Id Ye Göre sil.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteUserById(Guid id)
        {
            var findData = await _context.Users.Where(f => f.Id == id).FirstOrDefaultAsync();
            if (findData == null)
                throw new Exception("Kullanıcı Bulunamadı!!!!");
            _context.Users.Remove(findData);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        ///  Kullanıcıları getir
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<UserDTO>> GetUser()
        {
            return await _context.Users.Where(f => f.IsActive)
                 .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                 .ToListAsync();
        }

        /// <summary>
        ///  Id Ye Göre Tek Veriyi getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDTO> GetUserById(Guid id)
        {
            return await _context.Users.Where(f => f.IsActive && f.Id == id)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }



        /// <summary>
        ///  Login And JWT Operation
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Passwrd"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserLoginResponseDTO> Login(string EMail, string Passwrd)
        {
            try
            {
                // Şifreyi şifrele
                // var encryptPassword = PasswordEncrypter.Encrypt(Passwrd);

                // Kullanıcı doğrulama
                var userControl = await _context.Users.FirstOrDefaultAsync(x => x.EMailAddress == EMail && x.Password == Passwrd);

                if (userControl == null)
                    throw new ApiExeption("Kullanıcı bulunamadı!");

                if (!userControl.IsActive)
                    throw new ApiExeption("Kullanıcı kayıtlı fakat aktif değil!");

                UserLoginResponseDTO userLoginResponseDTO = new UserLoginResponseDTO();

                // Token üretimi
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtExpiryInDays"]));

                var claims = new[]
                {
            new Claim(ClaimTypes.Email, userControl.EMailAddress),
            new Claim(ClaimTypes.NameIdentifier, userControl.Id.ToString()), // ekstra claim önerisi
            new Claim(ClaimTypes.Name, userControl.FirstName + " " + userControl.LastName ?? "")
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtIssuer"],
                    audience: _configuration["JwtAudience"],
                    claims: claims,
                    expires: expiry,
                    signingCredentials: credentials);
                userLoginResponseDTO.ApiToken = new JwtSecurityTokenHandler().WriteToken(token);
                userLoginResponseDTO.User = _mapper.Map<UserDTO>(userControl);
                return userLoginResponseDTO;


            }
            catch (ApiExeption ex)
            {
                // Loglanabilir
                throw new ApiExeption("Login sırasında hata oluştu: " + ex.Message);
            }
        }


        /// <summary>
        /// Veriyi Güncelle
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDTO> UpdateUser(UserDTO User)
        {
            var findData = await _context.Users.Where(f => f.Id == User.Id).FirstOrDefaultAsync();
            if (findData == null)
                throw new ApiExeption("Kayıt Bulunamadı!!!!");

            _mapper.Map(User, findData);

            int result = await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(findData);
        }

        /// <summary>
        /// Datalary Sil
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        public async Task DeleteUser(Guid guid)
        {
            var deleteData = await _context.Users.FirstOrDefaultAsync(f => f.Id == guid);
            if (deleteData != null)
            {
                _context.Users.Remove(deleteData);
                await _context.SaveChangesAsync();
            }
        }
    }
}
