using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.RealEstate;
using Shop.Models.Spaceships;
using System.Collections.Generic;

namespace Shop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopContext _context;
        private readonly IRealEstateService _realEstateService;
        private readonly IFileServices _fileServices;
        public RealEstateController
            (
                ShopContext context,
                IRealEstateService realEstateService,
                IFileServices fileServices
            )
        {
            _context = context;
            _realEstateService = realEstateService;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.RealEstate
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Location = x.Location,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType
                });

            return View(result);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.Id,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()
            };
            var result = await _realEstateService.Create(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realEstate = await _realEstateService.DetailAsync(id);
            if (realEstate == null)
            {
                return NotFound();
            }
            var images = await ImageMethod(id);

            var vm = new RealEstateDeleteViewModel();
            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstate = await _realEstateService.Delete(id);

            if (realEstate == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate = await _realEstateService.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }
            var images = await ImageMethod(id);

            var vm = new RealEstateCreateUpdateViewModel();
            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.Id,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()
            };
            var result = await _realEstateService.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var RealEstate = await _realEstateService.DetailAsync(id);
            if (RealEstate == null)
            {
                return NotFound();
            }
   
            var images = await ImageMethod(id);

            var vm = new RealEstateDetailsViewModel();
            vm.Id = RealEstate.Id;
            vm.Area = RealEstate.Area;
            vm.Location = RealEstate.Location;
            vm.RoomNumber = RealEstate.RoomNumber;
            vm.BuildingType = RealEstate.BuildingType;
            vm.CreatedAt = RealEstate.CreatedAt;
            vm.ModifiedAt = RealEstate.ModifiedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        //[HttpPost]
        //public async Task<IActionResult> RemoveImage(RealEstateImagesViewModel vm)
        //{
        //    var dto = new FileToDatabaseDto()
        //    {
        //        Id = vm.Id
        //    };
        //    var image = await _fileServices.RemoveImageFromApi(dto);
        //    if (image == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpGet]
        public async Task<RealEstateImagesViewModel[]> ImageMethod(Guid id)
        {
            // Проверяем, есть ли вообще данные
            var images = await _context.FileToDatabase
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImagesViewModel
                {
                    RealEstateId = y.RealEstateId,
                    Id = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    // Преобразуем бинарные данные в base64 для отображения в <img>
                    Image = "data:image/gif;base64," + Convert.ToBase64String(y.ImageData)
                })
                .ToArrayAsync();

            // Возвращаем результат
            return images;
        }
    }
}
