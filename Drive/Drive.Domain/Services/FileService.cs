using Drive.Data.Entities.Models;
using Drive.Domain.Enums;
using Drive.Domain.Interfaces.Repositories;
using Drive.Domain.Interfaces.Services;
using File = Drive.Data.Entities.Models.File;

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
        public Status DeleteFile(Drive.Data.Entities.Models.File file)
        {
            try
            {
                _fileRepository.Remove(file);

                return Status.Success;
            }
            catch
            {
                return Status.Failed;
            }
        }
        public Status UpdateFile(Drive.Data.Entities.Models.File file, string newName)
        {
            try
            {
                file.Name = newName;
                _fileRepository.Update(file);

                return Status.Success;
            }
            catch
            {
                return Status.Failed;
            }
        }
        public Status UpdateFileContent(Drive.Data.Entities.Models.File file)
        { 
            try
            {
                _fileRepository.Update(file);
                return Status.Success;
            }
            catch
            {
                return Status.Failed;
            }
        }
        public File GetFileByName(string name, User user)
        {
            return _fileRepository.GetFileByName(name, user);
        }
    }
}
