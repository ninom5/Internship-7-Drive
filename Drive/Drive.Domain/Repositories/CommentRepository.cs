using Drive.Data.Entities;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Repositories;

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
    }
}
