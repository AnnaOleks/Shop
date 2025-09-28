using Shop.Data;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using Shop.Core.ServiceInterface;

namespace Shop.ApplicationServices.Services
{
    public class SpaceshipsServices : ISpaceshipsServices
    {
        private readonly ShopContext _context;
        private readonly IFileServices _fileServices;
        public SpaceshipsServices
            (
                ShopContext context,
                IFileServices fileServices

            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            Spaceship spaceship = new Spaceship();
            spaceship.ID = Guid.NewGuid();
            spaceship.Name = dto.Name;
            spaceship.TypeName = dto.TypeName;
            spaceship.BuiltDate = dto.BuiltDate;
            spaceship.Crew = dto.Crew;
            spaceship.EnginePower = dto.EnginePower;
            spaceship.Passengers = dto.Passengers;
            spaceship.InnerVolume = dto.InnerVolume;
            spaceship.CreatedAt = DateTime.Now;
            spaceship.ModifiedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, spaceship);

            await _context.Spaceships.AddAsync(spaceship);
            await _context.SaveChangesAsync();

            return spaceship;
        }

        public async Task<Spaceship> DetailAsync(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.ID == id);
            return result;

        }
        public async Task<Spaceship> Delete(Guid id)
        {
            //foreach, milles sees toimub failide kustutamine

            var images = await _context.FileToApis
               .Where(x => x.SpaceshipId == dto.SpaceshipId)
               .ToArrayAsync();

            foreach (var image in images)
            {
                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\" + image.ExistingFilePath;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToApis.Remove(image);
            }

            await _context.SaveChangesAsync();

            return null;

        }

        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            Spaceship domain = new();

            domain.ID = dto.ID;
            domain.Name = dto.Name;
            domain.TypeName = dto.TypeName;
            domain.BuiltDate = dto.BuiltDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.Passengers = dto.Passengers;
            domain.InnerVolume = dto.InnerVolume;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, domain);

            _context.Spaceships.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}
