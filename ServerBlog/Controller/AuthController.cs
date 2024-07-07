using ServerBlog.Services;
using ServerBlog.Models;
using ServerBlog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBlog.Controller
{
    public class AuthController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> Register(string username, string password)
        {
            return await _authService.RegisterAsync(username, password);
        }

        public async Task<User> Login(string username, string password)
        {
            return await _authService.LoginAsync(username, password);
        }     
       
    }
}
