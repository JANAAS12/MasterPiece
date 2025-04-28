using MasterPiece.Models;
using MasterPiece.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterPiece.Controllers
{
    public class ClinicsController : Controller
    {
        private readonly MyDbContext _context;

        public ClinicsController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.Clinics.ToList());

        }
        [HttpGet]
        public IActionResult GetClinicsByLocation(string location)
        {
            var clinics = _context.Clinics.AsQueryable();

            if (!string.IsNullOrEmpty(location) && location.ToLower() != "all")
            {
                clinics = clinics.Where(c => c.Location.ToLower() == location.ToLower());
            }

            return Json(clinics.ToList());
        }

        [HttpGet]
        public IActionResult SearchClinics(string query)
        {
            var clinics = _context.Clinics
                .Where(c => c.Name.Contains(query))
                .ToList();

            return Json(clinics);
        }


      

  public IActionResult ClinicDetails(int id)
        {
          
            var clinic = _context.Clinics.FirstOrDefault(c => c.Id == id);
            TempData["ClinicId"] = clinic.Id;
            TempData["ClinicName"] = clinic.Name;   
            if (clinic == null)
            {
                return NotFound();
            }
            return View(clinic);


        }


        public IActionResult ClinicServices(int id)
        {
            var serviceTypes = _context.ServiceTypes.Where(s => s.ServiceId == id).ToList();

            if (serviceTypes == null || serviceTypes.Count == 0)
            {
                return NotFound(); 
            }

            return View(serviceTypes);
        }

        //public IActionResult Appointment()
        //{

        //    return View();

        //}

        public IActionResult Appointment(int? serviceId)
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine("UserId", UserId);



            if (UserId == null)
            {
                return RedirectToAction("LoginUser", "User"); // إذا لم يكن هناك جلسة، إعادة توجيه إلى صفحة تسجيل الدخول
            }

            if (serviceId.HasValue)
            {
                var selectedService = _context.ServiceTypes.FirstOrDefault(s => s.Id == serviceId.Value);
                if (selectedService != null)
                {
                    var viewModel = new ServiceTypeAppointment
                    {
                        ServiceId = selectedService.Id,
                        PetType = selectedService.PetType,
                        ServiceName = selectedService.Name,
                        Price = selectedService.Price,
                        Img = selectedService.Img
                    };

                    return View(viewModel);
                }
            }

            return View(new ServiceTypeAppointment()); // في حال ما تم اختيار خدمة
        }

        [HttpPost]
        public IActionResult Appointment(Appointment Formdata)
        {
            int? ownerId = HttpContext.Session.GetInt32("UserId");
            int? clinicId = TempData["ClinicId"] as int?;
            string? clinicName = TempData["ClinicName"] as string;

            var appointment = new Appointment
            {
                OwnerId = ownerId.Value,
                ClinicId = clinicId.Value,
                ClinicName = clinicName,
                OwnerName = Formdata.OwnerName,
                OwnerPhone = Formdata.OwnerPhone,
                PetType = Formdata.PetType,
                ServiceName = Formdata.ServiceName,
                AppointmentDate = Formdata.AppointmentDate,
                AppointmentTime = Formdata.AppointmentTime,
                Description = Formdata.Description,
                Status = "Pending",
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            // إعداد ViewModel للتأكيد
            var service = _context.ServiceTypes.FirstOrDefault(s => s.Name == Formdata.ServiceName);

            var viewModel = new ServiceTypeAppointment
            {
                Id = appointment.Id,
                OwnerName = appointment.OwnerName,
                OwnerPhone = appointment.OwnerPhone,
                PetType = appointment.PetType,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                ServiceName = appointment.ServiceName,
                ClinicName = appointment.ClinicName,
                Price = service?.Price ?? 0,
                Img = service?.Img
            };
            return RedirectToAction("PaymentPage", new { appointmentId = appointment.Id });
            //return View("ConfirmAppointment", viewModel); // توجيه إلى صفحة التأكيد
        }




        //////////////////////////
        //public IActionResult PaymentPage(int appointmentId)
        //{
        //    var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
        //    if (appointment == null)
        //        return NotFound();

        //    // استرجاع بيانات الخدمة
        //    var service = _context.ServiceTypes.FirstOrDefault(s => s.Name == appointment.ServiceName);

        //    var viewModel = new PaymentViewModel
        //    {
        //        AppointmentId = appointment.Id,
        //        OwnerId = appointment.OwnerId ?? 0,
        //        Amount = service?.Price ?? 0, // السعر الحقيقي
        //        AppointmentDate = appointment.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
        //        ClinicName = appointment.ClinicName // اسم العيادة المخزن
        //    };

        //    return View(viewModel);
        //}
        public IActionResult PaymentPage(int appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment == null)
                return NotFound();

            // استرجاع بيانات الخدمة
            var service = _context.ServiceTypes.FirstOrDefault(s => s.Name == appointment.ServiceName);
            decimal basePrice = service?.Price ?? 0;

            // حساب السعر مع إضافة نسبة 20%
            decimal commissionPercentage = 0.20m;
            decimal totalPrice = basePrice + (basePrice * commissionPercentage);

            var viewModel = new PaymentViewModel
            {
                AppointmentId = appointment.Id,
                OwnerId = appointment.OwnerId ?? 0,
                Amount = totalPrice, // السعر النهائي بعد العمولة
                AppointmentDate = appointment.AppointmentDate.ToString("yyyy-MM-dd HH:mm"),
                ClinicName = appointment.ClinicName
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SubmitPayment(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payment = new Payment
                {
                    AppointmentId = model.AppointmentId,
                    OwnerId = model.OwnerId,
                    Amount = model.Amount,
                    PaymentMethod = model.PaymentMethod,
                    PaymentStatus = "Paid"
                };

                _context.Payments.Add(payment);
                _context.SaveChanges();


                 TempData["SuccessMessage"] = "Appointment booked successfully!";
                 return RedirectToAction("Appointment");
            }

            return View("PaymentPage", model);
        }
        public IActionResult Confirmation()
        {
            return View(); // صفحة تأكيد الدفع
        }
        //[HttpPost]
        //public IActionResult Appointment(Appointment Formdata)
        //{
        //    //int ownerId = (int)HttpContext.Session.GetInt32("UserId");
        //    int? ownerId = 1;
        //    int? clinicId = TempData["ClinicId"] as int?;
        //    string? clinicName = TempData["ClinicName"] as string;
        //    var appointment = new Appointment
        //    {
        //        OwnerId = ownerId.Value, // إضافة OwnerId
        //        ClinicId = clinicId.Value, // إضافة ClinicId
        //        ClinicName = clinicName,
        //        OwnerName = Formdata.OwnerName,
        //        OwnerPhone = Formdata.OwnerPhone,
        //        PetType = Formdata.PetType,
        //        ServiceName = Formdata.ServiceName,
        //        AppointmentDate = Formdata.AppointmentDate,
        //        AppointmentTime = Formdata.AppointmentTime,
        //        Description = Formdata.Description,
        //        Status = "Pending",// تعيين الحالة الافتراضية للحجز

        //    };
        //    _context.Appointments.Add(appointment);
        //    _context.SaveChanges();

        //    TempData["SuccessMessage"] = "Appointment booked successfully!";
        //    return RedirectToAction("Appointment");
        //}







    }
}
