using aspnet02_boardapp.Data;
using Microsoft.EntityFrameworkCore;

namespace aspnet02_boardapp
{
    public class Program
    {
        // ASP.NET 실행을 위한 구성 초기화
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Data에서 만든 ApplicationDbContext를 사용하겠다는 설정 추가
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                // appsettings.json ConnectionStrings내의 연결문자열 할당
                builder.Configuration.GetConnectionString("DefaultConnection"),
                // 연결문자열로 DB의 서버 버전을 자동으로 가져올 것
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
            ));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}