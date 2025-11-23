using BookManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.DataAccessLayer.Contexts;

public class BookDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();

    public BookDbContext(DbContextOptions<BookDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var bookEntity = modelBuilder.Entity<Book>();
        bookEntity.ToTable("Books");
        bookEntity.HasKey(b => b.ID);
        bookEntity.Property(b => b.ID)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        bookEntity.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(256);
        bookEntity.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(128);
        bookEntity.Property(b => b.Year)
            .IsRequired();

        var genreEntity = modelBuilder.Entity<Genre>();
        genreEntity.ToTable("Genres");
        genreEntity.HasKey(g => g.ID);
        genreEntity.Property(g => g.ID)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();
        genreEntity.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(128);
        genreEntity.HasIndex(g => g.Name)
            .IsUnique();

        bookEntity
            .HasMany(b => b.Genres)
            .WithMany(g => g.Books)
            .UsingEntity<Dictionary<string, object>>(
                "BookGenres",
                r => r.HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("GenreId")
                    .OnDelete(DeleteBehavior.Cascade),
                l => l.HasOne<Book>()
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("BookGenres");
                    j.HasKey("BookId", "GenreId");
                });

        base.OnModelCreating(modelBuilder);
    }
}
