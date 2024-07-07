
using ServerBlog.Models;
using ServerBlog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBlog.Services
{
    //internal class AuthService
    //{
    //}
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
           // var existingUser = await _userRepository.GetUserByUsernameAndPwdAsync(username, password);
           var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                return false; // Username already exists
            }
            else
            {
                var user = new User { Username = username, Password = password };
                await _userRepository.AddUserAsync(user);
                return true;
            }
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAndPwdAsync(username, password);
            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }

       
    }
}
