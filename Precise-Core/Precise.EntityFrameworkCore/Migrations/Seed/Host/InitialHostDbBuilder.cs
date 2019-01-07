using Precise.EntityFrameworkCore;

namespace Precise.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly PreciseDbContext _context;

        public InitialHostDbBuilder(PreciseDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
