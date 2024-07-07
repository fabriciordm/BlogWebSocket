using ClientBLog.Models;
using ClientBLog.Notifications;
using ClientBLog.Repositories;
using ClientBLog.Models;

using ClientBLog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientBLog.Services
{    
        public class PostService : IPostService
        {
            private readonly IPostRepository _postRepository;
            private readonly INotificationService _notificationService;

            public PostService(IPostRepository postRepository, INotificationService notificationService)
            {
                _postRepository = postRepository;
                _notificationService = notificationService;
            }

            public async Task<Post> GetPostByIdAsync(int id)
            {
                return await _postRepository.GetPostByIdAsync(id);
            }

            public async Task<IEnumerable<Post>> GetPostsAsync()
            {
                return await _postRepository.GetPostsAsync();
            }

            public async Task AddPostAsync(Post post)
            {
                await _postRepository.AddPostAsync(post);
                   
            }

            public async Task UpdatePostAsync(Post post)
            {
                await _postRepository.UpdatePostAsync(post);                         
            }

            public async Task<bool> DeletePostAsync(int id, User usr)
            {
                
            var post = await GetPostByIdAsync(id);
            if (post.UserId == usr.Id)
            {
                await _postRepository.DeletePostAsync(id);              
                return true;
            }

            else
            {
                return false;
            }
            }

           
            public bool verificarOwnerPost(int id, User usr, Post post)
            {
                return _postRepository.verificarOwnerPost(id, usr, post);
            }
        }
 
 }


