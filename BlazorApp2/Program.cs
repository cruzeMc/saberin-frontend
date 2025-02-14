using BlazorApp2;
using BlazorApp2.Services.BlazorApp2.Services;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load appsettings.json into ConfigurationManager
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Register for Dependency Injection
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register configuration in DI
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Configure logging level
builder.Logging.SetMinimumLevel(LogLevel.Debug);

await builder.Build().RunAsync();