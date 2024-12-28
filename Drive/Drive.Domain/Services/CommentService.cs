

using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces.Repositories;
using Drive.Domain.Interfaces.Services;
using System.Runtime.CompilerServices;
using File = Drive.Data.Entities.Models.File;


namespace Drive.Domain.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public Status AddComment(int fileId, string content, Drive.Data.Entities.Models.File file, User user)
        {
            try
            {
                Console.WriteLine($"DEBUG: Attempting to add comment. FileId: {fileId}, UserId: {user.Id}, name: {user.Name}");


                var newComment = new Comment()
                {
                    FileId = fileId,
                    UserId = user.Id,
                    Content = content,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    //File = file
                    //User = user
                };

                _commentRepository.AddComment(newComment);

                return Status.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pogreska prilikom kreiranja komentara: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"DEBUG: Inner exception: {ex.InnerException.Message}");
                }

                return Status.Failed;
            }
        }
        public Status RemoveComment(Comment comment)
        {
            try
            {
                _commentRepository.DeleteComment(comment);
                return Status.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pogreska prilikom brisanja komentara: {ex.Message}");

                return Status.Failed;
            }
        }

        public Status UpdateComment(Comment comment, string newContent)
        {
            try
            {
                comment.Content = newContent;
                comment.LastModifiedAt = DateTime.UtcNow;
                _commentRepository.UpdateComment(comment);

                return Status.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pogreska prilikom kreiranja komentara: {ex.Message}");

                return Status.Failed;
            }
        }
        public Comment GetComment(int commentId, int fileId)
        {
            return _commentRepository.GetCommentById(commentId, fileId);
        }
        public IEnumerable<Comment> GetCommentsByFile(File file)
        {
            return _commentRepository.GetAllFileComments(file);
        }
    }
}
