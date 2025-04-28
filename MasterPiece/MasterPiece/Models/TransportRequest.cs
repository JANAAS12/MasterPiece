using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class TransportRequest
{
    public int Id { get; set; }

    public int? AppointmentId { get; set; }

    public string PickupAddress { get; set; } = null!;

    public string DropoffAddress { get; set; } = null!;

    public string? Status { get; set; }

    public virtual Appointment? Appointment { get; set; }
}
