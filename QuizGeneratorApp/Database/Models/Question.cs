using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizGeneratorApp.Database.Models;

public partial class Question
{
    [Key]
    public int QuestionId { get; set; }

    [StringLength(500)]
    public string QuestionTitle { get; set; } = null!;

    public string QuestionBody { get; set; } = null!;

    [StringLength(500)]
    public string CorrectAnswer { get; set; } = null!;

    public string SearchText { get; set; } = null!;

    public int SuggestedDifficulty { get; set; }
}
