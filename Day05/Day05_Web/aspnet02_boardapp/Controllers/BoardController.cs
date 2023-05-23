using aspnet02_boardapp.Data;
using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace aspnet02_boardapp.Controllers
{
    // https://localhost:7125/Board/Index
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BoardController(ApplicationDbContext db)
        {
            _db = db; // 알아서 DB가 연결
        }

        // startCount = 1, 11, 21, 31, 41
        // endcount - 10, 20, 30, 40
        public IActionResult Index(int page = 1) // 게시판 최초화면 리스트
        {
            // EntityFramework로 작업해도 되고
            // IEnumerable<Board> objBoardList = _db.Boards.ToList(); // SELECT * 쿼리
            // SQL쿼리로 작업해도 됨
            // var objBoardList = _db.Boards.FromSql($"SELECT * FROM boards").ToList();
            var totalCount = _db.Boards.Count();
            var pageSize = 10;  // 게시판 한 페이지에 10개씩 리스트
            var totalPage = totalCount / pageSize;  // 34 / 10

            if (totalCount % pageSize > 0) { totalPage++; }     // 나머지글이 있으면 전체 페이지를 1증가

            // 제일 첫번째 페이지, 마지막 페이지
            var countPage = 10;
            var startPage = ((page - 1) / countPage) * countPage + 1;
            var endPage = startPage + countPage - 1;
            if (totalPage < endPage) endPage = totalPage;

            int startCount = ((page -1) * countPage) +1;
            int endCount = startCount + (pageSize - 1);

            // HTML화면에서 사용하기 위해서 선언 == ViewData, TempData 동일한 역할
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;

            var StartCount = new MySqlParameter("startCount", startCount);
            var EndCount = new MySqlParameter("endCount", endCount);

            var objBoardList = _db.Boards.FromSql($"CALL New_PagingBoard({StartCount}, {EndCount})").ToList();

            return View(objBoardList);
        }

        // https://localhost:7125/Board/Create
        // GetMethod로 화면을 URL로 부를때 액션

        [HttpGet]
        public IActionResult Create() // 게시판 글쓰기
        {
            return View();
        }

        // Submit이 발생해서 내부로 데이터를 전달 액션

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Board board)
        {
            try
            {
                board.PostDate = DateTime.Now;  // 현재 저장 일시 할당

                _db.Boards.Add(board);          // INSERT
                _db.SaveChanges();              // COMMIT

                TempData["succed"] = "게시글을 저장했습니다.";      // 성공 메세지
            }
            catch (Exception)
            {

                TempData["error"] = "게시글 작성에 오류발생했습니다.";
            }

            return RedirectToAction("Index", "Board");          // localhost/Board/Index 화면을 이동
        }

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var board = _db.Boards.Find(Id);    // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Board board)
        {
            board.PostDate = DateTime.Now;

            _db.Boards.Update(board);
            _db.SaveChanges();

            TempData["succed"] = "게시글을 수정했습니다.";

            return RedirectToAction("Index", "Board");
        }

        [HttpGet]

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var board = _db.Boards.Find(Id);    // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        [HttpPost]
        public IActionResult DeletePost(int? Id)
        {
            var board = _db.Boards.Find(Id);
            
            if (board == null)
            {
                return NotFound();
            }

            _db.Boards.Remove(board);   // DELETE query 실행
            _db.SaveChanges();          // Commit

            TempData["succed"] = "게시글을 삭제했습니다.";
            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var board = _db.Boards.Find(Id);    // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            board.ReadCount++;          // 조회수 1 증가
            _db.Boards.Update(board);   // UPDATE쿼리 실행
            _db.SaveChanges();

            return View(board);
        }
    }
}