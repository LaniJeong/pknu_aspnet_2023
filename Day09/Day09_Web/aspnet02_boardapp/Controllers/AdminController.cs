using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using NuGet.Protocol;

namespace aspnet02_boardapp.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleManager<IdentityUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, RoleManager<IdentityUser> userManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                {
                    Name = model.RoleName;
                };
                var result = await _roleManager.CreateAsync(role);  // 지정한 권한명이 DB에 저장

                if (result.Succeeded)
                {
                    // TODO : 토스트메시지 추가
                    return RedirectToAction("ListRoles", "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);    // aspnetroles 테이블에서 id 조회
            if (role == null)
            {
                TempData["error"] = $"권한이 없습니다.";
                return NotFound();
            }

            var model = new EditRoleModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            var userList = await _userManager.Users.ToListAsync();      // 사용자리스트

            foreach (var user in userList)
            {
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async async Task<IActionResult>(EditRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(id);    // aspnetroles 테이블에서 id 조회
            if (role == null)
            {
                TempData["error"] = $"권한이 없습니다.";
                return NotFound();
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

    }
}

// 
//@{
//    ViewData["Title"] = "권한목록";
//}
//< div class= "container p-3" >
//    < div class= "row pt-4" >
//        < div class= "col-8" >
//            < h4 class= "text-primary" > rjsgkstodtjd </ h4 >
//        </ div >
//        < div class= "col-4 text-end" >
//        </ div >
//    </ div >
//</ div >
//< hr />
//@if (Model.Any()) {
//    <a class="btn btn-sm btnprimary mb-3" asp-controller="Admin" asp-action="Create Role">
//        <i class= "bi bi-pin-angle" ></ i > &nbsp;권한생성
//    </a>

//    foreach (var role in Model) 
//    {
//      <div class="card mb-3">
//              권한아이디 : @role.Id
//      </div>
//      <div class="card-body>
//          <h5 class="card-title">
//              @role.Name
//          </h5>
//      </div>
//      <div class="card-footer">
//          <form method="post" asp-action="DeleteRole" asp-route-id="@role.Id">
//              <a  class="btn btn-sm-primary" asp-action="EditRole
//                  asp-controller="Admin" asp-route-id="@role.Id">
//                  <i class="bi bi-pencil-square></i>&nbsp;권한편집
//              </a>
//              <button type="submit" class="btn btn-sm btn-danger">
//                  <span class="bi bi-trash3"></span>&nbsp;권한삭제
//              </button>
//          </form>
//      </div>
//  </div>
//    }
//}
//else  @*권한목록이 없으면*@
//{
//  <div class="card mb-3">
//      <div class="card-header">
//          권한목록이 없습니다.
//      </div>
//  <div class="card body">
//      <h5 class="card-title">
//          아래의 권한생성 버튼을 누르세요
//      </h5>
//   </div>
//}

/*  EditRole.cshtml
 @{
    ViewData["Title"] = "권한목록";
}
< div class= "container p-3" >
    < div class= "row pt-4" >
        < div class= "col-8" >
            < h4 class= "text-primary" > rjsgkstodtjd </ h4 >
        </ div >
        < div class= "col-4 text-end" >
        </ div >
    </ div >
</ div >
< hr />
@if (Model.Any()) {
    <a class="btn btn-sm btnprimary mb-3" asp-controller="Admin" asp-action="Create Role">
        <i class= "bi bi-pin-angle" ></ i > &nbsp;권한생성
    </a>

    foreach (var role in Model) 
    {
      <div class="card mb-3">
              권한아이디 : @role.Id
      </div>
      <div class="card-body>
          <h5 class="card-title">
              @role.Name
          </h5>
      </div>
      <div class="card-footer">
          <form method="post" asp-action="DeleteRole" asp-route-id="@role.Id">
              <a  class="btn btn-sm-primary" asp-action="EditRole
                  asp-controller="Admin" asp-route-id="@role.Id">
                  <i class="bi bi-pencil-square></i>&nbsp;권한편집
              </a>
              <button type="submit" class="btn btn-sm btn-danger">
                  <span class="bi bi-trash3"></span>&nbsp;권한삭제
              </button>
          </form>
      </div>
  </div>
    }
}
else  @*권한목록이 없으면*@
{
  <div class="card mb-3">
      <div class="card-header">
          권한목록이 없습니다.
      </div>
  <div class="card body">
      <h5 class="card-title">
          아래의 권한생성 버튼을 누르세요
      </h5>
   </div>
}
 */