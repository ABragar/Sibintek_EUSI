using System.Data.Entity;
using Data.EF;

namespace Migrations
{
    public class MigrateDatabaseToLatestVersionInitializer : MigrateDatabaseToLatestVersion<DataContext, EFContextConfiguration>
    {
    }
}