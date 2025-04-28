using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterPiece.Models;

public partial class Clinic
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public string? WorkingHours { get; set; }

    public int? Rating { get; set; }

    public string Category { get; set; } = null!;

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
    [NotMapped]
    public IFormFile ImageFile1 { get; set; }
    [NotMapped]
    public IFormFile ImageFile2 { get; set; }
    [NotMapped]
    public IFormFile ImageFile3 { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<Vet> Vets { get; set; } = new List<Vet>();
}
