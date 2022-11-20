using Microsoft.EntityFrameworkCore;
using QuizGeneratorApp.Database.BaseContext;
using QuizGeneratorApp.Database.Models;
using QuizGeneratorApp.Models;
using System.Text;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace QuizGeneratorApp.Services;

public class QuizGeneratorService : IQuizGeneratorService
{
    public async Task<byte[]> GenerateAsync(QuestionDto[] questionDtos)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < questionDtos.Length; i++)
        {
            sb.Append($"<h3>{i+1}. {questionDtos[i].QuestionTitle}</h3>");
            sb.Append(questionDtos[i].QuestionBody);
            sb.Append("</br>");
        }

        var pdf = PdfGenerator.GeneratePdf(sb.ToString(), PdfSharp.PageSize.A4);
        using var ms = new MemoryStream();
        pdf.Save(ms);
        
        return await Task.FromResult(ms.ToArray());
    }
}
