using System;
using System.Collections.Generic;

namespace MoviesDomain.Model;

public partial class FilmActor : Entity
{

    public int? FilmId { get; set; }

    public int? ActorId { get; set; }

    public string? Role {  get; set; }

    public virtual Actor? Actor { get; set; }

    public virtual Film? Film { get; set; }
}
