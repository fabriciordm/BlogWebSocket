using ClientBLog.Models;
using ClientBLog.Services;
using ClientBLog.Models;
using ClientBLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClientBLog.Controller
{
    public class PostController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task CreatePost(string title, string content, int userId)
        {
            var post = new Post  { Title = title, Content = content, UserId = userId, CreatedAt = DateTime.Now };
            await _postService.AddPostAsync(post);
        }

        public bool  VerificarOwnerPost(int id, string title, string content,User usr)
        {
         
            Post pst = new Post();
            pst.Id = id;
            pst.Title = title;  
            pst.Content = content;
            var post = _postService.verificarOwnerPost(id, usr,pst);

            if (post)
            {
                return true;
               
            }
            else
                return false;
            
        }

        public async Task Update(Post post)
        {
            await _postService.UpdatePostAsync(post);
        }
        public Task<bool> DeletePost(int id, User usr)
        {
            return _postService.DeletePostAsync(id, usr);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _postService.GetPostsAsync();
        }

       
        
        
    }
}
