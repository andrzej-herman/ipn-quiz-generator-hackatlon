using Microsoft.EntityFrameworkCore;
using QuizGeneratorApp.Database.BaseContext;
using QuizGeneratorApp.Database.DbTools;
using QuizGeneratorApp.Database.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var server = builder.Configuration.GetValue(typeof(string), "DbConnection:Server").ToString();
var db = builder.Configuration.GetValue(typeof(string), "DbConnection:Database").ToString();
var user = builder.Configuration.GetValue(typeof(string), "DbConnection:User").ToString();
var pass = builder.Configuration.GetValue(typeof(string), "DbConnection:Password").ToString();
var connectionString = ConnectionStringCreator.BuildConnectionString(server, db, user, pass);
builder.Services.AddDbContext<IpnQuizContext>(options =>
{
    options.UseSqlServer(connectionString,
        assembly => assembly.MigrationsAssembly(typeof(IpnQuizContext).Assembly.FullName));
});

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();


