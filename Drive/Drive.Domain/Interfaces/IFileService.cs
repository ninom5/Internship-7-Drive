using Drive.Data.Entities.Models;
using Drive.Domain.Enums;


namespace Drive.Domain.Interfaces
{
    public interface IFileService
    {
        public Status CreateFile(string fileName, string content, User user, Folder folder);
    }
}
