﻿using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Drive.Domain.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DriveDbContext dbContext) : base(dbContext)
        {
        }

        public void AddComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }
        public void DeleteComment(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
        }
        public void UpdateComment(Comment comment)
        {
            _dbContext.Comments.Update(comment);
            _dbContext.SaveChanges();
        }
        public Comment GetCommentById(int commentId, int fileId)
        {
            return _dbContext.Comments.Where(item => item.Id == commentId && item.FileId == fileId).FirstOrDefault();
        }
        public IEnumerable<Comment> GetAllFileComments(Drive.Data.Entities.Models.File file)
        {
            return _dbContext.Comments.Where(item => item.File == file).Include(item => item.User).ToList();
        }


        public void TrackedUser(User user)
        {
            var existingUser = _dbContext.Users.Local.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                // Detach the existing user instance
                _dbContext.Entry(existingUser).State = EntityState.Detached;
            }
            _dbContext.Attach(user);
        }
    }
}
