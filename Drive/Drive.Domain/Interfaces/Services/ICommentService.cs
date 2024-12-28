using Drive.Data.Entities.Models;
using Drive.Domain.Enums;

namespace Drive.Domain.Interfaces.Services
{
    public interface ICommentService
    {
        public Status AddComment(int fileId, string content, Drive.Data.Entities.Models.File file, User user);
        public Status RemoveComment(Comment comment);
        public Status UpdateComment(Comment comment, string newContent);
        public Comment GetComment(int commentId, int fileId);
        public IEnumerable<Comment> GetCommentsByFile(Drive.Data.Entities.Models.File file);
    }
}
