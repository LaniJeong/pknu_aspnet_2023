using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    public class CreateRoleModel
    {
        [Required(ErrorMessage = "권한명은 필수입니다.")]
        [DisplayName("권한명")]
        public string RoleName { get; set; } // Admin, User, Manager ...
    }
}
