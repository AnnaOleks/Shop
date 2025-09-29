using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shop.Core.Domain;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Kindergarden;
using Shop.Models.Spaceships;

namespace Shop.Controllers
{
    public class KindergardenController : Controller
    {
        private readonly ShopContext _context;
        private readonly IKindergardenServices _kindergardenServices;
        public KindergardenController
            (
                ShopContext context,
                IKindergardenServices kindergardenServices
            )
        {
            _context = context;
            _kindergardenServices = kindergardenServices;
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
        
    }
}
