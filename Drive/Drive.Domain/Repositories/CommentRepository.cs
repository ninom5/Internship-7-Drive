using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using File = Drive.Data.Entities.Models.File;

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
        public IEnumerable<Comment> GetAllFileComments(File file)
        {
            return _dbContext.Comments.Where(item => item.File == file).Include(item => item.User).ToList();
        }
    }
}
