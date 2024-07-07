
using ClientBLog.Models;
using ClientBLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBLog.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAndPwdAsync(string username, string password);

        Task<User> GetUserByUsernameAsync(string username);

        Task AddUserAsync(User user);
    }
}
