using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Localization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tangy_Business.Repository;
using TangyWeb_Client;
using TangyWeb_Client.Service;
using TangyWeb_Client.Service.IService;
using TangyWeb_Client.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderSerivce, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<MyPopup>();
builder.Services.AddScoped<IBlogService, BlogService>(sp =>
{
    var httpClientgateway = new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIGatewayUrl")) };
    return new BlogService(httpClientgateway);
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton(new TextLocalizerService());
await builder.Build().RunAsync();
