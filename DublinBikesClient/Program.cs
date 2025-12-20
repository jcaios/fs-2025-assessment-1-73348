using DublinBikesClient;
using DublinBikesClient.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

// HttpClient for calling API
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("https://localhost:7010/") }
);



// Register API client class
builder.Services.AddScoped<StationsApiClient>();

await builder.Build().RunAsync();

