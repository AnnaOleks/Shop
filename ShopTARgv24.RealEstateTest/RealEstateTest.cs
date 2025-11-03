using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using System.Threading.Tasks;

namespace ShopTARgv24.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task Test1()
        {
            RealEstateDto dto = new();

            dto.Area = 120.5;
            dto.Location = "Downtown";
            dto.RoomNumber = 3;
            dto.BuildingType = "Apartment";
            dto.CreatedAt = DateTime.Now;
            dto.ModifiedAt = DateTime.Now;

            var result = await Svc<IRealEstateService>().Create(dto);  

            Assert.NotNull(result);
        }
    }
}
