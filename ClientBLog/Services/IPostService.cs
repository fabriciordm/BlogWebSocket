
using ClientBLog.Models;
using ClientBLog.Models;
using ClientBLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBLog.Services
{
    public interface IPostService
    {
        Task<Post> GetPostByIdAsync(int id);

        Task<IEnumerable<Post>> GetPostsAsync();
        Task AddPostAsync(Post post);
        //Task UpdatePostAsync_(Post post, User usr);      
        Task UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int id, User usr);
        bool verificarOwnerPost(int id, User usr, Post post);
    }
}
