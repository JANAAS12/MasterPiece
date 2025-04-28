using System;
using System.Collections.Generic;

namespace MasterPiece.Models;

public partial class Chat
{
    public int Id { get; set; }

    public string Message { get; set; } = null!;
}
