using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces.Services;
using Drive.Presentation.Reader;
using System.Text;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Presentation.Actions.Disk
{
    public class CommentAction
    {
        public static void CreateComment(File file, User user, ICommentService commentService)
        {
            Console.Clear();

            Console.WriteLine("Unesite sadrzaj komentara. Prazna linija za spremanje. Prazan prvi red za odustajanje");

            StringBuilder stringBuilder = new StringBuilder();

            while (true)
            {
                var lineOfContent = Console.ReadLine();
                if (string.IsNullOrEmpty(lineOfContent))
                    break;

                stringBuilder.AppendLine(lineOfContent);
            }

            var commentContent = stringBuilder.ToString();

            if (string.IsNullOrEmpty(commentContent))
            {
                Console.WriteLine("Sadrzaj ne moze biti prazan. Povratak...");
                return;
            }

            var createCommentStatus = commentService.AddComment(file.Id, commentContent, file, user);

            if (createCommentStatus == Status.Failed)
            {
                Console.WriteLine("Pogreska prilikom dodavanja komentara");
                return;
            }

            Console.WriteLine("Uspjesno dodan komentar");
        }
        public static void DeleteComment(int fileId, ICommentService commentService, User user)
        {
            var comment = GetComment(fileId, commentService, "izbrisati");
            if (comment == null)
                return;

            Console.Clear();

            if (comment.UserId != user.Id)
            {
                Console.WriteLine("Ne mozete izbrisati komentar koji nije vas");
                return;
            }

            if (!ReadInput.ConfirmAction("zelite li stvarno izbrisati komentar "))
            {
                Console.WriteLine("Odustali ste od brisanja komentara");
                return;
            }

            if (commentService.RemoveComment(comment) == Status.Failed)
            {
                Console.WriteLine("Pogreska prilikom brisanja komentara");
                return;
            }

            Console.WriteLine($"Uspjesno izbrisan komentar: {comment.Id}");
        }
        public static void EditComment(File file, ICommentService commentService, User user)
        {
            var comment = GetComment(file.Id, commentService, "urediti");
            if (comment == null)
                return;

            Console.Clear();

            if (comment.UserId != user.Id)
            {
                Console.WriteLine("Ne mozetet urediti komentar koji nije vas");
                return;
            }

            Console.WriteLine("Unesite novi sadrzaj: (prazno za ostavit isto)");

            var newContent = Console.ReadLine();
            if (string.IsNullOrEmpty(newContent))
            {
                Console.WriteLine("nista nije promijenjeno");
                return;
            }

            if (!ReadInput.ConfirmAction("Zelite li stvarno promijeniti sarzaj komentara "))
            {
                Console.WriteLine("Odustali ste od mijenjanja sadrzaja komentara");
                return;
            }

            if (Status.Failed == commentService.UpdateComment(comment, newContent))
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
            if (!commentsForFile.Any())
            {
                Console.WriteLine("Nema komentara za odabrani file");
                return;
            }

            string header = string.Format("{0,-5} | {1,-30} | {2,-20}", "ID", "Email", "Datum");
            string separator = new string('-', header.Length);

            Console.WriteLine(header);
            Console.WriteLine(separator);

            foreach (var comment in commentsForFile)
            {
                if (comment.User != null)
                {
                    string formattedDate = comment.LastModifiedAt.HasValue
                        ? comment.LastModifiedAt.Value.ToString("dd.MM.yyyy HH:mm")
                        : "problem s dohvacanjem datuma";

                    Console.WriteLine(string.Format("{0,-5} | {1,-30} | {2,-20}",
                        comment.Id,
                        comment.User.Email,
                        formattedDate));


                    Console.WriteLine("Sadržaj:");
                    string content = comment.Content;
                    int lineLength = 1000;//80 

                    for (int i = 0; i < content.Length; i += lineLength)
                    {
                        string line = content.Substring(i, Math.Min(lineLength, content.Length - i));
                        Console.WriteLine(line);
                    }

                    Console.WriteLine("\n" + separator);
                }
            }
        }



        //public static void ShowComments(File file, ICommentService commentService)
        //{
        //    Console.Clear();

        //    var commentsForFile = commentService.GetCommentsByFile(file);
        //    if(!commentsForFile.Any())
        //    {
        //        Console.WriteLine("Nema komentara za odabrani file");
        //        return;
        //    }

        //    Console.WriteLine("Id - Email - Datum (zadnje promjene) - sadrzaj\n");
        //    foreach (var comment in commentsForFile)
        //    {
        //        if (comment.User != null)
        //        {
        //            Console.WriteLine($"\nID: {comment.Id} - Email: {comment.User.Email} - Datum: {comment.LastModifiedAt} \n" +
        //                $"Sadrzaj: {comment.Content}");
        //        }
        //    }
        //}

        public static void CommentCommands(File file, User user, ICommentService _commentService)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine($"Trenutni sadrzaj datoteke: \n {file.Content} \n\n\n-------------------------------------------------------------------------------- \n\n\n" +
                    $"za upravljanje komentarima unesite jednu od komandi: dodaj komentar, uredi komentar, izbrisi komentar ili za vratiti se na prijasnji izbornik ostavite prazno\n");

                var command = Console.ReadLine();
                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine("Ne moze biti prazno. Povratak...");
                    return;
                }

                switch (command)
                {
                    case "dodaj komentar":
                        CreateComment(file, user, _commentService);
                        ReadInput.WaitForUser();

                        break;

                    case "izbrisi komentar":

                        ShowComments(file, _commentService);
                        DeleteComment(file.Id, _commentService, user);
                        ReadInput.WaitForUser();

                        break;

                    case "uredi komentar":
                        ShowComments(file, _commentService);
                        EditComment(file, _commentService, user);
                        ReadInput.WaitForUser();

                        break;

                    default:
                        Console.WriteLine("Ne ispravna komanda. Unesite help za pomoc");
                        break;
                }
            }
        }
    }
}
