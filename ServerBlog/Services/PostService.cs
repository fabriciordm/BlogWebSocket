using ServerBlog.Models;
using ServerBlog.Notifications;
using ServerBlog.Repositories;
using ServeBlog.Models;

using ServeBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerBlog.Services
{
    //public class PostService : IPostService
    //{
    //    private readonly IPostRepository _postRepository;
    //    private readonly INotificationService _notificationService;
    //    //private readonly NotificationService _notificationService;


    //    //public PostService(IPostRepository postRepository, NotificationService notificationService)
    //    public PostService(IPostRepository postRepository, INotificationService notificationService)
    //    {
    //        _postRepository = postRepository;
    //        _notificationService = notificationService;
    //    }

    //    public async Task<Post> GetPostByIdAsync(int id)
    //    {
    //        return await _postRepository.GetPostByIdAsync(id);
    //    }

    //    public async Task<IEnumerable<Post>> GetPostsAsync()
    //    {
    //        return await _postRepository.GetPostsAsync();
    //    }

    //    public async Task AddPostAsync(Post post)
    //    {
    //        await _postRepository.AddPostAsync(post);

    //        _notificationService.NotificationServiceMessage($"Nova postagem criada: {post.Title}");


    //    }

    //    //public bool  UpdatePostAsync(int id ,Post post, User usr)
    //    //{
    //    //    var post_ = GetPostByIdAsync(id);
    //    //    if (post_.Result.UserId == usr.Id)
    //    //    {
    //    //        _postRepository.UpdatePostAsync(post);
    //    //        return true;
    //    //    }
    //    //    else
    //    //        return false;
    //    //    //await _postRepository.UpdatePostAsync(post);
    //    //}

    //    public async Task UpdatePostAsync(Post post)
    //    {
    //       await _postRepository.UpdatePostAsync(post);

    //       _notificationService.NotificationServiceMessage($"Postagem atualizada {post.Title}");          
    //    }

    //    public bool DeletePostAsync(int id, User usr)
    //    {
    //        var post = GetPostByIdAsync(id);
    //        if (post.Result.UserId == usr.Id)
    //        {
    //            _postRepository.DeletePostAsync(id);
    //            _notificationService.NotificationServiceMessage($"Postagem deletada ");                
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }



    //    public bool verificarOwnerPost(int id, User usr, Post post)
    //    {
    //       return _postRepository.verificarOwnerPost(id, usr, post);
    //    }
    //}



    
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
                await _notificationService.NotificationServiceMessage($"Nova postagem criada: {post.Title}");
            }

            public async Task UpdatePostAsync(Post post)
            {
                await _postRepository.UpdatePostAsync(post);
                await _notificationService.NotificationServiceMessage($"Postagem atualizada: {post.Title}");
            }



            public async Task<bool> DeletePostAsync(int id, User usr)
            {
                var post = await GetPostByIdAsync(id);
                if (post.UserId == usr.Id)
                {
                    await _postRepository.DeletePostAsync(id);
                    await _notificationService.NotificationServiceMessage("Postagem deletada");
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //public bool VerificarOwnerPost(int id, User usr, Post post)
            //{
            //    return _postRepository.VerificarOwnerPost(id, usr, post);
            //}

            public bool verificarOwnerPost(int id, User usr, Post post)
            {
                return _postRepository.verificarOwnerPost(id, usr, post);
            }
        }
 
    }


