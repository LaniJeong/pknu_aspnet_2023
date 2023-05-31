using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace aspnet02_boardapp.Controllers
{
    // 사용자 회원가입, 로그인, 로그아웃 위함
    public class AccountController : Controller
    {
        // 인덱스 안씀!
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) // 생성자 생성 - null 검사 추가
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        // https://localhost:7100/Account/Register 루트임
        [HttpGet]
        public IActionResult Register()
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        // 비동기 아니면 - public IActionResult Register(RegisterModel model)
        // return값 IAcitonResult
        // 비동기라서 Task<IActionResult>
        public async Task<IActionResult> Register(RegisterModel model)
        {
            ModelState.Remove("PhoneNumber"); // PhoneNumber는 입력값 검증에서 제거 (필수 입력값 아님)
            if (ModelState.IsValid) // 데이터 제대로 입력해서 검증 성공하면 진행
            {
                // ASP.NET user - aspnetusers 테이블에 데이터 넣기 위해 매핑되는 인스턴스를 생성
                var user = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber // 핸드폰 번호 추가
                };
                // aspnetusers 테이블에 사용자 데이터를 대입
                var result = await _userManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                    // 로그인
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["succeed"] = "회원가입 성공했습니다."; // 성공메세지
                    return RedirectToAction("Index", "Home"); // 회원가입 성공했으면 로그인 한 뒤 Home/Index로 화면 전환
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model); // 회원가입을 실패하면 그 화면 그대로 유지
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // 파라미터 순서 : 아이디, 패스워드, isPersistent = RememberMe, 실패할 때 계정잠그기
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    TempData["succeed"] = "로그인 했습니다."; // 성공메세지
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "로그인 실패!");
            }

            return View(model); // 입력검증 실패하면 화면 그대로 유지
            
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["succeed"] = "로그아웃 했습니다."; // 성공메세지
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string userName)
        {
            ViewData["NoScroll"] = "true"; // 게시판은 메인 스크롤 안생김

            Debug.WriteLine(userName);

            var curUser = await _userManager.FindByNameAsync(userName);

            if (curUser == null)
            {
                TempData["error"] = "사용자가 없습니다.";
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterModel()
            {
                Email = curUser.Email,
                PhoneNumber = curUser.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password); // 비밀번호 변경.. 복잡함!
                
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["succeed"] = "프로필 변경 성공했습니다."; // 성공메세지
                    return RedirectToAction("Index", "Home"); // 프로필 변경 성공했으면 로그인 한 뒤 Home/Index로 화면 전환
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model); // 프로필 변경 실패하면 화면 유지
        }
    }
}
