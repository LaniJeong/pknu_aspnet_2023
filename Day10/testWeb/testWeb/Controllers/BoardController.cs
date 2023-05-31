using aspnet02_boardapp.Data;
using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Security.AccessControl;

namespace aspnet02_boardapp.Controllers
{
    // https://localhost:7800/Board/Index
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BoardController(ApplicationDbContext db)
        {
            _db = db; // 알아서 DB 연결
        }

        // startcount = 1, 11, 21, 31, 41.. 순으로
        // endcount = 10, 20, 30, 40.. 순으로
        public IActionResult Index(int page = 1) // 게시판 최초화면 리스트
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김

            // SELECT 쿼리
            // 1. EntityFramework로 작업 :  IEnumerable<Board> objBoardList = _db.Boards.ToList();
            // 2. SQL 쿼리로 작업 
            // var objBoardList = _db.Boards.FromSql($"SELECT * FROM boards").ToList();

            var totalCount = _db.Boards.Count();
            var pageSize = 10; // 게시판 한 페이지에 노출되는 글 10개
            var totalPage = totalCount / pageSize;

            if (totalCount % pageSize > 0) { totalPage++; } // 나머지글 있으면 전체페이지 1 증가

            var countPage = 10;
            var startPage = ((page - 1) / countPage) * countPage + 1;
            var endPage = startPage + countPage - 1;
            if (totalPage < endPage) endPage = totalPage;

            int startCount = ((page - 1) * countPage) + 1;
            int endCount = startCount + (pageSize - 1);

            // HTML화면에서 데이터 사용하기 위해서 선언 == ViewData, TempData와 동일한 역할
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;
            ViewBag.Page = page;
            ViewBag.TotalPage = totalPage;
            ViewBag.StartCount = startCount; // 230525 게시판 글 번호 노출 위해 추가

            var StartCount = new MySqlParameter("startCount", startCount);
            var EndCount = new MySqlParameter("endCount", endCount);

            var objBoardList = _db.Boards.FromSql($"CALL New_PagingBoard({StartCount}, {EndCount})").ToList();

            return View(objBoardList);
        }

        // https:/localhost:7100/Board/Create
        // GetMethod로 화면을 URL로 부를 때 액션
        public IActionResult Create() // 게시판 글쓰기
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김
            return View();
        }

        // submit이 발생해서 내부로 데이터 전달 액션
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
            catch (Exception ex)
            {
                TempData["Error"] = "게시글 작성에 오류가 발생했습니다.";
            }
           
            return RedirectToAction("Index", "Board"); // localhost/Board/Index 화면 이동
        }

        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김

            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id 랑 동일

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
            board.PostDate = DateTime.Now; // 날짜 다시 현재날짜로 
            _db.Boards.Update(board); // UPDATE query 실행
            _db.SaveChanges(); // 커밋

            TempData["succeed"] = "게시글을 수정했습니다."; // 성공메세지


            return RedirectToAction("Index", "Board"); // localhost/Board/Index 로 화면 전환!
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {   
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김

            // HttpGet Edit Action 로직과 완전 동일
            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id 랑 동일

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
            _db.SaveChanges(); // 커밋

            TempData["succeed"] = "게시글을 삭제했습니다."; // 성공메세지


            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김

            if (Id == null || Id == 0)
            {
                return NotFound(); // Error.cshtml이 표시
            }

            var board = _db.Boards.Find(Id); // SELECT * FROM board WHERE Id = @id 랑 동일

            if (board == null)
            {
                return NotFound();
            }

            board.ReadCount++; // 조회수 1 증가
            _db.Boards.Update(board); // UPDATE query 실행
            _db.SaveChanges(); // 커밋

            return View(board);
        }

    }
}
