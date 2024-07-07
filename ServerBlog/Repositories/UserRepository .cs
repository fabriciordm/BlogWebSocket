﻿
using ServerBlog.Models;

namespace ServerBlog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ServerBlog.Data.DataContext _context;

        public UserRepository(ServerBlog.Data.DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAndPwdAsync(string username, string password)
        {
            return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Username == username && u.Password == password));
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Username == username));
        }

        

        public async Task AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { }
        }

        
    }
}
