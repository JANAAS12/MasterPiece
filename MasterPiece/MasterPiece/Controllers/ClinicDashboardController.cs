using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterPiece.Controllers
{
    public class ClinicDashboardController : Controller
    {

        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClinicDashboardController(IWebHostEnvironment webHostEnvironment, MyDbContext context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        //public IActionResult ViewAppointments()
        //{
        //    return View();
        //}
        public async Task<IActionResult> ViewAppointments(string status = "")
        {
            int? clinicId = HttpContext.Session.GetInt32("ClinicId"); // مؤقتًا لحد ما تربط تسجيل الدخول

            var query = _context.Appointments
                .Include(a => a.ServiceType)
                .Where(a => a.ClinicId == clinicId);

            // تطبيق الفلتر إذا تم تحديد حالة
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            var appointments = await query.ToListAsync();

            // إعداد قائمة الحالات للفلتر
            var statuses = new List<string> { "Pending", "Confirmed", "Completed", "Cancelled" };
            ViewBag.Statuses = statuses;
            ViewBag.SelectedStatus = status;

            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, string status)
        {
            var validStatuses = new[] { "Pending", "Confirmed", "Completed", "Cancelled" };

            if (!validStatuses.Contains(status))
            {
                return BadRequest("Invalid status");
            }

            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewAppointments");
        }
        public async Task<IActionResult> ViewService()
        {
            int? clinicId = HttpContext.Session.GetInt32("ClinicId"); // مؤقتًا - سيتم استبداله بقيمة حقيقية بعد تطبيق نظام المصادقة

            var clinicServices = await _context.Services
                .Where(s => s.ClinicId == clinicId)
                .Include(s => s.ServiceTypes) // تضمين أنواع الخدمات
                .ToListAsync();

            return View(clinicServices);
        }
        // إضافة خدمة جديدة
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            if (ModelState.IsValid)
            {
                service.ClinicId = HttpContext.Session.GetInt32("ClinicId"); // Clinic ID من النظام بعد المصادقة
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewService");
            }
            return View(service);
        }

        // تعديل خدمة
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null || service.ClinicId != HttpContext.Session.GetInt32("ClinicId")) // التحقق من ملكية العيادة للخدمة
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(int id, Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            var existingService = await _context.Services.FindAsync(id);
            if (existingService == null || existingService.ClinicId != HttpContext.Session.GetInt32("ClinicId"))
            {
                return NotFound();
            }

            existingService.Name = service.Name;
            existingService.Description = service.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewService");
        }

        // حذف خدمة
        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services
                .Include(s => s.ServiceTypes)
                .FirstOrDefaultAsync(s => s.Id == id && s.ClinicId == HttpContext.Session.GetInt32("ClinicId"));

            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewService");
        }

        // إضافة نوع خدمة
        public IActionResult AddServiceType(int serviceId)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == serviceId && s.ClinicId == HttpContext.Session.GetInt32("ClinicId"));
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.ServiceName = service.Name;
            ViewBag.PetTypes = new List<string> { "Dog", "Cat", "Bird", "Rabbit", "Hamster", "Other" };
            return View(new ServiceType { ServiceId = serviceId });
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceType(ServiceType serviceType)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/clinics");
            if (ModelState.IsValid)
            {
                var service = await _context.Services
                    .FirstOrDefaultAsync(s => s.Id == serviceType.ServiceId && s.ClinicId == HttpContext.Session.GetInt32("ClinicId"));

                if (service == null)
                {
                    return NotFound();
                }

                // حفظ الصورة إذا تم تحميلها
                // Main Image
                if (serviceType.ImageFile != null)
                {
                    string fileName = Path.GetFileName(serviceType.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        serviceType.ImageFile.CopyTo(stream);
                    }
                    serviceType.Img = fileName;
                }

                _context.ServiceTypes.Add(serviceType);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewService");
            }

            ViewBag.PetTypes = new List<string> { "Dog", "Cat", "Bird", "Rabbit", "Hamster", "Other" };
            return View(serviceType);
        }


        // تعديل نوع خدمة
        public async Task<IActionResult> EditServiceType(int id)
        {
            var serviceType = await _context.ServiceTypes
                .Include(st => st.Service)
                .FirstOrDefaultAsync(st => st.Id == id && st.Service.ClinicId == HttpContext.Session.GetInt32("ClinicId"));

            if (serviceType == null)
            {
                return NotFound();
            }

            ViewBag.ServiceName = serviceType.Service.Name;
            ViewBag.PetTypes = new List<string> { "Dog", "Cat", "Bird", "Rabbit", "Hamster", "Other" };
            return View(serviceType);
        }

        [HttpPost]
        public async Task<IActionResult> EditServiceType(int id, ServiceType serviceType)
        {
            if (id != serviceType.Id)
            {
                return NotFound();
            }

            var existingServiceType = await _context.ServiceTypes
                .Include(st => st.Service)
                .FirstOrDefaultAsync(st => st.Id == id && st.Service.ClinicId == HttpContext.Session.GetInt32("ClinicId"));

            if (existingServiceType == null)
            {
                return NotFound();
            }

            // تحديث الصورة إذا تم تحميل صورة جديدة
            if (serviceType.ImageFile != null && serviceType.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Img");

                // حذف الصورة القديمة إذا كانت موجودة
                if (!string.IsNullOrEmpty(existingServiceType.Img))
                {
                    var oldImagePath = Path.Combine(uploadsFolder, existingServiceType.Img);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
              
                // حفظ الصورة الجديدة
                string fileName = Path.GetFileName(serviceType.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    serviceType.ImageFile.CopyTo(stream);
                }
                existingServiceType.Img = fileName;
            }

            existingServiceType.Name = serviceType.Name;
            existingServiceType.Price = serviceType.Price;
            existingServiceType.PetType = serviceType.PetType;

            await _context.SaveChangesAsync();
            return RedirectToAction("ViewService");
        }
        // حذف نوع خدمة
        [HttpPost]
        public async Task<IActionResult> DeleteServiceType(int id)
        {
            var serviceType = await _context.ServiceTypes
                .Include(st => st.Service)
                .FirstOrDefaultAsync(st => st.Id == id && st.Service.ClinicId == HttpContext.Session.GetInt32("ClinicId"));

            if (serviceType == null)
            {
                return NotFound();
            }

            // حذف الصورة المرتبطة إذا كانت موجودة
            if (!string.IsNullOrEmpty(serviceType.Img))
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Img");
                var imagePath = Path.Combine(uploadsFolder, serviceType.Img);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.ServiceTypes.Remove(serviceType);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewService");
        }

    }
    }

