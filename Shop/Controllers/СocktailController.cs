using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Models.cocktails;



namespace Shop.Controllers
{
    public class CocktailController : Controller
    {
        private readonly IcocktailServices _cocktailServices;

        public CocktailController(IcocktailServices cocktailServices)
        {
            _cocktailServices = cocktailServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCocktail(cocktailSearchViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.strDrink))
            {
                // Покажем ту же форму с ошибкой
                return View("Index", model);
            }

            return RedirectToAction("Cocktail", new { cocktail = model.strDrink!.Trim() });
        }

        [HttpGet]
        public async Task<IActionResult> Cocktail(string? cocktail)
        {
            if (string.IsNullOrWhiteSpace(cocktail))
            {
                // Вернёмся на форму, если параметр пустой
                ModelState.AddModelError(nameof(cocktailSearchViewModel.strDrink), "Sisesta kokteili nimi.");
                return View("Index");
            }

            try
            {
                var dto = new cocktailDto { strDrink = cocktail };
                var result = await _cocktailServices.GetCocktailAsync(dto);

                if (result == null || string.IsNullOrWhiteSpace(result.strDrink))
                {
                    return View(new cocktailViewModel { strDrink = $"Pole leitud: {cocktail}" });
                }

                var vm = new cocktailViewModel
                {
                    strDrink = result.strDrink,
                    strGlass = result.strGlass,
                    strCategory = result.strCategory,
                    strAlcoholic = result.strAlcoholic,
                    strInstructions = result.strInstructions,
                    strDrinkThumb = result.strDrinkThumb
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                // Можно залогировать ex
                return View(new cocktailViewModel { strDrink = $"Tekkis viga päringuga: {ex.Message}" });
            }
        }
    }
}

           