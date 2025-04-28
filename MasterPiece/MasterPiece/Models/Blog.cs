using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Image { get; set; }

    public int? Likes { get; set; }
}
