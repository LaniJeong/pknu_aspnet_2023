using aspnet02_boardapp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace aspnet02_boardapp
{
    public class Program
    {
        // ASP.NET ������ ���� ���� �ʱ�ȭ
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Data���� ���� ApplicationDbcontext�� ����ϰڴٴ� ���� �߰�
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                // appsettiong.json ConnectionStrings ���� ���Ṯ�ڿ� �Ҵ�
                builder.Configuration.GetConnectionString("DefaultConnection"),
                // ���Ṯ�ڿ��� DB�� ���� ������ �ڵ����� ������ ��
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
            ));

            // 2. ASP.NET Identity - ASP.NET ������ ���� ���� ����
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // ��й�ȣ ��å ���� ����
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Custom Password Policy
                options.Password.RequireDigit = false; // ������ �ʿ� ����
                options.Password.RequireUppercase = false; // �빮�� �ʿ� ����
                options.Password.RequireLowercase = false; // �ҹ��� �ʿ� ����
                options.Password.RequireNonAlphanumeric = false; // Ư������ �ʿ� ����
                options.Password.RequiredLength = 4; // ��й�ȣ �ּ� ���� �� 4�� ����
                options.Password.RequiredUniqueChars = 0; // ��ȣ ���� ���� ���� 0���� ����
            });

            // ������ ���� ���� �߰�
            builder.Services.AddAuthorization();

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

            app.UseAuthentication(); // 3. ASP.NET Identity - �����߰�
            app.UseAuthorization(); // ����

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}