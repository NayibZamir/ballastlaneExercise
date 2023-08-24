using Crud.Data;
using Crud.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Business.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;

        private byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                var userDto = new UserDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = Crypt(user.Password),
                };
                var result = await _userRepository.CreateUser(userDto);
                return new User
                {
                    IdUser = result.IdUser,
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetUser(string email, string password)
        {
            try
            {
                var result = await _userRepository.GetUser(email, Crypt(password));
                return new User
                {
                    IdUser = result.IdUser,
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string Crypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        private string Decrypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }
    }
}
