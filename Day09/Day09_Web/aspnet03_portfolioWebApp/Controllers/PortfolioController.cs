using aspnet02_boardapp.Data;
using aspnet03_portfolioWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace aspnet03_portfolioWebApp.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly ApplicationDbContext _db;

        // 파일업로드 웹환경 객체 - 없으면 웹 통한 파일 업로드 불가능함
        private readonly IWebHostEnvironment _environment;

        public PortfolioController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = _db.Portfolios.ToList(); // SELECT *
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // 뷰 만들때 조심해야함 - Index 처럼 PortfolioModel 쓰는거 아님! TempPortflioModel 써야함
            return View();
        }

        [HttpPost]
        public IActionResult Create(TempPortfolioModel temp)
        {
            
            // 파일업로드, Temp -> Model db 저장
            if (ModelState.IsValid)
            {
                // 파일업로드되면 새로운 이미지 파일명을 받음
                string upFileName = UploadImageFile(temp);

                // TempPortfolioModel -> PortfolioModel 변경
                var portfolio = new PortfolioModel()
                {
                    Division = temp.Division,
                    Title = temp.Title,
                    Description = temp.Description,
                    Url = temp.Url,
                    FileName = upFileName // 핵심
                };

                _db.Portfolios.Add(portfolio);
                _db.SaveChanges();

                TempData["succeed"] = "포트폴리오 저장 완료!";
                return RedirectToAction("Index", "Portfolio");
            }

            return View(temp); // 실패하면 화면 그대로
        }

        // Routiong이나 Get/Post랑 관계 없음. 그냥 메서드
        private string UploadImageFile(TempPortfolioModel temp)
        {
            var resultFileName = "";

            try
            {
                if (temp.PortfolioImage != null)
                {
                    string uploadFolder = Path.Combine(_environment.WebRootPath, "uploads"); // wwwroot 밑에 uploads라는 폴더 있어야함 - 추가해서 만들기
                    resultFileName = Guid.NewGuid() + "_" + temp.PortfolioImage.FileName; // 중복된 이미지 파일명 제거하기 위해서 Guid.NewGuid()에 원래 파일명 붙이는것
                    string filePath = Path.Combine(uploadFolder, resultFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        temp.PortfolioImage.CopyTo(fileStream);
                    }
                }
            }
            catch 
            {
                Debug.WriteLine(temp.ToString());
            }
            

            return resultFileName;

        }
    }
}
