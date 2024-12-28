using Drive.Data.Entities;
using Drive.Data.Entities.Models;


namespace Drive.Domain.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        public void AddComment(Comment comment);
        public void DeleteComment(Comment comment);
        public void UpdateComment(Comment comment);
        public Comment GetCommentById(int commentId, int fileId);
        public IEnumerable<Comment> GetAllFileComments(Drive.Data.Entities.Models.File file);
    }
}
