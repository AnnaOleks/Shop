using Shop.Core.Domain;
using Shop.Core.Dto;
using System.Xml;

namespace Shop.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceship spaceship);
        void UploadFilesToDatabase(KindergardenDto dto, Kindergarden domain);
        Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto);
        Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos);
    }
}
