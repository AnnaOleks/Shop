using Microsoft.Extensions.Hosting;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using System.Xml;


namespace Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopContext _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                ShopContext context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship spaceship)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    // muutuja string uploadFolder ja siina laetakse failid
                    string uploadFolder = Path.Combine(_webHost.ContentRootPath, "multipleFileUpload");

                    //muutuja string uniqueFileName ja siin genereeritakse uus Guid ja lisatakse see faili ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;

                    // muutuja string filePath kombineeritakse ja lisatakse koos kausta unikaalse nimega
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = spaceship.ID
                        };

                        _context.FileToApisKinder.AddAsync(path);
                    }
                }
            }
        }
        public void UploadFilesToDatabase(KindergardenDto dto, Kindergarden domain)
        {
            //tuleb ara kontrollida, kas on uks fail voi mitu
            if (dto?.Files != null || dto.Files.Count > 0)
            {
                //kui tuleb mitu faili, siis igaks juhuks tuleks kasutada foreachi
                foreach (var file in dto.Files)
                {
                    //foreach sees kasutada using-t ja ara mappida
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            KindergardenId = domain.Id
                        };
                        //salvestada andmed andmebaasi
                        file.CopyTo(target);
                        files.ImageData = target.ToArray();
                        _context.FileToDataKinder.Add(files);
                    }
                }
            }
        }
    }
}