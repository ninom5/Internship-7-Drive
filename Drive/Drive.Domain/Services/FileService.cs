﻿using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces;

namespace Drive.Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public Status CreateFile(string name, string content,  User user, Folder folder)
        {
            try
            {
                var newFile = new Drive.Data.Entities.Models.File
                {
                    Name = name,
                    Content = content,
                    FolderId = folder.Id,
                    OwnerId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    Folder = folder,
                    Owner = user
                };

                _fileRepository.AddFile(newFile);

                return Status.Success;
            }
            catch 
            {
                return Status.Failed;
            }
        }
    }
}
