using System.ComponentModel.DataAnnotations;

namespace TechShop.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tài khoản là bắt buộc")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}