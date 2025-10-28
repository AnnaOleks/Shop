using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Shop.ApplicationServices.Services
{

    public class cocktailServices : IcocktailServices
    {
        public async Task<cocktailDto> GetCocktailAsync(cocktailDto dto)
        {
            using var http = new HttpClient();
            var url = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={dto.strDrink}";
            var resp = await http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return dto;

            var json = await resp.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<cocktailDto.Rootobject>(json);

            dto.Raw = root;

            var drink = root?.drinks?.FirstOrDefault();
            if (drink is null) return dto;

            dto.strDrink = drink.strDrink;
            dto.strGlass = drink.strGlass;
            dto.strCategory = drink.strCategory;
            dto.strAlcoholic = drink.strAlcoholic;
            dto.strInstructions = drink.strInstructions;
            dto.strDrinkThumb = drink.strDrinkThumb;

            return dto;
        }
    }
}
