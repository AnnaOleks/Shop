using Shop.Core.ServiceInterface;
using Shop.Data;


namespace Shop.ApplicationServices.Services
{
    public class KindergardenServices : IKindergardenServices
    {
        private readonly ShopContext _context;
        private readonly IKindergardenServices _kindergardenservice;

        public KindergardenServices
            (
                ShopContext context,
                IKindergardenServices kindergardenservice
            )
        {
            _context = context;
            _kindergardenservice = kindergardenservice;
        }
    }
}
