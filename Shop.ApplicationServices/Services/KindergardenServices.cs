using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Microsoft.EntityFrameworkCore;


namespace Shop.ApplicationServices.Services
{
    public class KindergardenServices : IKindergardenServices
    {
        private readonly ShopContext _context;
        private readonly IFileServices _fileServices;

        // teha constructor
        public KindergardenServices
            (
                ShopContext  context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }
        public async Task<Kindergarden> Create(KindergardenDto dto)
        {
            Kindergarden kindergarden = new Kindergarden();

            kindergarden.Id = Guid.NewGuid();
            kindergarden.GroupName = dto.GroupName;
            kindergarden.ChildrenCount = (int)dto.ChildrenCount;
            kindergarden.KindergardenName = dto.KindergardenName;
            kindergarden.TeacherName = dto.TeacherName;
            kindergarden.CreatedAt = DateTime.Now;
            kindergarden.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, kindergarden);
            }

            await _context.Kindergardens.AddAsync(kindergarden);
            await _context.SaveChangesAsync();

            return kindergarden;
        }

        public async Task<Kindergarden> DetailAsync(Guid id)
        {
            var result = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
        public async Task<Kindergarden> Delete(Guid id)
        {
            var remove = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergardens.Remove(remove);
            await _context.SaveChangesAsync();

            return remove;
        }
        public async Task<Kindergarden> Update(KindergardenDto dto)
        {
            Kindergarden domain = new();

            domain.Id = (Guid)dto.Id;
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = (int)dto.ChildrenCount;
            domain.KindergardenName = dto.KindergardenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = dto.CreatedAt;
            domain.UpdatedAt = DateTime.Now;

            _context.Kindergardens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }
    }
}
