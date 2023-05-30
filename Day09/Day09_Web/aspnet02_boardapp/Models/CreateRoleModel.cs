using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    public class CreateRoleModel
    {
        [Required(ErrorMessage ="권한명은 필수입니다.")]
        [Display]
        public string RoleName { get; set; }    // Admin, User, Manager
    }
}
