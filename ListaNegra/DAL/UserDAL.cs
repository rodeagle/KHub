using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ListaNegra.DALModels;
using ListaNegra.Models;
using ListaNegra.Repository;
using Microsoft.EntityFrameworkCore;

namespace ListaNegra.DAL
{
    public class UserDAL : IUserDAL
    {
        private AppDbContext _userRepo;

        public UserDAL(IRepositoryContext userRepo) => _userRepo = (AppDbContext)userRepo;

        public Task<bool> CreatePost(Guid userId, ListItemModel model)
        {
            return Task.Run(async () =>
            {
            using (var context = _userRepo) {
                using (var trans = _userRepo.Database.BeginTransaction()) 
                    {
                        try
                        {
                            var post = new Posts()
                            {
                                UserID = userId,
                                Title = model.Title,
                                Latitude = model.Latitude,
                                Longitude = model.Longitude,
                                Phone = model.Phone,
                                PostInfo = model.Description,
                            };

                            context.Posts.Add(post);

                            await context.SaveChangesAsync();

                            var postid = post.PostID;

                            PostTags[] tags = model.Tags.Select(x => new PostTags() { 
                                PostID = postid,
                                Tag = x
                            }).ToArray();

                            context.PostTags.AddRange(tags);


                            ExternalImages[] photos = model.Images.Select(x => new ExternalImages()
                            {
                                PostID = postid,
                                ImageUrl = x
                            }).ToArray();

                            context.ExternalImages.AddRange(photos);

                            await context.SaveChangesAsync();

                            await trans.CommitAsync();

                            return true;

                        }
                        catch (Exception)
                        {
                            await trans.RollbackAsync();
                            throw;
                        }
                    }
                }
            });
        }

        public Task<Posts> GetPost(int postId)
        {
            try
            {
                using (var context = _userRepo) {

                    return Task.Run(async () => {

                        var post = await (
                            from p in context.Posts
                            join pt in context.PostTags
                            on p.PostID equals pt.PostID
                            join ei in context.ExternalImages
                            on p.PostID equals ei.PostID
                            select p
                        ).FirstAsync();

                        return post;

                    });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetUser() {
            return 1;
        }

        public int UserName()
        {
            return _userRepo.AspNetUser.Count();
        }
    }
}