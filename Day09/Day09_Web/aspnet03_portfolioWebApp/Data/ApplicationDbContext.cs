using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using aspnet02_boardapp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using aspnet03_portfolioWebApp.Models;

namespace aspnet02_boardapp.Data
{
    public class ApplicationDbContext : IdentityDbContext // 1. ASP.NET Identity : DbContext -> IdentityDbContext 로 변경
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Board> Boards { get; set; }

        // 포트폴리오를 DB로 관리하기 위한 모델
        public DbSet<PortfolioModel> Portfolios { get; set; } 

        // 포트폴리오를 DB로 관리하기 위한 모델
        //public DbSet<aspnet03_portfolioWebApp.Models.TempPortfolioModel>? TempPortfolio { get; set; }
    }
}
