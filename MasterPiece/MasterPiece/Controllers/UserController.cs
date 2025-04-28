using MasterPiece.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    public class UserController : Controller
    {

        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult ViewClinics()
        //{

        //    var locations = _context.Clinics.Select(c => c.Location).Distinct().ToList();
        //    return View(new ClinicViewModel { Locations = locations });
        //    //return View(_context.Clinics.ToList());

        //}





        public IActionResult LoginUser()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(PetOwner user, string UserType)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                //ModelState.AddModelError("", "Email and Password are required.");
                return View(user);
            }

            if (UserType == "User")
            {
                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Image = user.Image,
                    Phone = user.Phone,

                };
                _context.Users.Add(newUser);
                _context.SaveChanges();

                _context.UserRoles.Add(new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = 3 // Assuming 1 is the Admin role ID
                });
                _context.SaveChanges();
            }

            return RedirectToAction("LoginUser");


        }



        [HttpPost]
        public IActionResult LoginUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View(user);
            }


            var userInfo = _context.Users.FirstOrDefault(e => e.Email == user.Email);
            if (userInfo != null)
            {
                HttpContext.Session.SetInt32("UserId", userInfo.Id);
                HttpContext.Session.SetString("UserName", userInfo.Name);
                HttpContext.Session.SetString("UserEmail", userInfo.Email);
                //HttpContext.Session.SetString("UserPhone", userInfo.Phone);
                //HttpContext.Session.SetString("ProfileImage", userInfo.Image);

                var userId = _context.Users.FirstOrDefault(e => e.Id == userInfo.Id).Id;

                var userRole = _context.UserRoles.FirstOrDefault(e => e.UserId == userId).RoleId;

                if (userRole == 1)
                {
                    return RedirectToAction( "Dashboard", "Admin");
                }
                else if (userRole == 2)
                {
                    //HttpContext.Session.SetInt32("ClinicId", userInfo.Id);
                    HttpContext.Session.SetString("ClinicEmail", userInfo.Email);
                    var clinicId = _context.Clinics.FirstOrDefault(e => e.Email == userInfo.Email).Id;
                    HttpContext.Session.SetInt32("ClinicId", clinicId);
                    return RedirectToAction("Dashboard", "ClinicDashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Clinics");

                }




            }
            else
            {

                ModelState.AddModelError("", "Email Or Password not correct.");
                return View(user);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // مسح الجلسة
            return RedirectToAction("HomePage", "Home");

        }
        public IActionResult PetOwnerProfile()
        {
            int? PetOwnerId = HttpContext.Session.GetInt32("UserId");

            if (PetOwnerId == null)
            {
                // المستخدم مو مسجّل دخول
                return RedirectToAction("LoginUser", "User");
            }

            var PetOwnerInfo = _context.PetOwners.FirstOrDefault(t => t.Id == PetOwnerId);

            if (PetOwnerInfo == null)
            {
                // احتياطًا، إذا ما لقيناه
                return RedirectToAction("LoginUser", "User");
            }

            return View(PetOwnerInfo);
        }


        public IActionResult POEditProfile()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(e => e.Email == userEmail);

            return View(user);
        }

        public IActionResult POEditProfileHandle(PetOwner updatedUser, IFormFile profilePicture)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(e => e.Email == userEmail);
            if (user != null)
            {
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                user.Phone = updatedUser.Phone;



                // Handle Profile Picture Upload
                if (profilePicture != null)
                {
                    string fileName = Path.GetFileName(profilePicture.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        profilePicture.CopyTo(stream);
                    }

                    user.Image = fileName;
                }

                _context.SaveChanges();
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("ProfileImage", user.Image);
                HttpContext.Session.SetString("UserPhone", user.Phone);
                return RedirectToAction("PetOwnerProfile");

            }


            return View(user);
        }











    }
}
