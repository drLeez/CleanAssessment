using CleanAssessment.Components;
using CleanAssessment.DB;
using CleanAssessment.Domain.Contracts.Repositories;
using CleanAssessment.Helpers;
using CleanAssessment.Managers;
using CleanAssessment.Managers.Customer;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Demo;Trusted_Connection=True;TrustServerCertificate=True;"));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}

builder.Services.AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
builder.Services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

builder.Services.AddMudServices();

if (builder.Services.All(x => x.ServiceType != typeof(HttpClient)))
{
    builder.Services.AddScoped(
        s =>
        {
            var navigationManager = s.GetRequiredService<NavigationManager>();
            return new HttpClient
            {
                BaseAddress = new Uri(navigationManager.BaseUri)
            };
        });
}

builder.Services.AddScoped<IKeyboardHelper, KeyboardHelper>();

builder.Services.AddSecondLayerInterface<IManager>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
app.Run();