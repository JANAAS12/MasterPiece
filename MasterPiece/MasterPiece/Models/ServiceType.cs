using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterPiece.Models;

public partial class ServiceType
{
    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Img { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public string? PetType { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Service? Service { get; set; }
}
