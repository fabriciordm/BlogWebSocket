
using ClientBLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBLog.Services
{
    public interface IAuthService
    {
       
        Task<bool> RegisterAsync(string username, string password);
        Task<User> LoginAsync(string username, string password);
    }
}
