using Shop.Core.Domain;
using Shop.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface IKindergardenServices
    {
        Task<Kindergarden> Create(KindergardenDto dto);
        Task<Kindergarden> DetailAsync(Guid id);
        Task<Kindergarden> Delete(Guid id);
        Task<Kindergarden> Update(KindergardenDto dto);
    }
}
