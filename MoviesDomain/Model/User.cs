using System;
using System.Collections.Generic;

namespace MoviesDomain.Model;

public partial class User : Entity
{

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
