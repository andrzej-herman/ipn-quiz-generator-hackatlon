using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuizGeneratorApp.Database.Models;

namespace QuizGeneratorApp.Database.BaseContext;

public partial class IpnQuizContext : DbContext
{
    public IpnQuizContext()
    {
    }

    public IpnQuizContext(DbContextOptions<IpnQuizContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuizUser> QuizUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=mssql-93939-0.cloudclusters.net,19916;Initial Catalog=IPNQuizGenerator;Persist Security Info=False;User ID=rainbowhack;Password=@Rainbow2022@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC144D9702");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
