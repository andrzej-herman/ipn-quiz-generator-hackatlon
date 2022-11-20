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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC733FE9FF");

            entity.Property(e => e.SuggestedDifficulty).HasDefaultValueSql("((1))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
