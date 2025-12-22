using System;
using System.Collections.Generic;

namespace efcore_dbfirst.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;
}
