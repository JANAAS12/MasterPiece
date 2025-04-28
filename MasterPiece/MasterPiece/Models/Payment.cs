using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? AppointmentId { get; set; }

    public int? OwnerId { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual PetOwner? Owner { get; set; }
}
