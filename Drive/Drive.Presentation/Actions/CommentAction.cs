using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Services;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Actions
{
    public class CommentAction
    {
        public static void CreateComment(Drive.Data.Entities.Models.File file, User user, ICommentService commentService)
        {
            Console.WriteLine("Unesite sadrzaj komentara. Prazno za odustajanje");
            var commentContent = Console.ReadLine();

            if (string.IsNullOrEmpty(commentContent))
            {
                Console.WriteLine("Povratak");
                return;
            }

            var createCommentStatus = commentService.AddComment(file.Id, commentContent, file, user);

            if(createCommentStatus == Domain.Enums.Status.Failed)
            {
                Console.WriteLine("Pogreska prilikom dodavanja komentara");
                return;
            }

            Console.WriteLine("Uspjesno dodan komentar");
        }
        public static void DeleteComment(int fileId, ICommentService commentService)
        {
            var comment = GetComment(fileId, commentService, "izbrisati");
            if (comment == null)
                return;

            Console.Clear();

            if (commentService.RemoveComment(comment) == Domain.Enums.Status.Failed)
            {
                Console.WriteLine("Pogreska prilikom brisanja komentara");
                return;
            }

            Console.WriteLine($"Uspjesno izbrisan komentar: {comment.Id} za datoteku: {comment.File.Name}");
        }
        public static void EditComment(File file, ICommentService commentService)
        {
            var comment = GetComment(file.Id, commentService, "urediti");
            if (comment == null)
                return;

            Console.Clear();

            Console.WriteLine("Unesite novi sadrzaj: (prazno za ostavit isto)");
            
            var newContent = Console.ReadLine();
            if (string.IsNullOrEmpty(newContent))
            {
                Console.WriteLine("nista nije promijenjeno");
                return;
            }

            if(Status.Failed == commentService.UpdateComment(comment, newContent))
            {
                Console.WriteLine("Pogreska priliko azuriranja komentara");
                return;
            }

            Console.WriteLine($"Uspjesno azuriran komentar. Novi saadrzaj: {comment.Content}");
        }
        private static Comment? GetComment(int fileId, ICommentService commentService, string message)
        {

            Console.WriteLine($"Unesite id komentara kojeg zelite {message}");

            int commentId;
            var id = int.TryParse(Console.ReadLine(), out commentId);

            var comment = commentService.GetComment(commentId, fileId);
            if (comment == null)
            {
                Console.WriteLine("Komentar nije pronaden");
                return null;
            }

            return comment;
        }
        public static void ShowComments(File file, ICommentService commentService)
        {
            Console.Clear();

            var commentsForFile = commentService.GetCommentsByFile(file);
            if(!commentsForFile.Any())
            {
                Console.WriteLine("Nema komentara za odabrani file");
                return;
            }

            Console.WriteLine("Id - Email - Datum (zadnje promjene) - sadrzaj");
            foreach (var comment in commentsForFile)
            {
                if (comment.User != null)
                {
                    Console.WriteLine($"ID: {comment.Id} - Email: {comment.User.Email} - Datum: {comment.LastModifiedAt} \n" +
                        $"Sadrzaj: {comment.Content}");
                }
            }
        }
    }
}
