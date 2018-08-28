using System.IO;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            //read Mode from configure file 
            var path = Directory.GetCurrentDirectory() + "/../WebApi/";
            var builder = new ConfigurationBuilder().SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                        
            var config = builder.Build();
            optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"));
            return new AppDbContext(optionsBuilder.Options);
        }

    }
}