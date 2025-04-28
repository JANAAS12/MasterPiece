using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Service
{
    public int Id { get; set; }

    public int? ClinicId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Clinic? Clinic { get; set; }

    public virtual ICollection<ServiceType> ServiceTypes { get; set; } = new List<ServiceType>();
}
