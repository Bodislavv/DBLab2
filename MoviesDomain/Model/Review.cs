using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesDomain.Model;

public partial class Review : Entity
{

    public int? UserId { get; set; }

    public int? FilmId { get; set; }

    public string? ReviewText { get; set; }

    [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
    public int? Rating { get; set; }

    public virtual Film? Film { get; set; }

    public virtual User? User { get; set; }
}
