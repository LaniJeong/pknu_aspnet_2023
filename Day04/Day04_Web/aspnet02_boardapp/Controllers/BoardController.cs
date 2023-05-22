using aspnet02_boardapp.Data;
using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnet02_boardapp.Controllers
{
    // https://localhost:7800/Board/Index
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BoardController(ApplicationDbContext db)
        {
            _db = db;   // 알아서 DB생성
        }

        // 게시판 최초화면 리스트
        public IActionResult Index()
        {
            IEnumerable<Board> objBoardList = _db.Boards.ToList();   // SELECT쿼리
            return View(objBoardList);
        }

        // 게시판 글쓰기
        public IActionResult Create()
        {
            return View();
        }
    }
}
