//using System.ComponentModel.DataAnnotations;
using MasterPiece.Models;
namespace MasterPiece.ViewModel
{
    public class ServiceTypeAppointment
    {
        public int Id { get; set; }

        public int? OwnerId { get; set; }

        public int? ClinicId { get; set; }

        public int? ServiceTypeId { get; set; }

        //[DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        //[DataType(DataType.Time)]
        public DateTime AppointmentTime { get; set; }

        public string? Status { get; set; }

        public string? Description { get; set; }

        public string? OwnerName { get; set; }

        public string? OwnerPhone { get; set; }

        public string? PetType { get; set; }

        public string? ClinicName { get; set; }

        public string? ServiceName { get; set; }

        public int? ServiceId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Img { get; set; }


    }
}
