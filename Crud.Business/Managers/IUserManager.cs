using Crud.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Business.Managers
{
    public interface IUserManager
    {
        Task<User> GetUser(string email, string password);
        Task<User> CreateUser(User user);
    }
}
