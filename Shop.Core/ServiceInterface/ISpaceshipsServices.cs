using Shop.Core.Domain;
using Shop.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface ISpaceshipsServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);
        Task<Spaceship> DetailAsync(Guid id);
        Task<Spaceship> Delete(Guid id);
        Task<Spaceship> Update(SpaceshipDto dto);
    }
}
