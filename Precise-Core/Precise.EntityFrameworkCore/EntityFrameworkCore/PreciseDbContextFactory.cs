using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Precise.Configuration;
using Precise.Web;

namespace Precise.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class PreciseDbContextFactory : IDesignTimeDbContextFactory<PreciseDbContext>
    {
        public PreciseDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PreciseDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            PreciseDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PreciseConsts.ConnectionStringName));

            return new PreciseDbContext(builder.Options);
        }
    }
}