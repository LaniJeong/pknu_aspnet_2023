using aspnet02_boardapp.Data;
using Microsoft.AspNetCore.Identity;
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

            // 2. ASP>NET Identity -> ASP.NET 계정을 위한 서비스 설정
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // 비밀번호 정책 변경 설정
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Custom Password policy   // 실무에서는 안됨
                options.Password.RequireDigit = false;  // 영문자 필요여부
                options.Password.RequireLowercase = false;  // 소문자 필요 여부
                options.Password.RequireUppercase = false;  // 대문자 필요 여부
                options.Password.RequireNonAlphanumeric = false;    // 특수문자 필요 여부
                options.Password.RequiredLength = 4;    // 최소 글자수
                options.Password.RequiredUniqueChars = 0;   // 고유문자 갯수
            });

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
            app.UseAuthentication();    // 계정추가
            app.UseAuthorization();     // 권한

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}