using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesDomain.Model;

public partial class Actor: Entity
{

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
    public DateTime Birthdate { get; set; }

    public virtual ICollection<FilmActor> FilmActors { get; set; } = new List<FilmActor>();
}
