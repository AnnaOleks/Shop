using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateService
    {
        private readonly ShopContext _context;
        private readonly IFileServices _fileServices;
        public RealEstateServices
            (
                ShopContext context,
                IFileServices fileServices

            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate RealEstate = new RealEstate();
            RealEstate.Id = Guid.NewGuid();
            RealEstate.Area = dto.Area;
            RealEstate.Location = dto.Location;
            RealEstate.RoomNumber = dto.RoomNumber;
            RealEstate.BuildingType = dto.BuildingType;
            RealEstate.CreatedAt = DateTime.Now;
            RealEstate.ModifiedAt = DateTime.Now;

            await _context.RealEstate.AddAsync(RealEstate);
            await _context.SaveChangesAsync();

            return RealEstate;
        }

        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var result = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);
            return result;

        }
        public async Task<RealEstate> Delete(Guid id)
        {
            var RealEstate = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.RealEstate.Remove(RealEstate);
            await _context.SaveChangesAsync();

            return RealEstate;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate domain = new();

            domain.Id = dto.Id;
            domain.Area = dto.Area;
            domain.Location = dto.Location;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = DateTime.Now;

            _context.RealEstate.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

    }
}
