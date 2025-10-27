using Shop.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface IWeatherForecastServices
    {
        Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto);
        Task<AccuLocationWeatherResultDto> AccuWeatherResultWebClient(AccuLocationWeatherResultDto dto);
    }
}
