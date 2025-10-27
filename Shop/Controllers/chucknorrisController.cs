using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Models.chucknorris;
using Shop.Models.Weather;

namespace Shop.Controllers
{
    public class chucknorrisController : Controller
    {
        private readonly IchucknorrisServices _chucknorrisServices;

        public chucknorrisController
        (IchucknorrisServices chucknorrisServices)
        {
            _chucknorrisServices = chucknorrisServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            chucknorrisDto dto = await _chucknorrisServices.GetRandomAsync();

            // создаём модель для отображения
            var vm = new chucknorrisViewModel
            {
                value = dto.value,
                icon_url = dto.icon_url,
                created_at = dto.created_at,
                updated_at = dto.updated_at

            };

            // возвращаем вью ChuckJoke.cshtml
            return View("Index", vm);
        }

        // Кнопка для загрузки новой шутки
        [HttpPost]
        public IActionResult NewJoke()
        {
            return RedirectToAction("Index");
        }
    }
}