using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManger, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            this._userManger = userManger;
            this._signInManager = signInManager;
            this._context = context;
        }

        public async Task<IActionResult> Users() => View(await _context.Users.ToListAsync());

        public IActionResult Login() => View(new Login());

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = await _userManger.FindByEmailAsync(login.Email);
            if (user != null)
            {
                if (await _userManger.CheckPasswordAsync(user, login.Password))
                {
                    if ((await _signInManager.PasswordSignInAsync(user, login.Password, false, false)).Succeeded)
                    {
                        if (User.IsInRole("User"))
                            return RedirectToAction("Index", "Appointment");

                        return RedirectToAction("Index", "Doctors");
                    }
                }
            }

            TempData["Error"] = "Email or password is incorrect";
            return View(login);
        }

        public IActionResult Register() => View(new Register());

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (!ModelState.IsValid)
                return View(register);

            if (await _userManger.FindByEmailAsync(register.Email) != null)
            {
                TempData["Error"] = "This email is already in use";
                return View(register);
            }

            var User = new User()
            {
                Name = register.Name,
                Email = register.Email,
                UserName = register.Email,
                EmailConfirmed = true
            };

            var UserResponse = await _userManger.CreateAsync(User, register.Password);
            var UserRoleResponse = await _userManger.AddToRoleAsync(User, UserRole.User);

            return View("CompletedRegister");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Appointment");
        }
    }
}
