using MasterPiece.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace MasterPiece.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IWebHostEnvironment webHostEnvironment, MyDbContext context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {

            return View();
        }
        public IActionResult AddClinic()
        {
            return View();
        }
        public IActionResult ViewClinics()
        {
            var clinics = _context.Clinics.ToList();
            return View(clinics);
        }
        [HttpPost]
        public IActionResult DeleteClinic(int id)
        {
            var clinic = _context.Clinics.Find(id);
            if (clinic != null)
            {
                _context.Clinics.Remove(clinic);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewClinics");
        }
        public IActionResult ClinicDetails(int id)
        {
            var clinic = _context.Clinics.FirstOrDefault(c => c.Id == id);
            if (clinic == null)
            {
                return NotFound();
            }
            return View(clinic);
        }
        // GET: Edit Clinic
        public IActionResult EditClinic(int id)
        {
            var clinic = _context.Clinics.Find(id);
            if (clinic == null)
            {
                return NotFound();
            }
            return View(clinic);
        }

        // POST: Edit Clinic
        [HttpPost]
        public IActionResult EditClinic(int id, Clinic clinic)
        {
            if (id != clinic.Id)
            {
                return NotFound();
            }

            var existingClinic = _context.Clinics.Find(id);
            if (existingClinic == null)
            {
                return NotFound();
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/clinics");

            // Update main image if new one is provided
            if (clinic.ImageFile != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingClinic.Image))
                {
                    string oldFilePath = Path.Combine(uploadsFolder, existingClinic.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save new image
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(clinic.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    clinic.ImageFile.CopyTo(stream);
                }
                existingClinic.Image = fileName;
            }

            // Update extra images if new ones are provided
            //UpdateClinicImage(clinic.ImageFile1, ref existingClinic.Image1, uploadsFolder);
            //UpdateClinicImage(clinic.ImageFile2, ref existingClinic.Image2, uploadsFolder);
            //UpdateClinicImage(clinic.ImageFile3, ref existingClinic.Image3, uploadsFolder);

            // Update other properties
            existingClinic.Name = clinic.Name;
            existingClinic.Location = clinic.Location;
            existingClinic.WorkingHours = clinic.WorkingHours;
            existingClinic.Category = clinic.Category;
            existingClinic.Description = clinic.Description;
            existingClinic.Phone = clinic.Phone;
            existingClinic.Email = clinic.Email;
            existingClinic.Rating = clinic.Rating;

            _context.SaveChanges();

            return RedirectToAction("ViewClinics");
        }

        private void UpdateClinicImage(IFormFile imageFile, ref string imageProperty, string uploadsFolder)
        {
            if (imageFile != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(imageProperty))
                {
                    string oldFilePath = Path.Combine(uploadsFolder, imageProperty);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save new image
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                imageProperty = fileName;
            }
        }
        //public IActionResult ViewAppointments()
        //{
        //    return View();
        //}
        public IActionResult ViewAppointments(string status = "", string clinicName = "", string dateFilter = "")
        {
            var appointments = _context.Appointments
                .Include(a => a.Clinic)
                .Include(a => a.ServiceType)
                .Include(a => a.Owner)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(status))
            {
                appointments = appointments.Where(a => a.Status == status);
            }

            if (!string.IsNullOrEmpty(clinicName))
            {
                appointments = appointments.Where(a => a.Clinic.Name.Contains(clinicName));
            }

            if (!string.IsNullOrEmpty(dateFilter))
            {
                var today = DateTime.Today;
                switch (dateFilter)
                {
                    case "today":
                        appointments = appointments.Where(a => a.AppointmentDate.Date == today);
                        break;
                    case "upcoming":
                        appointments = appointments.Where(a => a.AppointmentDate.Date >= today);
                        break;
                    case "past":
                        appointments = appointments.Where(a => a.AppointmentDate.Date < today);
                        break;
                }
            }

            // Pass filter values to view
            ViewBag.Statuses = new List<string> { "Pending", "Confirmed", "Completed", "Cancelled" };
            ViewBag.Clinics = _context.Clinics.Select(c => c.Name).Distinct().ToList();

            return View(appointments.OrderByDescending(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).ToList());
        }
        public async Task<IActionResult> ViewUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        public IActionResult AddClinic(Clinic clinic)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/clinics");

            // Main Image
            if (clinic.ImageFile != null)
            {
                string fileName = Path.GetFileName(clinic.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    clinic.ImageFile.CopyTo(stream);
                }
                clinic.Image = fileName;
            }

            // Extra Image 1
            if (clinic.ImageFile1 != null)
            {
                string fileName1 = Path.GetFileName(clinic.ImageFile1.FileName);
                string filePath1 = Path.Combine(uploadsFolder, fileName1);
                using (var stream1 = new FileStream(filePath1, FileMode.Create))
                {
                    clinic.ImageFile1.CopyTo(stream1);
                }
                clinic.Image1 = fileName1;
            }

            // Extra Image 2
            if (clinic.ImageFile2 != null)
            {
                string fileName2 = Path.GetFileName(clinic.ImageFile2.FileName);
                string filePath2 = Path.Combine(uploadsFolder, fileName2);
                using (var stream2 = new FileStream(filePath2, FileMode.Create))
                {
                    clinic.ImageFile2.CopyTo(stream2);
                }
                clinic.Image2 =fileName2;
            }

            // Extra Image 3
            if (clinic.ImageFile3 != null)
            {
                string fileName3 = Path.GetFileName(clinic.ImageFile3.FileName);
                string filePath3 = Path.Combine(uploadsFolder, fileName3);
                using (var stream3 = new FileStream(filePath3, FileMode.Create))
                {
                    clinic.ImageFile3.CopyTo(stream3);
                }
                clinic.Image3 = fileName3;
            }

            // Save to DB
            _context.Clinics.Add(clinic);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

       


    }
}
