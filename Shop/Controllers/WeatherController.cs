using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Models.Weather;

namespace Shop.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherForecastServices _WeatherForecastServices;

        private readonly IWeatherForecastServices _weather;
        public WeatherController
        (IWeatherForecastServices weatherForecastServices)
        {
            _weather = weatherForecastServices;
            _WeatherForecastServices = weatherForecastServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        //teha action SearchCity
        [HttpPost]
        public async Task<IActionResult> SearchCity(AccuWeatherSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("city", "weather", new { city = model.CityName });
            }
            return View(model);

            //var result = await _WeatherForecastServices.AccuWeatherResult(dto);
            //return View("Index", result);
        }
        [HttpGet]
        public IActionResult City(string city)
        {
            AccuLocationWeatherResultDto dto = new();
            dto.CityName = city;

            //_weatherForecastServices.AccuWeatherResult(dto);
            _WeatherForecastServices.AccuWeatherResultWebClient(dto);
            AccuWeatherViewModel vm = new();
            vm.CityName = dto.CityName;
            vm.EffectiveDate = dto.EffectiveDate;
            vm.EffectiveEpochDate = dto.EffectiveEpochDate;
            vm.Severity = dto.Severity;
            vm.Text = dto.Text;
            vm.Category = dto.Category;
            vm.EndDate = dto.EndDate;
            vm.EndEpochDate = dto.EndEpochDate;
            vm.DailyForecastsDate = dto.DailyForecastsDate;
            vm.DailyForecastsEpochDate = dto.DailyForecastsEpochDate;

            vm.TempMinValue = dto.TempMinValue;
            vm.TempMinUnit = dto.TempMinUnit;
            vm.TempMinUnitType = dto.TempMinUnitType;

            vm.TempMaxValue = dto.TempMaxValue;
            vm.TempMaxUnit = dto.TempMaxUnit;
            vm.TempMaxUnitType = dto.TempMaxUnitType;

            vm.DayIcon = dto.DayIcon;
            vm.DayIconPhrase = dto.DayIconPhrase;
            vm.DayHasPrecipitation = dto.DayHasPrecipitation;
            vm.DayPrecipitationType = dto.DayPrecipitationType;
            vm.DayPrecipitationIntensity = dto.DayPrecipitationIntensity;

            vm.NightIcon = dto.NightIcon;
            vm.NightIconPhrase = dto.NightIconPhrase;
            vm.NightHasPrecipitation = dto.NightHasPrecipitation;
            vm.NightPrecipitationType = dto.NightPrecipitationType;
            vm.NightPrecipitationIntensity = dto.NightPrecipitationIntensity;

            vm.MobileLink = dto.MobileLink;
            vm.Link = dto.Link;

            return View(vm);
        }
    }
}