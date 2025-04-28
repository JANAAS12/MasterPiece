using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Vet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Location { get; set; }

    public string? Specialization { get; set; }

    public string? Image { get; set; }

    public int? ExperienceYears { get; set; }

    public int? ClinicId { get; set; }

    public virtual Clinic? Clinic { get; set; }
}
