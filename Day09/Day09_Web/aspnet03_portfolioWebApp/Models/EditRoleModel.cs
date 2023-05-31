using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    public class EditRoleModel
    {
        [DisplayName("권한 아이디")]
        public string Id { get; set; }

        [DisplayName("권한 이름")]
        [Required(ErrorMessage ="권한 이름이 필요합니다.")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }

        public EditRoleModel()
        {
            Users = new List<string>();
        }
    }
}
