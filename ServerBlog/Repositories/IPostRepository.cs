
using ServeBlog.Models;
using ServerBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBlog.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostByIdAsync(int id);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);

        bool verificarOwnerPost(int id,User usr,Post post);
    }
}
