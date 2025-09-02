using Microsoft.AspNetCore.Mvc;
using Shop.Models.Spaceships;
using Shop.Data;


namespace Shop.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly ShopContext _context;
        public SpaceshipsController
            (
                ShopContext context
            )
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipsIndexViewModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    BuiltDate = x.BuiltDate,
                    TypeName = x.TypeName
                });

            return View(result);
        }
    }
}
