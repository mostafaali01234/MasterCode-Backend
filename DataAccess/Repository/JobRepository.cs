using DataAccess.Data;
using Entities.Models;
using Entities.IRepository;

namespace DataAccess.Repository
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        private readonly ApplicationDbContext _context;
        public JobRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Job job)
        {
            var JobInDb = _context.Job.FirstOrDefault(x => x.Id == job.Id);
            if (JobInDb != null)
            {
                JobInDb.Name = job.Name;
                JobInDb.Description = job.Description;
                JobInDb.CreatedTime = DateTime.Now;
            }
        }
    }
}
