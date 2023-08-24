using Crud.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Data
{
    public interface IUserRepository
    {
        Task<UserDto> GetUser(string email, string password);
        Task<UserDto> CreateUser(UserDto user);
    }
}
