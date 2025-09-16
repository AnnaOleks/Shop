using Shop.Core.ServiceInterface;
using Shop.Data;


namespace Shop.ApplicationServices.Services
{
    public class KindergardenServices : IKindergardenServices
    {
        private readonly ShopContext _context;
        private readonly IKindergardenServices _kgservice;

        public KindergardenServices
            (
                ShopContext context,
                IKindergardenServices kgservice
            )
        {
            _context = context;
            _kgservice = kgservice;
        }
    }
}
