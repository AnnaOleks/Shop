using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Kindergarden;


namespace Shop.Controllers
{
    public class KindergardenController : Controller
    {
        private readonly ShopContext _context;
        private readonly IKindergardenServices _kindergardenServices;
        private readonly IFileServices _fileServices;
        public KindergardenController
            (
                ShopContext context,
                IKindergardenServices kindergardenServices,
                IFileServices fileServices
            )
        {
            _context = context;
            _kindergardenServices = kindergardenServices;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergardens
                .Select(x => new KindergardenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergardenName = x.KindergardenName,
                    TeacherName = x.TeacherName
                });

            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            KindergardenCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(KindergardenCreateUpdateViewModel vm)
        {
            var dto = new KindergardenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergardenName = vm.KindergardenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergardenId = x.KindergardenId
                    }).ToArray()
            };

            var result = await _kindergardenServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var update = await _kindergardenServices.DetailAsync(id);

            if (update == null)
            {
                return NotFound();
            }

            KindergardenImagesViewModel[] images = await ImageMethod(id);

            var vm = new KindergardenCreateUpdateViewModel();

            vm.Id = update.Id;
            vm.GroupName = update.GroupName;
            vm.ChildrenCount = update.ChildrenCount;
            vm.KindergardenName = update.KindergardenName;
            vm.TeacherName = update.TeacherName;
            vm.CreatedAt = update.CreatedAt;
            vm.UpdatedAt = update.UpdatedAt;

            vm.Image ??= new List<KindergardenImagesViewModel>();
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergardenCreateUpdateViewModel vm)
        {
            var dto = new KindergardenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergardenName = vm.KindergardenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        KindergardenId = x.KindergardenId
                    }).ToArray()
            };

            var result = await _kindergardenServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), vm);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergardenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergardenImagesViewModel[] images = await ImageMethod(id);

            var vm = new KindergardenDetailsViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergardenName = kindergarten.KindergardenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;

            vm.Image.AddRange(images);

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergardenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergardenImagesViewModel[] images = await ImageMethod(id);

            var vm = new KindergardenDeleteViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergardenName = kindergarten.KindergardenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var deleted = await _kindergardenServices.Delete(id);
            if (deleted == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergardenImagesViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };
            var image = await _fileServices.RemoveImageFromDatabase(dto);
            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<KindergardenImagesViewModel[]> ImageMethod(Guid id)
        {
            // Проверяем, есть ли вообще данные
            var images = await _context.FileToDataKinder
                .Where(x => x.KindergardenId == id)
                .Select(y => new KindergardenImagesViewModel
                {
                    KindergardenId = y.KindergardenId,
                    ImageId = y.Id,
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
