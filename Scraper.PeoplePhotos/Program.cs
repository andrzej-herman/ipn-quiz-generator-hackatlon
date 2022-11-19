using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scraper.PeoplePhotos.Entities;
using Scraper.PeoplePhotos.Services;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddDbContext<ImagesContext>(x => x.UseSqlServer("Server=MSI-MATI\\SQLEXPRESS;Database=Images;User Id=visualStudio;Password=qwerty12345;"));
        services.AddScoped<IImagesScraper, ImagesScraper>();
    })
    .Build();

GenerateQuestions(host.Services);

await host.RunAsync();

static void ScrapImages(IServiceProvider services)
{
    using IServiceScope serviceScope = services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    IImagesScraper imagesScraper = provider.GetRequiredService<IImagesScraper>();

    imagesScraper.Scrap();
}

static void GenerateQuestions(IServiceProvider services)
{
    using IServiceScope serviceScope = services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    IQuestionsGenerator questionsGenerator = provider.GetRequiredService<IQuestionsGenerator>();

    questionsGenerator.Generate();
}