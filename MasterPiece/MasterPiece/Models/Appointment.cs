using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int? OwnerId { get; set; }

    public int? ClinicId { get; set; }

    public int? ServiceTypeId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public DateTime AppointmentTime { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerPhone { get; set; }

    public string? PetType { get; set; }

    public string? ClinicName { get; set; }

    public string? ServiceName { get; set; }

    public virtual Clinic? Clinic { get; set; }

    public virtual PetOwner? Owner { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ServiceType? ServiceType { get; set; }

    public virtual ICollection<TransportRequest> TransportRequests { get; set; } = new List<TransportRequest>();
}
