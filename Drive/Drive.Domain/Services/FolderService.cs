using Drive.Domain.Enums;
using Drive.Domain.Interfaces;
using Drive.Data.Entities.Models;

namespace Drive.Domain.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        public FolderService(IFolderRepository folderRepository) 
        {
            _folderRepository = folderRepository;
        }

        public Status CreateFolder(string folderName, User user, Folder? parentFolder)
        {
            try
            {
                var newFolder = new Folder
                {
                    Name = folderName,
                    OwnerId = user.Id,
                    ParentFolderId = parentFolder?.Id,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    Owner = user,
                    ParentFolder = parentFolder
                };

                _folderRepository.AddFolder(newFolder);

                return Status.Success;
            }
            catch (Exception ex)
            {
                return Status.Failed;
            }
        }
        public Status DeleteFolder(Folder folder)
        {
            try
            {
                _folderRepository.RemoveFolder(folder);
                return Status.Success;
            }
            catch
            {
                return Status.Failed;
            }
        }
        public Status UpdateFolder(Folder folder, string name)
        {
            try
            {
                _folderRepository.UpdateFolder(folder, name);
                return Status.Success;
            }catch
            { return Status.Failed; }
        }
    }
}
