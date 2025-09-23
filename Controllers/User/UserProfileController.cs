using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;
using TechShop.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace TechShop.Controllers.User
{
    [Authorize(Roles = "User")]
    [Route("User")]
    public class UserProfileController : Controller
    {
        private readonly AppDbContext _context;

        public UserProfileController(AppDbContext context)
        {
            _context = context;
        }

        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var username = User.Identity.Name;
            var account = await _context.Accounts
                .Include(a => a.Khachhangs)
                .FirstOrDefaultAsync(a => a.UsernameTk == username);

            if (account == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                FullName = account.Khachhangs.FirstOrDefault()?.FulltenKh,
                Email = account.Gmail,
                PhoneNumber = account.Khachhangs.FirstOrDefault()?.SdtKh,
                Address = account.Khachhangs.FirstOrDefault()?.DiachiKh
            };

            return View("~/Views/User/Profile.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var account = await _context.Accounts
                    .Include(a => a.Khachhangs)
                    .FirstOrDefaultAsync(a => a.UsernameTk == username);

                if (account == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin khách hàng
                var khachhang = account.Khachhangs.FirstOrDefault();
                if (khachhang != null)
                {
                    khachhang.FulltenKh = model.FullName;
                    khachhang.SdtKh = model.PhoneNumber;
                    khachhang.DiachiKh = model.Address;
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Profile));
            }

            return View("~/Views/User/Profile.cshtml", model);
        }
    }
} 