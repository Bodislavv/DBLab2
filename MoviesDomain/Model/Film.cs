using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesDomain.Model;

public partial class Film : Entity
{

    [Required(ErrorMessage = "Title is required.")]
    public string? Title { get; set; } = null!;

    [Required(ErrorMessage = "Runtime is required.")]
    public string? Runtime { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = "Genre is required.")]
    public string? Genre { get; set; }

    [Required(ErrorMessage = "Director is required.")]
    public string? Director { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "BoxOffice is required.")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public decimal? BoxOffice { get; set; }

    public virtual ICollection<FilmActor> FilmActors { get; set; } = new List<FilmActor>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
