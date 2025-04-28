using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class SubscriptionPackage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }
}
