﻿using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using File = Drive.Data.Entities.Models.File;

namespace Drive.Domain.Interfaces.Services
{
    public interface ICommentService
    {
        public Status AddComment(int fileId, string content, File file, User user);
        public Status RemoveComment(Comment comment);
        public Status UpdateComment(Comment comment, string newContent);
        public Comment GetComment(int commentId, int fileId);
        public IEnumerable<Comment> GetCommentsByFile(File file);
    }
}
