using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    // 회원가입할 때 데이터 받는 부분
    public class RegisterModel
    {
        [Required(ErrorMessage = "이메일주소는 필수입니다.")]
        [EmailAddress]
        [Display(Name = "이메일 주소")]
        public string Email { get; set; }

        [Display(Name = "핸드폰번호")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "패스워드는 필수입니다.")]
        [DataType(DataType.Password)]
        [Display(Name = "패스워드")]
        public string Password { get; set; }

        [Required(ErrorMessage = "패스워드 확인은 필수입니다.")]
        [DataType(DataType.Password)]
        [Display(Name = "패스워드 확인")]
        [Compare("Password", ErrorMessage = "패스워드가 일치하지 않습니다.")]
        public string ConfirmPassword { get; set; }

    }
}
