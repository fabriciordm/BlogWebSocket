
using ServerBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBlog.Services
{
    public interface IAuthService
    {
       
        Task<bool> RegisterAsync(string username, string password);
        Task<User> LoginAsync(string username, string password);
    }
}
