using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class SettingRepository : GenericRepository<Settings>, ISettingRepository
    {
        private readonly ApplicationDbContext _context;
        public SettingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Settings settings)
        {
            var settingsInDb = _context.Settings.FirstOrDefault(x => x.Name == settings.Name);
            if (settingsInDb != null)
            {
                settingsInDb.Value = settings.Value;
            }
        }
    }
}
