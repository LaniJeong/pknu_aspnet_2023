using aspnet02_boardapp.Data;
using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Diagnostics;

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

        // startcount = 1, 11, 21, 31, 41
        // endcout = 10, 20, 30, 40, 50 
        public IActionResult Index(int page = 1) // 게시판 최초화면 리스트
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인스크롤이 안생김

            // EntityFramework로 작업해도 되고
            //IEnumerable<Board> objBoardList = _db.Boards.ToList(); // SELECT * 쿼리
            // SQL 쿼리로 작업해도 됨
            //var objBoardList = _db.Boards.FromSql($"SELECT * FROM boards").ToList();
            var totalCount = _db.Boards.Count(); // 12
            var pageSize = 10; // 게시판 한페이지 10개씩 리스트
            var totalPage = totalCount / pageSize; // 1

            if (totalCount % pageSize > 0) { totalPage++; } // 2

            // 제일 첫번째 페이지, 제일 마지막 페이지
            var countPage = 10;
            var startPage = ((page - 1) / countPage) * countPage + 1; // 12데이터일때 1페이지 startPage = 1
            var endPage = startPage + countPage - 1; // 10
            if (totalPage < endPage) endPage = totalPage; // 10

            int startCount = ((page - 1) * countPage) + 1; // 1, 11
            int endCount = startCount + (pageSize - 1);  // 10, 20

            // HTML화면에서 사용하기 위해서 선언 == ViewData, TempData 동일한 역할
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.StartCount = startCount; // 230525. 게시판 번호를 위해서 새로추가 

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
            ViewData["NoScroll"] = "true"; // 게시판은 메인스크롤이 안생김

            return View();
        }

        // Submit이 발생해서 내부로 데이터를 전달 액션

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Board board)
        {
            try
            {
                board.PostDate = DateTime.Now; // 현재 저장하는 일시를 할당

                _db.Boards.Add(board); // INSERT
                _db.SaveChanges(); // COMMIT

                TempData["succeed"] = "새 게시글이 저장되었습니다."; // 성공메세지
            }
            catch (Exception)
            {
                TempData["error"] = "게시글 작성에 오류발생했습니다.";
            }

            return RedirectToAction("Index", "Board"); // localhost/Board/Index 화면을 이동
        }

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인스크롤이 안생김

            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id

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
            board.PostDate = DateTime.Now; // 날짜를 다시 

            _db.Boards.Update(board);  // UPDATE query실행
            _db.SaveChanges(); // COMMIT

            TempData["succeed"] = "게시글을 수정했습니다."; // 성공메세지

            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {   // HttpGet Edit Action의 로직과 완전 동일
            ViewData["NoScroll"] = "true"; // 게시판은 메인스크롤이 안생김

            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id

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

            _db.Boards.Remove(board); // DELETE query 실행
            _db.SaveChanges(); // Commit

            TempData["succeed"] = "게시글을 삭제했습니다."; // 성공메세지

            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인스크롤이 안생김 

            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id

            if (board == null)
            {
                return NotFound();
            }

            board.ReadCount++;  // 조회수를 1증가
            _db.Boards.Update(board); // UPDATE 쿼리 실행
            _db.SaveChanges(); // COMMIT

            return View(board);
        }
    }
}