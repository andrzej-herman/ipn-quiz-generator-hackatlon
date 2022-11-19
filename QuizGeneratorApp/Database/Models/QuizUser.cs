using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizGeneratorApp.Database.Models;

[Table("QuizUser")]
[Index("QuizUserEmail", Name = "IX_QuizEmail", IsUnique = true)]
public partial class QuizUser
{
    [Key]
    [StringLength(200)]
    public string QuizUserId { get; set; } = null!;

    [StringLength(200)]
    public string QuizUserEmail { get; set; } = null!;

    [StringLength(200)]
    public string QuizUserFirstName { get; set; } = null!;

    [StringLength(200)]
    public string QuizUserLastName { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;
}
