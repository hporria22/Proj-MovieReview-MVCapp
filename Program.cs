using Amazon.Runtime;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<MongoDbDatabase>();
builder.Services.AddSingleton<IMovieService,MovieService>();
builder.Services.AddSingleton<IUserService,UserService>();
builder.Services.AddSingleton<IReviewService,ReviewService>();
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireLoggedIn", policy =>
        {
            policy.RequireAuthenticatedUser();
        });
    });

var app = builder.Build();

// builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
// builder.Services.AddSingleton<IMongoDbSettings>(sp=>sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
// builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")));
// builder.Services.AddScoped<IOmdbApiService>((provider) =>
//     {
//         var configuration = provider.GetRequiredService<IConfiguration>();
//         var apiKey = configuration["OMDBApiKey"]; // Get your API key from configuration

//         var httpClient = new HttpClient();
//         return new OmdbApiService(httpClient, apiKey);
//     });
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
