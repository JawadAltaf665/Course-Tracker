using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CourseTracker.EntityFrameworkCore
{
    public static class CourseTrackerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CourseTrackerDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CourseTrackerDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
