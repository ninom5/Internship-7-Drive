using Drive.Domain.Enums;
using Drive.Data.Entities.Models;
using Drive.Domain.Interfaces.Services;
using Drive.Domain.Interfaces.Repositories;

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
                Console.WriteLine($"Pogreska prilikom kreiranja foldera: {ex.Message}");
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
            catch (Exception ex)
            {
                Console.WriteLine($"Pogreksa prilikom brisanja foldera: {ex.Message}");
                return Status.Failed;
            }
        }
        public Status UpdateFolder(Folder folder, string name)
        {
            try
            {
                _folderRepository.UpdateFolder(folder, name);
                return Status.Success;
            }catch(Exception ex)
            {
                Console.WriteLine($"Pogreksa prilikom azuriranja foldera: {ex.Message}");
                return Status.Failed; 
            }
        }
        public Folder GetFolderByName(string name, User user)
        {
            return _folderRepository.GetFolderByName(name, user);
        }
    }
}
