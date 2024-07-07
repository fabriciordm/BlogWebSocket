using Microsoft.EntityFrameworkCore;
using ClientBLog.Models;

using ClientBLog.Models;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBLog.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ClientBLog.Data.DataContext _context;

        public PostRepository(ClientBLog.Data.DataContext context)
        {
            _context = context;
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            try
            {
                 _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { }
        }
        
        public async Task UpdatePostAsync(Post post)
        {
            var existingPost = await _context.Posts.FindAsync(post.Id);
            if (existingPost != null)
            {
                try
                {
                    existingPost.Title = post.Title;
                    existingPost.Content = post.Content;

                    var existingUser = await _context.Users.FindAsync(post.User.Id);
                    if (existingUser != null)
                    {
                        existingPost.User = existingUser;
                    }

                    _context.Entry(existingPost).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("An error occurred while updating the post.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred.", ex);
                }
            }
            else
            {
                throw new Exception("Post not found");
            }
        }


        //public async Task UpdatePostAsync(Post post)
        //{
        //    var existingPost = await _context.Posts.FindAsync(post.Id);
        //    if (existingPost != null)
        //    {
        //        // Atualize as propriedades da entidade existente com as novas propriedades
        //        existingPost.Title = post.Title;
        //        existingPost.Content = post.Content;
        //        existingPost.User = post.User;
        //        // Atualize outras propriedades conforme necessário

        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        throw new Exception("Post not found");
        //    }
        //}

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public bool verificarOwnerPost(int id, User usr, Post post)
        {

            var retorno =  GetPostByIdAsync(id);

           

            if (retorno.Result.UserId == usr.Id)
                return true;
            else
                return false;
           
        }
    }
}
