using Microsoft.EntityFrameworkCore;
using QuizGeneratorApp.Database.BaseContext;
using QuizGeneratorApp.Database.Models;
using QuizGeneratorApp.Models;
using SelectPdf;
using System.Text;

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
        
        HtmlToPdf converter = new HtmlToPdf();
        converter.Options.KeepImagesTogether = true;
        converter.Options.MarginBottom = 40;
        converter.Options.MarginTop = 40;
        converter.Options.MarginLeft = 40;
        converter.Options.MarginRight = 40;

        var doc = converter.ConvertHtmlString(sb.ToString());
        
        return await Task.FromResult(doc.Save());
    }
}
