using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoviesDomain.Model;

//namespace MoviesDomain.Model;
namespace MoviesInfrastructure;

public partial class MoviesDbContext : DbContext
{
    public MoviesDbContext()
    {
    }

    public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<FilmActor> FilmActors { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Bohdan\\SQLEXPRESS; Database=MoviesDB; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Actor__3214EC07EDCA5E4A");

            entity.ToTable("Actor");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Film__3214EC07AB82547B");

            entity.ToTable("Film");

            entity.Property(e => e.BoxOffice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Director).HasMaxLength(50);
            entity.Property(e => e.Genre).HasMaxLength(50);
            entity.Property(e => e.Runtime).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<FilmActor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FilmActo__3214EC07B91217EC");

            entity.ToTable("FilmActor");

            entity.HasOne(d => d.Actor).WithMany(p => p.FilmActors)
                .HasForeignKey(d => d.ActorId)
                .HasConstraintName("FK__FilmActor__Actor__3F466844");

            entity.HasOne(d => d.Film).WithMany(p => p.FilmActors)
                .HasForeignKey(d => d.FilmId)
                .HasConstraintName("FK__FilmActor__FilmI__3E52440B");

            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3214EC073B9CC2BF");

            entity.ToTable("Review");

            entity.HasOne(d => d.Film).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.FilmId)
                .HasConstraintName("FK__Review__FilmId__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Review__UserId__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07D7FDBCED");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534CC98BA68").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
